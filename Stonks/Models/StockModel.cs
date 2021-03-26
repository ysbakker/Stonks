using Newtonsoft.Json;

namespace Stonks.Models
{
    public class StockModel
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("price")]
        public string Price { get; set; }
        [JsonProperty("change")]
        public string Change { get; set; }

        public StockModel()
        {
        }

        public StockModel(string symbol, string name, string price, string change)
        {
            Symbol = symbol;
            Name = name;
            Price = price;
            Change = change;
        }
    }
}