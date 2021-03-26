﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Stonks.Models;
using Stonks.StonksRestClient;

namespace Stonks.ServicesHandler
{
    public class StockServices
    {
        private StockMap<StockModel> _stocksRest = new StockMap<StockModel>();
        
        public async Task<List<StockModel>> GetStocks()
        {
            return await _stocksRest.GetAllStocks();
        }
    }
}