using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ImportExportDesktopApp.Converters
{
    class PartnerTypesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<PartnerType> partnerTypes = value as List<PartnerType>;
            String result = "";
            if (partnerTypes != null)
            {
                for (int i = 0; i < partnerTypes.Count; i++)
                {
                    if (i > 0)
                    {
                        result = result + ", ";
                    }
                    result = result + partnerTypes[i].PartnerTypeName;
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
