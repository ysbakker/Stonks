using System;
using System.Globalization;
using Xamarin.Forms;

namespace Stonks.Assets
{
    public class PercentColorConverter : IValueConverter
    {
        OSAppTheme currentTheme = Application.Current.RequestedTheme;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString().Substring(0, 1) == "-")
                if (currentTheme.Equals(OSAppTheme.Dark))
                    return "#FE3C2F";
                else
                    return "#FE3C30";
            else
                if (currentTheme.Equals(OSAppTheme.Dark))
                    return "#34C759";
                else
                    return "#35C759";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}