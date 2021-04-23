using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

/**
* @author Loi Nguyen
*
* @date - 2/24/2021 2:47:26 PM 
*/

namespace ImportExportDesktopApp.Validations
{
    class PhoneValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult result = new ValidationResult(true, null);
            String valueS = value as String;
            int phone = 0;

            if (valueS == null || valueS.Length != 10 || !int.TryParse(valueS, out phone))
            {
                String message = "Phone must be a numner and phone length must be 10 character";
                result = new ValidationResult(false, message);
            }

            return result;
        }
    }
}
