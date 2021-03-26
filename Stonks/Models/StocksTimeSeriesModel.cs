using System.Linq.Expressions;
using Newtonsoft.Json;

namespace Stonks.Models
{
    public class StocksTimeSeriesModel
    {
        [JsonProperty("date")]
        public string Date { get; set; }
        [JsonProperty("open")]
        public float Open { get; set; }
        [JsonProperty("close")]
        public float Close { get; set; }
        [JsonProperty("high")]
        public float High { get; set; }
        [JsonProperty("low")]
        public float Low { get; set; }

        public StocksTimeSeriesModel()
        {
        }

        public StocksTimeSeriesModel(string date, float open, float close, float high, float low)
        {
            Date = date;
            Open = open;
            Close = close;
            High = high;
            Low = low;
        }
    }
}