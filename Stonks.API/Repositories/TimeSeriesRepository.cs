using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
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
        
        public async Task<IEnumerable<TimeSeries>> GetAllById(string symbol)
        {
            var timeSeries = base.GetAll();
            
            if (timeSeries.ToList().Count == 0)
            {
                HttpClient httpClient = new HttpClient();
                Console.WriteLine("GOING TO DO AN API CALL BITCHES");
                
                string apiKey = _configuration.GetValue<string>("API_KEY");
                string uri = String.Format(_configuration.GetValue<string>("ExternalUrls:TimeSeries"), symbol, apiKey);

                Console.WriteLine(uri);
                
                var temp = await httpClient.GetFromJsonAsync<IEnumerable<TimeSeries>>(
                    uri, 
                    new System.Text.Json.JsonSerializerOptions
                    {
                        IgnoreNullValues = true, 
                        PropertyNameCaseInsensitive = true,
                    });

                // if (temp != null && temp.RootElement.GetArrayLength() == 0) return timeSeries;
                //
                // JsonElement meta = temp.RootElement.GetProperty("Meta Data");
                // JsonElement data = temp.RootElement.GetProperty("Time Series (Daily)");
                // // 
                // foreach (var property in data.EnumerateObject().Where(prop => DateTime.Parse(prop.Name) > DateTime.Parse("2021-01-01")))
                // {
                //     JsonElement company = meta.GetProperty("Symbol");
                //     // var newEntry = property.Value.EnumerateObject().Cast<TimeSeries>().ToList();
                //     // timeSeries.Append(property.Value);
                // }

            }

            return timeSeries;
        } 
    }
}