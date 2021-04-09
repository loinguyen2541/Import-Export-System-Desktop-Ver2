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
* @date - 4/9/2021 8:03:00 AM 
*/

namespace ImportExportDesktopApp.Converters
{
    class TransactionStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int type = (int)value;
            if (type == 0)
            {
                return "Import";
            }
            else
            {
                return "Export";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
