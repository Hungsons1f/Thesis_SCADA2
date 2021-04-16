using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Thesis_SCADA.Converters
{
    class DecimalFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            float? val;
            try
            {
                val = (float)value;
            }
            catch (Exception)
            {
                return 0;
            }
            if (val == null) return 0;

            if (val >= 100) return decimal.Parse(val.ToString()).ToString("0");
            if ((val < 100) && (val >= 10)) return decimal.Parse(val.ToString()).ToString("0.#");
            if ((val < 10) && (val >= 1)) return decimal.Parse(val.ToString()).ToString("0.##");
            if ((val < 1) && (val >= 0.001)) return decimal.Parse(val.ToString()).ToString("0.###");
            if (val < 0.001) return 0;
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
