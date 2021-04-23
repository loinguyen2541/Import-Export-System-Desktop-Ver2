using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ImportExportDesktopApp.Validations
{
    class EmailValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult result = new ValidationResult(true, null);
            if (value != null)
            {
                String mail = value as String;
                Regex mailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = mailRegex.Match(mail);
                if (!match.Success)
                {
                    String message = "Email must be follow the format email@domain.com";
                    result = new ValidationResult(false, message);
                }
            }
            else
            {
                String message = "Email must be follow the format email@domain.com";
                result = new ValidationResult(false, message);
            }
            return result;
        }
    }
}
