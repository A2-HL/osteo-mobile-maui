using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsteoMAUIApp.Converters
{
    public class SelectedBorderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int selectedValue && parameter is string paramString && int.TryParse(paramString, out int paramValue))
            {
                return selectedValue == paramValue ? "#00d9bc" : "Transparent"; // Highlight selected button
            }
            return "Transparent";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null; // One-way binding, no need for ConvertBack
        }
    }
}
