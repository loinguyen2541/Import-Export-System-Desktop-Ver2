using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ImportExportDesktopApp.Converters
{
    class TransactionStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int status = System.Convert.ToInt32(value);
            if (status == 0)
            {
                return "Processing";
            }
            else if (status == 1)
            {
                return "Success";
            }
            else
            {
                return "Disable";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
