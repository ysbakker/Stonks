using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
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
        public StockModel Stock { get; }
        public Chart<LineChart> Chart { get; set; }

        public StockDetailsViewModel()
        {
            Stock = new StockModel();
        }

        public StockDetailsViewModel(StockModel stock)
        {
            Stock = stock;
            Chart = null;
            _ = GetChartData();
        }

        private async Task GetChartData()
        {
            List<StocksTimeSeriesModel> history = await _stockServices.GetStockTimeSeries(Stock);
            var labels = history.Select(x => x.Date).ToList();
            var openPrices = history.Select(x => x.Open).ToList();
            var closePrices = history.Select(x => x.Close).ToList();
            var highPrices = history.Select(x => x.High).ToList();
            var lowPrices = history.Select(x => x.Low).ToList();

            LineChart openPricesChart = new(labels, openPrices) {
                Width = 4,
                ChartColor = SKColors.LightGray
            };
            LineChart closePricesChart = new(labels, closePrices) {
                Width = 4,
                ChartColor = SKColors.DarkBlue
            };
            LineChart highPricesChart = new(labels, highPrices) {
                Width = 4,
                ChartColor = SKColors.LightGreen
            };
            LineChart lowPricesChart = new(labels, lowPrices) {
                Width = 4,
                ChartColor = SKColors.LightPink
            };

            Chart = new Chart<LineChart>(new[] {openPricesChart, closePricesChart, highPricesChart, lowPricesChart});

            OnPropertyChanged(nameof(Chart));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}