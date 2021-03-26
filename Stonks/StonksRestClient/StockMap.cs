using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Stonks.StonksRestClient
{
    public class StockMap<T>
    {
        private readonly string _stonksApi;
        private readonly HttpClient _httpClient = new();

        public StockMap()
        {
            _stonksApi = Device.RuntimePlatform == Device.Android ? "http://10.0.2.2:3000/stocks" : "http://localhost:3000/stocks";
        }

        public async Task<List<T>> GetAllStocks()
        {
            var json = await _httpClient.GetStringAsync(_stonksApi);
            return JsonConvert.DeserializeObject<List<T>>(json);
        }
    }
}