using System.Linq.Expressions;
using Newtonsoft.Json;

namespace Stonks.Models
{
    public class StocksTimeSeriesModel
    {
        [JsonProperty("timeStamp")]
        public string Date { get; set; }
        public float Open { get; set; }
        public float Close { get; set; }
        public float High { get; set; }
        public float Low { get; set; }
        public int Volume { get; set; }
        public string Symbol { get; set; }
    }
}