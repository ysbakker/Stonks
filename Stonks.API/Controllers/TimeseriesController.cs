using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
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
    [Route("timeseries")]
    public class TimeSeriesController : ControllerBase
    {
        private readonly ILogger<TimeSeriesController> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly TimeSeriesRepository _timeseriesRepository;
        
        public TimeSeriesController(ILogger<TimeSeriesController> logger, IConfiguration configuration, TimeSeriesRepository timeseriesRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _timeseriesRepository = timeseriesRepository;
        }

        [HttpGet("{symbol}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TimeSeries>))]
        public async Task<ActionResult> GetBySymbol(string symbol)
        {
            Expression<Func<TimeSeries, bool>> filterBySymbol = (entity) => entity.Symbol == symbol; 

            var timeseries =  await _timeseriesRepository.Get(symbol, filterBySymbol);

           if (timeseries == null || !timeseries.Any())
                return NotFound();

            return Ok(timeseries);
        }
    }
}