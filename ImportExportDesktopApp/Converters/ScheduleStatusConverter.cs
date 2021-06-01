using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ImportExportDesktopApp.Converters
{
    class ScheduleStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                int status = System.Convert.ToInt32(value);
                if (status == 0)
                {
                    return "Approved";
                }
                else if (status == 1)
                {
                    return "Success";
                }
                else if (status == 2)
                {
                    return "Cancel";
                }
                else if (status == 3)
                {
                    return "Late";
                }
                else if (status == 4)
                {
                    return "Early";
                }
            }
            return "N/A";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
