using Newtonsoft.Json;

namespace Stonks.Models
{
    public class StockPredictionModel
    {
        public string Symbol { get; set; }

        public float PredictedValue { get; set; }
    }
}