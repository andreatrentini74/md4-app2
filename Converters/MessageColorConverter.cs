using System.Globalization;

namespace md4.Converters
{
    public class MessageColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string message)
            {
                if (string.IsNullOrEmpty(message))
                    return Colors.Black;

                if (message.Contains("successo"))
                    return Colors.Green;

                if (message.Contains("Errore"))
                    return Colors.Red;
            }

            return Colors.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}