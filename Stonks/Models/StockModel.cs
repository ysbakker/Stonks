using Newtonsoft.Json;

namespace Stonks.Models
{
    public class StockModel
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("open")]
        public string Open { get; set; }

        [JsonProperty("high")]
        public string High { get; set; }

        [JsonProperty("low")]
        public string Low { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("volume")]
        public string Volume { get; set; }

        [JsonProperty("latest")]
        public string Latest { get; set; }

        [JsonProperty("previous")]
        public string Previous { get; set; }

        [JsonProperty("change")]
        public string Change { get; set; }

        [JsonProperty("percent")]
        public string Percent { get; set; }

        public StockModel()
        {
        }

        // public StockModel(string symbol, string name, string price, string change)
        // {
        //     Symbol = symbol;
        //     Name = name;
        //     Price = price;
        //     Change = change;
        // }
    }
}