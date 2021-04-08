// This file was auto-generated by ML.NET Model Builder. 

using Microsoft.ML.Data;

namespace StonksML.Model
{
    public class ModelInput
    {
        [ColumnName("timestamp"), LoadColumn(0)]
        public string Timestamp { get; set; }


        [ColumnName("open"), LoadColumn(1)]
        public float Open { get; set; }


        [ColumnName("high"), LoadColumn(2)]
        public float High { get; set; }


        [ColumnName("low"), LoadColumn(3)]
        public float Low { get; set; }


        [ColumnName("close"), LoadColumn(4)]
        public float Close { get; set; }


        [ColumnName("volume"), LoadColumn(5)]
        public float Volume { get; set; }


    }
}