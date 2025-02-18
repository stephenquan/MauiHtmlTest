using System.Globalization;

namespace MauiHtmlTest;

/// <summary>
/// Converts a string to a HTML formatted string.
/// </summary>
public class HtmlFormatConverter : IValueConverter
{
    /// <summary>
    /// Converts a HTML string to a formatted string.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string html)
        {
            var builder = new FormattedStringBuilder();
            builder.AddHtmlText(html);
            return builder.FormattedString;
        }

        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
