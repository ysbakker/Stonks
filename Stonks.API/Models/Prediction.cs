using StonksML.Model;

namespace Stonks.API.Models
{
    public class Prediction
    {
        public Prediction(string symbol, ModelOutput predictionResult)
        {
            Symbol = symbol;
            PredictedValue = predictionResult.Score;
        }

        public string Symbol { get; set; }
        public float PredictedValue { get; set; }
    }
}