using System;
using System.Net.Http;
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
    }
}