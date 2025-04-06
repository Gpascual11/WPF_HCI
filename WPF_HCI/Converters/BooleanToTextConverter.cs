// BooleanToTextConverter.cs
// ----------------------------------------------------------------------
// This file defines the BooleanToTextConverter class, which implements the
// IValueConverter interface. It converts a boolean value to its textual
// representation ("True" or "False") and converts a text value back to a boolean.
// This converter is useful for displaying boolean values as text in the UI.
// 
// Author: Gerard Pascual
// Date: 4/4/2025
// Version: 1.4
// ----------------------------------------------------------------------

namespace WPF_HCI.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Converts boolean values to their textual representation ("True" or "False")
    /// and converts text back to a boolean value.
    /// </summary>
    public class BooleanToTextConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean value to a string ("True" or "False").
        /// </summary>
        /// <param name="value">The boolean value to convert.</param>
        /// <param name="targetType">The type of the binding target property (unused).</param>
        /// <param name="parameter">An optional parameter for the converter (unused).</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A string representing the boolean value ("True" or "False").</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Check if the input value is a boolean
            if (value is bool booleanValue)
            {
                // Return "True" if the value is true; otherwise, return "False"
                return booleanValue ? "True" : "False";
            }
            // If the value is not a boolean, default to "False"
            return "False";
        }

        /// <summary>
        /// Converts a string ("True" or "False") back to a boolean value.
        /// </summary>
        /// <param name="value">The string value to convert back.</param>
        /// <param name="targetType">The type to convert to (expected to be boolean).</param>
        /// <param name="parameter">An optional parameter for the converter (unused).</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A boolean value corresponding to the input string.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Check if the input value is a string
            if (value is string stringValue)
            {
                // Return true if the string equals "True" (case-insensitive), otherwise false
                return stringValue.Equals("True", StringComparison.OrdinalIgnoreCase);
            }
            // If conversion is not possible, return false
            return false;
        }
    }
}
