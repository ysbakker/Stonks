using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        private readonly IGenericRepository<Stock> _stocksRepository;

        public StocksController(ILogger<StocksController> logger, IConfiguration configuration, IGenericRepository<Stock> stocksRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _stocksRepository = stocksRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Stock>))]
        public async Task<ActionResult> Get()
        {
            var stocks = await _stocksRepository.GetAll();
            if (stocks == null || !stocks.Any())
                return NotFound();
            
            return Ok(stocks);
        }
        
        [HttpGet("{symbol}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Stock>> GetStockItem(string symbol)
        {
            var stock = await _stocksRepository.GetById(symbol);

            if (stock == null)
                return NotFound(symbol);

            return stock;
        }
    }
}