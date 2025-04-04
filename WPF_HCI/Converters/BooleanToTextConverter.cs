namespace WPF_HCI.Converters

{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class BooleanToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool booleanValue)
            {
                return booleanValue ? "True" : "False";
            }
            return "False";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                return stringValue.Equals("True", StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }
    }
}
