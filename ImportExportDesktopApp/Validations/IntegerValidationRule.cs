using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ImportExportDesktopApp.Validations
{
    class IntegerValidationRule : ValidationRule
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {


            ValidationResult result = new ValidationResult(true, null);
            int number = -1;
            if (value != null && value.ToString().Length < 10 && int.TryParse(value.ToString(), out number))
            {
                number = System.Convert.ToInt32(value);
            }
            if (number < Min)
            {
                String message = "Input must be greater than " + Min;
                result = new ValidationResult(false, message);
                return result;
            }
            if (number > Max)
            {
                String message = "Input must be smaller than " + Max;
                result = new ValidationResult(false, message);
                return result;
            }
            return result;
        }
    }
}
