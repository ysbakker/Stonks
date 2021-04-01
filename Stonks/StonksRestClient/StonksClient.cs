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
            _stonksApi = Device.RuntimePlatform == Device.Android ? "http://10.0.2.2:3000/stocks" : "http://localhost:3000/stocks";
        }

        public async Task<List<StockModel>> GetAllStocks()
        {
            var json = await _httpClient.GetStringAsync(_stonksApi);
            return JsonConvert.DeserializeObject<List<StockModel>>(json);
        }

        public async Task<List<StocksTimeSeriesModel>> GetStockTimeSeries(string symbol)
        {
            var json = await _httpClient.GetStringAsync($"{_stonksApi}/{symbol}/history");
            return JsonConvert.DeserializeObject<List<StocksTimeSeriesModel>>(json);
        }
    }
}