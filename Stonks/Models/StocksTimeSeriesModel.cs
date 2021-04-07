using System.Linq.Expressions;
using Newtonsoft.Json;

namespace Stonks.Models
{
    public class StocksTimeSeriesModel
    {
        [JsonProperty("timeStamp")]
        public string Date { get; set; }
        [JsonProperty("open")]
        public float Open { get; set; }
        [JsonProperty("close")]
        public float Close { get; set; }
        [JsonProperty("high")]
        public float High { get; set; }
        [JsonProperty("low")]
        public float Low { get; set; }
        [JsonProperty("volume")]
        public int Volume { get; set; }
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
    }
}