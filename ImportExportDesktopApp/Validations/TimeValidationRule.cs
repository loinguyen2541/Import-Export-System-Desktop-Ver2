using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ImportExportDesktopApp.Validations
{
    class TimeValidationRule : ValidationRule
    {
        private TimeValidationRuleRange _range;

        public String Min { get; set; }
        public String Max { get; set; }

        public TimeValidationRuleRange Range
        {
            get { return _range; }
            set
            {
                _range = value;
                value?.SetValidator(this);
            }
        }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult result = new ValidationResult(true, null);
            DateTime dateTime;
            if (DateTime.TryParse(value as String, out dateTime))
            {
                TimeSpan timeSpan = dateTime.TimeOfDay;
                if (Min != null)
                {
                    DateTime minDateTime;
                    if (DateTime.TryParse(Min, out minDateTime))
                    {
                        if (timeSpan < minDateTime.TimeOfDay)
                        {
                            String message = "Thời gian phải lớn hơn " + minDateTime.TimeOfDay.ToString();
                            result = new ValidationResult(false, message);
                            return result;
                        }

                    }
                }
                if (Max != null)
                {
                    DateTime maxDateTime;
                    if (DateTime.TryParse(Max, out maxDateTime))
                    {
                        if (timeSpan > maxDateTime.TimeOfDay)
                        {
                            String message = "Thời gian phải nhỏ hơn " + maxDateTime.TimeOfDay.ToString();
                            result = new ValidationResult(false, message);
                            return result;
                        }
                    }
                }
            }
            return result;
        }
    }
}
