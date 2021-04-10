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
            if (value.Equals("Approved"))
            {
                return "#ffc107";
            }
            if (value.Equals("Cancel"))
            {
                return "#c62828";
            }
            if (value.Equals("Success"))
            {
                return "#00c853";
            }
            if (value.Equals(true))
            {
                return "#c62828";
            }
            else
            {
                return "#00c853";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
