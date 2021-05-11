using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ImportExportDesktopApp.Validations
{
    class TimeValidationRuleRange : Freezable
    {
        public static readonly DependencyProperty MinLengthProperty = DependencyProperty.Register(
        "MinLength", typeof(String), typeof(TimeValidationRuleRange), new FrameworkPropertyMetadata(null, OnMinLengthChanged));

        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register(
            "MaxLength", typeof(String), typeof(TimeValidationRuleRange), new FrameworkPropertyMetadata(null, OnMaxLengthChanged));

        public void SetValidator(TimeValidationRule validator)
        {
            Validator = validator;
            if (validator != null)
            {
                validator.Min = MinLength;
                validator.Max = MaxLength;
            }
        }

        public String MaxLength
        {
            get { return (String)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }

        public String MinLength
        {
            get { return (String)GetValue(MinLengthProperty); }
            set { SetValue(MinLengthProperty, value); }
        }

        private TimeValidationRule Validator { get; set; }

        private static void OnMaxLengthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var range = (TimeValidationRuleRange)d;
            if (range.Validator != null)
            {
                range.Validator.Max = (String)e.NewValue;
            }
        }

        private static void OnMinLengthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var range = (TimeValidationRuleRange)d;
            if (range.Validator != null)
            {
                range.Validator.Min = (String)e.NewValue;
            }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new TimeValidationRuleRange();
        }
    }
}
