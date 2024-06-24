using System.Globalization;
using System.Windows.Data;

namespace Leagueinator.Controls {

    /// <summary>
    /// Convert data in an xaml linked attribute.
    /// Adds one to the model value before displaying.
    /// </summary>
    public class LaneConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is int zeroIndexed) {
                return zeroIndexed + 1;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is int zeroIndexed) {
                return zeroIndexed + 1;
            }

            return value;
        }
    }
}
