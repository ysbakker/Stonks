using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stonks.API.Data;
using Stonks.API.Models;

namespace Stonks.API.Controllers
{
    [ApiController]
    [Route("stocks")]
    public class StocksController : ControllerBase
    {
        private readonly StonksContext _context;
        private readonly ILogger<StocksController> _logger;
        private readonly HttpClient httpClient = new HttpClient();

        public StocksController(StonksContext context, ILogger<StocksController> logger)
        {
            _context = context;
            _logger = logger;
            
            HttpClient httpClient = new HttpClient();
        }

        [HttpGet]
        public IEnumerable<Stock> Get()
        {
            Stock stock = new Stock();

            return new[] {new Stock(), new Stock()};
        }
        
        [HttpGet("{symbol}")]
        public async Task<ActionResult<Stock>> GetStockItem(string symbol)
        {
            var stock = await _context.Stocks.FindAsync(symbol);

            if (stock == null)
            {
                string uri = "https://www.alphavantage.co/query?function=OVERVIEW&symbol=IBM";

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
                        return await System.Text.Json.JsonSerializer.DeserializeAsync<Stock>(
                            contentStream, 
                            new System.Text.Json.JsonSerializerOptions
                            {
                                IgnoreNullValues = true, 
                                PropertyNameCaseInsensitive = true
                            }
                        );
                    }
                    catch (JsonException) // Invalid JSON
                    {
                        Console.WriteLine("Invalid JSON.");
                        return NotFound();
                    }                
                }
                else
                {
                    Console.WriteLine("HTTP Response was invalid and cannot be deserialised.");
                }

                return NotFound();
            }

            return stock;
        }
    }
}