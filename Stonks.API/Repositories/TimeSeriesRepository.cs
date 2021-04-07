using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stonks.API.Data;
using Stonks.API.Models;

namespace Stonks.API.Repositories
{
    public class TimeSeriesRepository : GenericRepository<TimeSeries>
    {
        public TimeSeriesRepository(StonksContext context, IConfiguration configuration) : base(context, configuration)
        {
        }
        
        public async Task<IEnumerable<TimeSeries>> Get(string symbol,
            Expression<Func<TimeSeries, bool>> filter)
        {
            IQueryable<TimeSeries> query = _dbSet;
            bool shouldUpdate = false;
            bool hasPreviousEntries = _dbSet.Any(filter);

            if (hasPreviousEntries)
            {
                var latestEntry = await _dbSet.Where(filter).OrderByDescending(ts => ts.TimeStamp).FirstOrDefaultAsync();

                if ((DateTime.Now - latestEntry.TimeStamp).TotalMinutes > 5 && latestEntry.TimeStamp.Day == DateTime.Today.Day)
                {
                    shouldUpdate = true;
                }
            }
            
            if (shouldUpdate || !hasPreviousEntries)
            {
                Console.WriteLine("External call!");
                var entities = await GetFromExternals(symbol);
                if (entities == null) return null;

                // timeseries are tracked by day
                // so we need to manually upsert to avoid duplicates
                foreach (var entity in entities)
                {
                    Console.WriteLine(entity.TimeStamp);
                    if (!_dbSet.Any(e => e.TimeStamp == entity.TimeStamp && e.Symbol == entity.Symbol))
                    {
                        await _dbSet.AddAsync(entity);
                    }
                }
                Save();
            }
            
            return await query.Where(filter).OrderBy(t => t.TimeStamp).ToListAsync();
        }
        
        private async Task<IEnumerable<TimeSeries>> GetFromExternals(object id)
        {
            HttpClient httpClient = new HttpClient();
            
            string apiKey = _configuration.GetValue<string>("API_KEY");
            
            // get correct URL from appsettings.Docker.json
            string classname = typeof(TimeSeries).Name;
            Console.WriteLine(classname);
            
            string uri = String.Format(_configuration.GetValue<string>("ExternalUrls:" + classname), id, apiKey);
            Console.WriteLine(uri);

            // TODO: error handling if request fails
            using var httpResponse = await httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            httpResponse.EnsureSuccessStatusCode(); // throws if not 200-299
            
            if (httpResponse.Content.Headers.ContentType?.MediaType == "application/json")
            {
                var contentStream = await httpResponse.Content.ReadAsStreamAsync();

                // Deserialize the JSON to a specific TimeSeries
                try
                {
                    var newEntity = await System.Text.Json.JsonSerializer.DeserializeAsync<IEnumerable<TimeSeries>>(
                        contentStream, 
                        new System.Text.Json.JsonSerializerOptions
                        {
                            IgnoreNullValues = true, 
                            PropertyNameCaseInsensitive = true,
                            NumberHandling =
                                JsonNumberHandling.AllowReadingFromString |
                                JsonNumberHandling.WriteAsString,
                            WriteIndented = true,
                            Converters = { new TimeSeriesJsonConverter() }
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
    }
}