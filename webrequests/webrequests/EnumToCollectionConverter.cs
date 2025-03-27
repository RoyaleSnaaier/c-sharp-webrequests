using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Webrequest
{
    public class EnumToCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumType = value as Type;
            if (enumType == null || !enumType.IsEnum)
                return null;

            return Enum.GetValues(enumType).Cast<Enum>().Select(e => new
            {
                Name = e.ToString(),
                Value = e
            });
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
