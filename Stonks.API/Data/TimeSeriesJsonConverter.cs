using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Stonks.API.Models;

namespace Stonks.API.Data
{
    public class TimeSeriesJsonConverter : JsonConverter<TimeSeries>
    {
        public override TimeSeries? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, TimeSeries value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}