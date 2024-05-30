using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Scaffold.XamlDesigner.Converters
{
    public sealed class NullToVisibilityConverter : IValueConverter
    {
        public Visibility TrueValue { get; set; }
        public Visibility FalseValue { get; set; }

        public NullToVisibilityConverter()
        {
            TrueValue = Visibility.Visible;
            FalseValue = Visibility.Collapsed;
        }

        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value is IEnumerable and not string)
            {
                var enumeration = (IEnumerable<object>)value;
                return enumeration.Any() == false ? TrueValue : FalseValue;
            }
            
            return value == null ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (Equals(value, TrueValue))
                return true;

            if (Equals(value, FalseValue))
                return false;

            return null;
        }
    }
}
