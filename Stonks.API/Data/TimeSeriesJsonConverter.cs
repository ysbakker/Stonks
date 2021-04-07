using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Stonks.API.Models;

namespace Stonks.API.Data
{
    public class TimeSeriesJsonConverter : JsonConverter<IEnumerable<TimeSeries>>
    {
        private readonly Dictionary<string, string> _propertyMappings = new Dictionary<string, string>
        {
            {"2. Symbol", "Symbol"},
            {"1. open", "Open"},
            {"2. high", "High"},
            {"3. low", "Low"},
            {"4. close", "Close"},
            {"5. volume", "Volume"},
        };
        
        public override IEnumerable<TimeSeries>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException("Expected StartObject token");

            var series = new TimeSeries();
            var fullseries = new List<TimeSeries>();
            
            // register current depth to fix https://stackoverflow.com/a/62155881
            var startDepth = reader.CurrentDepth;
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.PropertyName &&
                    DateTime.TryParse(reader.GetString(), out var test))
                {
                    series.TimeStamp = test;
                }

                if (reader.TokenType == JsonTokenType.EndObject && reader.CurrentDepth != startDepth && series.TimeStamp != DateTime.MinValue)
                {
                    var d = series.TimeStamp;
                    fullseries.Add(new TimeSeries()
                    {
                        TimeStamp = d,
                        Open = series.Open,
                        High = series.High,
                        Low = series.Low,
                        Close = series.Close,
                        Volume = series.Volume,
                        Symbol = series.Symbol
                    });
                    series.TimeStamp = DateTime.MinValue;
                }

                if (reader.TokenType == JsonTokenType.PropertyName &&  _propertyMappings.TryGetValue(reader.GetString(), out var propName))
                {
                    reader.Read();
                }
                else
                {
                    continue;
                }
                
                switch (propName)
                {
                    case nameof(TimeSeries.Open):
                        series.Open = decimal.Parse(reader.GetString());
                        break;
                    case nameof(TimeSeries.High):
                        series.High = decimal.Parse(reader.GetString());
                        break;
                    case nameof(TimeSeries.Low):
                        series.Low = decimal.Parse(reader.GetString());
                        break;
                    case nameof(TimeSeries.Close):
                        series.Close = decimal.Parse(reader.GetString());
                        break;
                    case nameof(TimeSeries.Volume):
                        series.Volume = long.Parse(reader.GetString());
                        break;
                    case nameof(TimeSeries.Symbol):
                        series.Symbol = reader.GetString();
                        break;
                    default:
                        continue;
                }
            }
            // return if we see a a closing bracket and we're back at the start of the object
            if (reader.TokenType == JsonTokenType.EndObject && reader.CurrentDepth == startDepth && reader.IsFinalBlock)
            {
                if (fullseries.Count == 0)
                    throw new ArgumentException();
                return fullseries;
            }

            throw new JsonException("Expected EndObject token");
        }

        public override void Write(Utf8JsonWriter writer, IEnumerable<TimeSeries> value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}