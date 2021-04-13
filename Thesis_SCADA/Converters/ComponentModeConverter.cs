using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Thesis_SCADA.Converters
{
    class ComponentModeConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            short? mode;
            try
            {
                mode = (short)value;
            }
            catch (Exception)
            {
                return null;
            }
            if (mode == null) return null;

            switch (mode)
            {
                case 0:
                    return "Manual";
                case 1:
                    return "Automatic";
                case 2:
                    return "Service";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
