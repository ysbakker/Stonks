using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stonks.API.Data;

namespace Stonks.API.Repositories
{
    // https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly StonksContext _context;
        protected readonly DbSet<TEntity> _dbSet;
        protected  readonly IConfiguration _configuration;

        public GenericRepository(StonksContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _dbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return _dbSet.ToList();
        }

        public IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual async Task<TEntity> GetById(object id)
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity == null)
            {
                return await GetFromExternal(id);
            }
            
            Console.WriteLine("Returned entity from DB instead of API!");
            return entity;
        }

        protected virtual async Task<TEntity> GetFromExternal(object id)
        {
            HttpClient httpClient = new HttpClient();
            Console.WriteLine("GOING TO DO AN API CALL BITCHES");
            
            string apiKey = _configuration.GetValue<string>("API_KEY");
            string classname = typeof(TEntity).Name;
            string uri = String.Format(_configuration.GetValue<string>("ExternalUrls:" + classname), id, apiKey);

            Console.WriteLine(uri);
            
            using var httpResponse = await httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            httpResponse.EnsureSuccessStatusCode(); // throws if not 200-299

            // ReSharper disable once PossibleNullReferenceException
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            // ReSharper disable once IsExpressionAlwaysTrue
            if (httpResponse.Content is object && httpResponse.Content.Headers.ContentType.MediaType == "application/json")
            {
                var contentStream = await httpResponse.Content.ReadAsStreamAsync();

                try
                {
                    var newEntity = await System.Text.Json.JsonSerializer.DeserializeAsync<TEntity>(
                        contentStream, 
                        new System.Text.Json.JsonSerializerOptions
                        {
                            IgnoreNullValues = true, 
                            PropertyNameCaseInsensitive = true
                        }
                    );

                    if (_context.Entry<TEntity>(newEntity).IsKeySet)
                    Save();
                    
                    return newEntity;
                }
                catch (JsonException) // Invalid JSON
                {
                    Console.WriteLine("Invalid JSON.");

                    return null;
                }                
            }
            else
            {
                Console.WriteLine("HTTP Response was invalid and cannot be deserialised.");
                return null;
            }
        }

        public void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void InsertMany(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
        }
        
        public void Delete(object id)
        {
            var entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }

        public void Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }

            _dbSet.Remove(entityToDelete);
        }

        public void Update(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}