using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
    [Route("timeseries")]
    public class TimeSeriesController : ControllerBase
    {
        private readonly ILogger<TimeSeriesController> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly IGenericRepository<TimeSeries> _timeseriesRepository;
        
        public TimeSeriesController(ILogger<TimeSeriesController> logger, IConfiguration configuration, IGenericRepository<TimeSeries> timeseriesRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _timeseriesRepository = timeseriesRepository;
        }

        [HttpGet]
        public IEnumerable<TimeSeries> Get()
        {
            TimeSeries stock = new TimeSeries();

            return new[] {new TimeSeries(), new TimeSeries()};
        }
        
        [HttpGet("{symbol}")]
        public async Task<ActionResult<TimeSeries>> GetBySymbol(string symbol)
        {
            return await _timeseriesRepository.GetById(symbol);
        }
    }
}