using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Thesis_SCADA.Converters
{
    class Value2PercentConverter : IValueConverter
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
            if (val == null) return 0.0;
            return (double)(val * 100.0) ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
