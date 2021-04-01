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
        private readonly TimeSeriesRepository _timeseriesRepository;
        
        public TimeSeriesController(ILogger<TimeSeriesController> logger, IConfiguration configuration, TimeSeriesRepository timeseriesRepository)
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
        public async Task<IEnumerable<TimeSeries>> GetBySymbol(string symbol)
        {
            // Expression<Func<TimeSeries, bool>> filterByName = (entity) => entity.Symbol == symbol; 
            // var stock = _timeseriesRepository.Get(filter: filterByName);
            //
            // return stock;

            return await _timeseriesRepository.GetAllById(symbol);
        }
    }
}