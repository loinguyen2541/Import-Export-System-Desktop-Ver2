using System;
using System.Globalization;
using System.Windows.Controls;

/**
* @author Loi Nguyen
*
* @date - 2/22/2021 11:39:41 AM 
*/

namespace ImportExportDesktopApp.Validations
{
    class NotEmptyValidationRule : ValidationRule
    {
        public int Min { get; set; }
        public int Max { get; set; }

        public NotEmptyValidationRule()
        {
            Min = 0;
            Max = 0;
        }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult result = new ValidationResult(true, null);
            String valueS = value as String;
            if (valueS == null || valueS.Length < Min || valueS.Length > Max)
            {
                String message = String.Format("The length of the value must be between {0} and {1}", Min, Max);
                result = new ValidationResult(false, message);
            }
            return result;
        }
    }
}
