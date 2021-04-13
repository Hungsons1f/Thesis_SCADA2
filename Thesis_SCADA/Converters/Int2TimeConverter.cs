using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Thesis_SCADA.Converters
{
    class Int2TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            uint? time;
            try
            {
                time = (uint)value;
            }
            catch (Exception)
            {
                return null;
            }
            if (time == null) return null;

            uint hour, min, sec;
            hour = (uint)time / 3600;
            min = ((uint)time - hour * 3600)/60;
            sec = ((uint)time - hour * 3600 - min * 60);
            return (hour.ToString() + ":" + ((min<10)? ("0" + min.ToString()) : min.ToString()) + ":" + ((sec < 10) ? ("0" + sec.ToString()) : sec.ToString()));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
