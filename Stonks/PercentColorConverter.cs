using System;
using System.Globalization;
using Xamarin.Forms;

namespace Stonks
{
    public class PercentColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString().Substring(0, 1) == "-" ? "#fe3c30" : "#35c759";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}