using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace CESI_ToDoListWPF
{
    public class DateToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime date)
            {
                TimeSpan timeUntilExpiration = date - DateTime.Now;
                if (date < DateTime.Now)
                {
                    return Brushes.Red;
                }
                else if (timeUntilExpiration.TotalHours < 1)
                {
                    return Brushes.Orange;
                }
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
