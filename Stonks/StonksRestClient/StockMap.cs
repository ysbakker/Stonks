using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Stonks.StonksRestClient
{
    public class StockMap<T>
    {
        private const string StonksApi = "http://10.0.2.2:3000/stocks";
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<List<T>> GetAllStocks()
        {
            var json = await _httpClient.GetStringAsync(StonksApi);
            return JsonConvert.DeserializeObject<List<T>>(json);
        }
    }
}