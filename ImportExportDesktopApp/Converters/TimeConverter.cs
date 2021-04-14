using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

/**
* @author Loi Nguyen
*
* @date - 4/13/2021 9:47:45 PM 
*/

namespace ImportExportDesktopApp.Converters
{
    class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime dateTime = System.Convert.ToDateTime(value);
            return dateTime.Hour + ":" + dateTime.Minute;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
