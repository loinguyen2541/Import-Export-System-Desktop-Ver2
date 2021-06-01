using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ImportExportDesktopApp.Converters
{
    class StatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.Equals("Approved") || value.Equals(0))
            {
                return "#ffc107";
            }
            if (value.Equals("Cancel") || value.Equals(2))
            {
                return "#c62828";
            }
            if (value.Equals("Success") || value.Equals(1))
            {
                return "#00c853";
            }
            if (value.Equals(true))
            {
                return "#c62828";
            }
            else
            {
                return "#2196F3";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
