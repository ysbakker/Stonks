using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Stonks.API.Models;

namespace Stonks.API.Data
{
    public class QuoteJsonConverter : JsonConverter<Quote>
    {
        // map API keys to entity keys
        private readonly Dictionary<string, string> _propertyMappings = new Dictionary<string, string>
        {
            {"01. symbol", "Symbol"},
            {"02. open", "Open"},
            {"03. high", "High"},
            {"04. low", "Low"},
            {"05. price", "Price"},
            {"06. volume", "Volume"},
            {"07. latest trading day", "LatestTradingDay"},
            {"08. previous close", "PreviousClose"},
            {"09. change", "Change"},
            {"10. change percent", "ChangePercent"}
        };
        
        public override Quote? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException("Expected StartObject token");

            var quote = new Quote();
            
            // register current depth to fix https://stackoverflow.com/a/62155881
            var startDepth = reader.CurrentDepth;
            while (reader.Read())
            {
                string propName;
                if (reader.TokenType == JsonTokenType.PropertyName &&  _propertyMappings.TryGetValue(reader.GetString(), out propName))
                {
                    reader.Read();
                }
                else
                {
                    reader.Read();
                    continue;
                }
                
                switch (propName)
                {
                    case nameof(Quote.Symbol):
                        quote.Symbol = reader.GetString();
                        break;
                    case nameof(Quote.Open):
                        quote.Open = decimal.Parse(reader.GetString());
                        break;
                    case nameof(Quote.High):
                        quote.High = decimal.Parse(reader.GetString());
                        break;
                    case nameof(Quote.Low):
                        quote.Low = decimal.Parse(reader.GetString());
                        break;
                    case nameof(Quote.Price):
                        quote.Price = decimal.Parse(reader.GetString());
                        break;
                    case nameof(Quote.Volume):
                        quote.Volume = long.Parse(reader.GetString());
                        break;
                    case nameof(Quote.LatestTradingDay):
                        quote.LatestTradingDay = DateTime.Parse(reader.GetString());
                        break;
                    case nameof(Quote.Change):
                        quote.Change = decimal.Parse(reader.GetString());
                        break;
                    case nameof(Quote.ChangePercent):
                        quote.ChangePercent = reader.GetString();
                        break;
                    case nameof(Quote.PreviousClose):
                        quote.PreviousClose = decimal.Parse(reader.GetString());
                        break;
                    default:
                        continue;
                }
            }
            // return if we see a a closing bracket and we're back at the start of the object
            if (reader.TokenType == JsonTokenType.EndObject && reader.CurrentDepth == startDepth)
            {
                if (quote.Symbol == null)
                    throw new ArgumentException();
                return quote;
            }

            throw new JsonException("Expected EndObject token");
        }

        public override void Write(Utf8JsonWriter writer, Quote value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            
            writer.WriteString("symbol", value.Symbol);
            writer.WriteNumber("open", value.Open);
            writer.WriteNumber("high", value.High);
            writer.WriteNumber("low", value.Low);
            writer.WriteNumber("price", value.Price);
            writer.WriteNumber("volume", value.Volume);
            writer.WriteString("latestTradingDay", value.LatestTradingDay);
            writer.WriteNumber("change", value.Change);
            writer.WriteString("changePercent", value.ChangePercent);

            writer.WriteEndObject();
        }
    }
}