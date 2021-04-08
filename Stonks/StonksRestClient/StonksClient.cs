using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Stonks.Models;
using Xamarin.Forms;

namespace Stonks.StonksRestClient
{
    public class StonksClient
    {
        private readonly string _stonksApi;
        private readonly HttpClient _httpClient = new();

        public StonksClient()
        {
            _stonksApi = Device.RuntimePlatform == Device.Android ? "http://10.0.2.2:4000" : "http://localhost:4000";
        }

        public async Task<List<StockModel>> GetAllStocks()
        {
            var json = await _httpClient.GetStringAsync($"{_stonksApi}/quotes/");
            return JsonConvert.DeserializeObject<List<StockModel>>(json);
        }

        public async Task<List<StocksTimeSeriesModel>> GetStockTimeSeries(string symbol)
        {
            var json = await _httpClient.GetStringAsync($"{_stonksApi}/timeseries/{symbol}/");
            return JsonConvert.DeserializeObject<List<StocksTimeSeriesModel>>(json);
        }

        public async Task<StockPredictionModel> GetStockPrediction(string symbol)
        {
            var json = await _httpClient.GetStringAsync($"{_stonksApi}/predictions/{symbol}/");
            return JsonConvert.DeserializeObject<StockPredictionModel>(json);
        }
    }
}