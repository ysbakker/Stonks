using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using SkiaChart;
using SkiaChart.Charts;
using SkiaSharp;
using Stonks.Models;
using Stonks.ServicesHandler;

namespace Stonks.ViewModels
{
    public class StockDetailsViewModel : INotifyPropertyChanged
    {
        private readonly StockServices _stockServices = new();
        public SKColor GridColor { get; }
        public StockModel Stock { get; }
        public Chart<LineChart> Chart { get; set; }

        public StockDetailsViewModel()
        {
            Stock = new StockModel();
        }

        public StockDetailsViewModel(StockModel stock)
        {
            GridColor = SKColors.LightGray;
            Stock = stock;
            Chart = null;
            _ = GetChartData();
        }

        private async Task GetChartData()
        {
            List<StocksTimeSeriesModel> history = await _stockServices.GetStockTimeSeries(Stock);
            var labels = history.Select(x => x.Date).Reverse().ToList();
            var openPrices = history.Select(x => x.Open).Reverse().ToList();
            var closePrices = history.Select(x => x.Close).Reverse().ToList();
            var highPrices = history.Select(x => x.High).Reverse().ToList();
            var lowPrices = history.Select(x => x.Low).Reverse().ToList();

            LineChart openPricesChart = new(labels, openPrices) {
                ChartName = "open",
                Width = 4,
                ChartColor = SKColors.LightGray
            };
            LineChart closePricesChart = new(labels, closePrices) {
                ChartName = "close",
                Width = 4,
                ChartColor = SKColors.DarkBlue
            };
            LineChart highPricesChart = new(labels, highPrices) {
                ChartName = "high",
                Width = 4,
                ChartColor = SKColors.LightGreen
            };
            LineChart lowPricesChart = new(labels, lowPrices) {
                ChartName = "low",
                Width = 4,
                ChartColor = SKColors.LightPink
            };

            Chart = new Chart<LineChart>(new[] {openPricesChart, highPricesChart, lowPricesChart, closePricesChart})
            {
                XTitle = "Date",
                YTitle = "Price"
            };

            OnPropertyChanged(nameof(Chart));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}