// This file was auto-generated by ML.NET Model Builder. 

using System;
using StonksML.Model;

namespace StonksML.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create single instance of sample data from first line of dataset for model input
            ModelInput sampleData = new ModelInput()
            {
                Open = 126.5F,
                High = 127.13F,
                Low = 125.65F,
            };

            // Make a single prediction on the sample data and print results
            var predictionResult = ConsumeModel.Predict(sampleData);

            Console.WriteLine("Using model to make single prediction -- Comparing actual Close with predicted Close from sample data...\n\n");
            Console.WriteLine($"Open: {sampleData.Open}");
            Console.WriteLine($"High: {sampleData.High}");
            Console.WriteLine($"Low: {sampleData.Low}");
            Console.WriteLine($"\n\nPredicted Close: {predictionResult.Score}\n\n");
            Console.WriteLine("=============== End of process, hit any key to finish ===============");
            Console.ReadKey();
        }
    }
}