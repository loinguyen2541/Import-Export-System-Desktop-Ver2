using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ImportExportDesktopApp.Converters
{
    class TotalWeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Transaction transaction = value as Transaction;
            if (transaction.TransactionStatus == 1)
            {
                if ((transaction.WeightIn - transaction.WeightOut) < 0)
                {
                    return (transaction.WeightIn - transaction.WeightOut) * -1 + " kg";
                }
                return (transaction.WeightIn - transaction.WeightOut) + " kg";
            }
            return "--";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
