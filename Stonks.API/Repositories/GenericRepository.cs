using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stonks.API.Data;
using Stonks.API.Models;

namespace Stonks.API.Repositories
{
    // https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly StonksContext _context;
        protected readonly DbSet<TEntity> _dbSet;
        protected  readonly IConfiguration _configuration;
        private readonly JsonConverter _converter;

        public GenericRepository(StonksContext context, IConfiguration configuration, JsonConverter<TEntity> converter)
        {
            _context = context;
            _configuration = configuration;
            _dbSet = context.Set<TEntity>();
            _converter = converter;
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
                entity = await GetFromExternal(id);

                Console.WriteLine(entity);
                
                if (_context.Entry<TEntity>(entity).IsKeySet)
                {
                    _context.Add(entity);
                    Save();
                }
            }
            
            return entity;
        }

        protected virtual async Task<TEntity> GetFromExternal(object id)
        {
            HttpClient httpClient = new HttpClient();
            Console.WriteLine("GOING TO DO AN API CALL BITCHES");
            
            string apiKey = _configuration.GetValue<string>("API_KEY");
            string classname = typeof(TEntity).Name;
            string uri = String.Format(_configuration.GetValue<string>("ExternalUrls:" + classname), id, apiKey);
            
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
                            PropertyNameCaseInsensitive = true,
                            NumberHandling =
                                JsonNumberHandling.AllowReadingFromString |
                                JsonNumberHandling.WriteAsString,
                            WriteIndented = true,
                            Converters = { _converter },
                        }
                    );
                    
                    /*var t = newEntity.RootElement.GetProperty("Global Quote").ToString();
                    var serializedEntity = System.Text.Json.JsonSerializer.Deserialize<TEntity>(
                        t,
                        new System.Text.Json.JsonSerializerOptions
                        {
                            IgnoreNullValues = true, 
                            PropertyNameCaseInsensitive = true,
                            NumberHandling =
                                JsonNumberHandling.AllowReadingFromString |
                                JsonNumberHandling.WriteAsString,
                            WriteIndented = true,
                            Converters = { _converter }
                        }
                    );*/
                    
                    return newEntity;
                }
                catch (JsonException jsonException) // Invalid JSON
                {
                    Console.WriteLine("Invalid JSON.");

                    Console.WriteLine(jsonException.Message);

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