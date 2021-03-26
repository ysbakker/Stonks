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
        private readonly string StonksApi;
        private readonly HttpClient _httpClient = new HttpClient();

        public StockMap()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                StonksApi = "http://10.0.2.2:3000/stocks";
            }
            else 
            {
                StonksApi = "http://localhost:3000/stocks"; 
            }
        }

        public async Task<List<T>> GetAllStocks()
        {
            var json = await _httpClient.GetStringAsync(StonksApi);
            return JsonConvert.DeserializeObject<List<T>>(json);
        }
    }
}