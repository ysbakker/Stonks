using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SkiaChart;
using SkiaChart.Charts;
using Stonks.Models;

namespace Stonks.ViewModels
{
    public class StockDetailsViewModel
    {
        public StockModel Stock { get; }
        public bool ShowChart { get; }
        private List<string> Labels { get; }
        private List<float> Values { get; }
        public Chart<LineChart> Chart { get; }

        public StockDetailsViewModel()
        {
            Stock = new StockModel();
        }

        public StockDetailsViewModel(StockModel stock)
        {
            _ = GetChartData();
            Labels = new List<string>();
            Values = new List<float>();
            ShowChart = false;
            Stock = stock;
            Chart = null;
        }

        private async Task GetChartData()
        {
            HttpClient client = new();
            HttpResponseMessage response = await client.GetAsync("http://localhost:3000/stocks/TSLA/history");
            response.EnsureSuccessStatusCode();
            LineChart lineChart = new(Labels, Values);
        }
    }
}