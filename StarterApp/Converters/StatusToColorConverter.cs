using System.Globalization;

namespace StarterApp.Converters;

public class StatusToColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value as string switch
        {
            "Approved" => Colors.Green,
            "Rejected" => Colors.Red,
            _ => Colors.Orange
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
