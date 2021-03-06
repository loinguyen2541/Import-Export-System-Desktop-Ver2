using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ImportExportDesktopApp.Validations
{
    class MinutesValidationRule : ValidationRule
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult result = new ValidationResult(true, null);
            String valueS = value as String;
            float num;

            if (valueS == null || !float.TryParse(valueS, out num))
            {
                String message = "Time must be a integer number!";
                result = new ValidationResult(false, message);
                return result;
            }
            if (num < Min)
            {
                String message = "Time must be greater than " + Min;
                result = new ValidationResult(false, message);
                return result;
            }
            if (num > Max)
            {
                String message = "Time must be smaller than " + Max;
                result = new ValidationResult(false, message);
                return result;
            }
            return result;
        }
    }
}
