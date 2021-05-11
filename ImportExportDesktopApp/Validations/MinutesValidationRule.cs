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
                String message = "Thời gian phải là một số nguyên!";
                result = new ValidationResult(false, message);
                return result;
            }
            if (num < Min)
            {
                String message = "Thời gian phải lớn hơn " + Min;
                result = new ValidationResult(false, message);
                return result;
            }
            if (num > Max)
            {
                String message = "Thời gian phải nhỏ hơn " + Max;
                result = new ValidationResult(false, message);
                return result;
            }
            return result;
        }
    }
}
