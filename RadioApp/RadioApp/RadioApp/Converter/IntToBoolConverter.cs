using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace RadioApp.Converter
{
    class IntToBoolConverter : IValueConverter
    {

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return "false";
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return (bool)value ? "true" : "false";
            }
        
    }
}
