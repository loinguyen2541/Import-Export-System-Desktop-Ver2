using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ImportExportDesktopApp.Validations
{
    public class FloatValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult result = new ValidationResult(true, null);
            String valueS = value as String;
            float weight;

            if (valueS == null || valueS.Length != 10 || !float.TryParse(valueS, out weight))
            {
                String message = "Weight information must be Float type";
                result = new ValidationResult(false, message);
            }
            else
            {
                if (weight < 1)
                {
                    String message = "Weight information must be greater than 0";
                    result = new ValidationResult(false, message);
                }
            }

            return result;
        }
    }
}
