using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Thesis_SCADA.Model;

namespace Thesis_SCADA.Converters
{
    class EventGridConverter
    {
    }

    public class EvIconTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int? icontype = (int?)value;
            if (icontype == null) return null;

            switch (icontype)
            {
                case 1:
                    return PackIconKind.InfoOutline;
                case 2:
                    return PackIconKind.AlertOutline;
                case 3:
                    return PackIconKind.CloseOctagonOutline;
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class EvIconColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int? icontype = (int?)value;
            if (icontype == null) return null;

            switch (icontype)
            {
                case 1:
                    return GlobalColor.ClBlue;
                case 2:
                    return GlobalColor.ClYellow;
                case 3:
                    return GlobalColor.ClRed;
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class EvClassConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string evclass = (string)value;
            if (evclass == null) return null;

            switch (evclass)
            {
                case "Component Event":
                    return "Sự kiện thiết bị";
                case "Process Event":
                    return "Sự kiện quá trình";
                case "Analog Alarm":
                    return "Cảnh báo Analog";
                case "Digital Alarm":
                    return "Cảnh báo Digital";
                default:
                    return "Sự kiện hệ thống";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class EvContentConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string evclass = (string)values[0];
            int? evid = (int?)values[1];
            if (evclass == null) return null;
            if (evid == null) return null;

            switch (evclass)
            {
                case "Component Event":
                    switch (evid)
                    {
                        case 1:
                            return "Đã bật";
                        case 2:
                            return "Đã tắt";
                        case 3:
                            return "Chuyển sang Chế độ bằng tay";
                        case 4:
                            return "Chuyển sang Chế độ tự động";
                        case 5:
                            return "Chuyển sang Chế độ bảo trì";
                        case 6:
                            return "Lỗi";
                        case 7:
                            return "Đã reset";
                        default:
                            return null;
                    }
                case "Process Event":
                    switch (evid)
                    {
                        case 1:
                            return "Đã khởi tạo";
                        case 2:
                            return "Đã kích hoạt";
                        case 3:
                            return "Đang khởi động";
                        case 4:
                            return "Đã khởi động";
                        case 5:
                            return "Đã ở trạng thái xác lập";
                        case 6:
                            return "Đang dừng";
                        case 7:
                            return "Đã dừng";
                        case 8:
                            return "Đã vô hiệu";
                        case 9:
                            return "Đã dừng khẩn cấp";
                        case 10:
                            return "Lỗi: Đã dừng khẩn cấp quá trình";
                        case 11:
                            return "Đã reset";
                        default:
                            return null;
                    }
                case "Analog Alarm":
                    switch (evid)
                    {
                        case 1:
                            return "HIHI";
                        case 2:
                            return "HI";
                        case 3:
                            return "LO";
                        case 4:
                            return "LOLO";
                        default:
                            return null;
                    }
                case "Digital Alarm":
                    switch (evid)
                    {
                        case 1:
                            return "ON";
                        case 2:
                            return "OFF";
                        default:
                            return null;
                    }
                default:
                    return evclass + ", ID = " + evid;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class EvSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string source = (string)value;
            if (source == null) return null;

            switch (source)
            {
                case "Condense Pump":
                    return "Bơm ngưng";
                case "Supply Pump":
                    return "Bơm cấp";
                case "Circular Pump":
                    return "Bơm tuần hoàn";
                case "Low Pressure Heater Valve":
                    return "Van Gia nhiệt hạ áp";
                case "Deaerator Valve":
                    return "Van Bình khử khí";
                case "High Pressure Heater Valve":
                    return "Van Gia nhiệt cao áp";
                case "Turbine Valve":
                    return "Van Tua bin";
                case "Force Fan 1":
                    return "Quạt cấp 1";
                case "Force Fan 2":
                    return "Quạt cấp 2";
                case "Force Fan 3":
                    return "Quạt cấp 3";
                case "Process Controller":
                    return "Bộ điều khiển quá trình";
                case "LP Heater Temperature":
                    return "Nhiệt độ GN hạ áp";
                case "LP Heater Pressure":
                    return "Áp suất GN hạ áp";
                case "Deaerator Temperature":
                    return "Nhiệt độ Bình khử khí";
                case "HP Heater Temperature":
                    return "Nhiệt độ GN cao áp";
                case "HP Heater Pressure":
                    return "Áp suất GN cao áp";
                case "Boiler Temperature":
                    return "Nhiệt độ Lò hơi";
                case "Condenser Temperature":
                    return "Nhiệt độ Bình ngưng";
                case "Turbine Temperature":
                    return "Nhiệt độ Tua bin";
                case "Turbine Pressure":
                    return "Áp suất Tua bin";
                case "Turbine Speed":
                    return "Tốc độ quay Tua bin";
                default:
                    return source;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class EvTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime? time = (DateTime?)value;
            if (time == null) return null;

            if (time == new DateTime(1899, 12, 30, 00, 00, 00, 000)) return null;
            return ((DateTime)time).ToString("dd/MM/yyyy HH:mm:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

}
