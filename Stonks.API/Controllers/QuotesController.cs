using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
using static System.DateTime;

namespace Stonks.API.Controllers
{
    [ApiController]
    [Route("quotes")]
    public class QuotesController : ControllerBase
    {
        private readonly ILogger<QuotesController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IGenericRepository<Quote> _quotesRepository;

        public QuotesController(ILogger<QuotesController> logger, IConfiguration configuration, IGenericRepository<Quote> quotesRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _quotesRepository = quotesRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Quote>))]
        public async Task<ActionResult> Get()
        {
            var quotes = await _quotesRepository.GetAll();
            // TODO: error handling?
            
            return Ok(quotes);
        }
        
        [HttpGet("{symbol}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Quote))]
        public async Task<ActionResult> GetQuoteItem(string symbol)
        {
            var quote = await _quotesRepository.GetById(symbol);
            
            if (quote == null)
                return NotFound(symbol);

            return Ok(quote);
        }
    }
}