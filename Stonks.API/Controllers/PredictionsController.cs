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
using StonksML.Model;

namespace Stonks.API.Controllers
{
    [ApiController]
    [Route("predictions")]
    public class PredictionsController : ControllerBase
    {
        private readonly ILogger<PredictionsController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IGenericRepository<Quote> _quoteRepository;

        public PredictionsController(ILogger<PredictionsController> logger, IConfiguration configuration, IGenericRepository<Quote> quoteRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _quoteRepository = quoteRepository;
        }
        
        [HttpGet("{symbol}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Prediction))]
        public async Task<ActionResult> GetStockItem(string symbol)
        {
            var quote = await _quoteRepository.GetById(symbol);

            if (quote == null)
                return NotFound(symbol);
            
            var sampleData = new ModelInput()
            {
                Open = (float) quote.Open,
                High = (float) quote.High,
                Low = (float) quote.Low,
            };

            var predictionResult = ConsumeModel.Predict(sampleData);

            return Ok(new Prediction(symbol, predictionResult));
        }
    }
}