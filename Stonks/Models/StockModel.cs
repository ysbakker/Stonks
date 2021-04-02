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

        [JsonProperty("latestTradingDay")]
        public string LatestTradingDay { get; set; }

        [JsonProperty("previousClose")]
        public string PreviousClose { get; set; }

        [JsonProperty("change")]
        public string Change { get; set; }

        [JsonProperty("changePercent")]
        public string ChangePercent { get; set; }
    }
}