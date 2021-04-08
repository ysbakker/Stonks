using System.Collections.Generic;
using System.Threading.Tasks;
using Stonks.Models;
using Stonks.StonksRestClient;

namespace Stonks.ServicesHandler
{
    public class StockServices
    {
        private readonly StonksClient _stocksRest = new();
        
        public async Task<List<StockModel>> GetStocks()
        {
            return await _stocksRest.GetAllStocks();
        }

        public async Task<List<StocksTimeSeriesModel>> GetStockTimeSeries(StockModel stock)
        {
            return await _stocksRest.GetStockTimeSeries(stock.Symbol);
        }

        public async Task<StockPredictionModel> GetStockPrediction(StockModel stock)
        {
            return await _stocksRest.GetStockPrediction(stock.Symbol);
        }
    }
}