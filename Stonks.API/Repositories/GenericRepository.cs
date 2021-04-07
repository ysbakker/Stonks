using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
        protected readonly StonksContext _context;
        protected readonly DbSet<TEntity> _dbSet;
        protected  readonly IConfiguration _configuration;

        public GenericRepository(StonksContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _dbSet = context.Set<TEntity>();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            var supportedStocks = _configuration.GetSection("SupportedStocks").Get<string[]>();
            foreach (var symbol in supportedStocks)
            {
                // TODO: concurrency?
                _ = await GetById(symbol);
            }
            
            return _dbSet.ToList();
        }

        public async Task<IEnumerable<TEntity>> Get(
            Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }

        public virtual async Task<TEntity> GetById(object id, object otherKeys = null)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                Console.WriteLine("Doing an api call bitches");
                // get the entity from AlphaVantage
                entity = await GetFromExternal(id);

                // if it doesn't exist in AlphaVantage return null
                if (entity == null || !_context.Entry<TEntity>(entity).IsKeySet) return entity;
                
                // add to context and save to DB
                _context.Add(entity);
                Save();
            }
            
            return entity;
        }

        protected virtual async Task<TEntity> GetFromExternal(object id)
        {
            HttpClient httpClient = new HttpClient();
            
            string apiKey = _configuration.GetValue<string>("API_KEY");
            
            // get correct URL from appsettings.Docker.json
            string classname = typeof(TEntity).Name;
            string uri = String.Format(_configuration.GetValue<string>("ExternalUrls:" + classname), id, apiKey);
            
            // TODO: error handling if request fails
            using var httpResponse = await httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            httpResponse.EnsureSuccessStatusCode(); // throws if not 200-299
            
            if (httpResponse.Content.Headers.ContentType?.MediaType == "application/json")
            {
                var contentStream = await httpResponse.Content.ReadAsStreamAsync();

                // Deserialize the JSON to a specific TEntity
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
                            // No need to specify a converter here. Classes that need custom converters are annotated.
                        }
                    );
                    
                    return newEntity;
                }
                catch (Exception exception) when(exception is JsonException || exception is ArgumentException)
                {
                    return null;
                }                
            }

            return null;
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