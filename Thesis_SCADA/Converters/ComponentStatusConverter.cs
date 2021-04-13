using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Thesis_SCADA.Converters
{
    class ComponentStatusConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool? run;
            bool? fault;
            try
            {
                run = (bool)values[0];
                fault = (bool)values[1];
            }
            catch (Exception)
            {
                return 0;
            }
            if (run == null || fault == null ) return 0;

            byte i = 0;
            if (run == true) i = 1;
            if (fault == true) i = 2;
            return i;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
