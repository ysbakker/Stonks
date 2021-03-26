using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stonks.API.Data;
using Stonks.API.Models;
using Stonks.API.Repositories;

namespace Stonks.API.Controllers
{
    [ApiController]
    [Route("stocks")]
    public class StocksController : ControllerBase
    {
        private readonly ILogger<StocksController> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly IGenericRepository<Stock> _stocksRepository;
        
        public StocksController(ILogger<StocksController> logger, IConfiguration configuration, IGenericRepository<Stock> stocksRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _stocksRepository = stocksRepository;
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
            var stock = await _stocksRepository.GetById(symbol);
            
            return stock;
        }
    }
}