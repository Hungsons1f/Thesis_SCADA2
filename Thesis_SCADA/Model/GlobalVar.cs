using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Thesis_SCADA.Model
{
    public static class GlobalVar
    {
        public static ConnectionStatus connectionStatus;
    }

    public enum ConnectionStatus
    {
        Offline,
        Connecting,
        Online
    }

    public enum ComponentStatus
    {
        Stop,
        Run,
        Fault
    }

    public class GlobalColor
    {
        private static Brush clYellow = new SolidColorBrush(Color.FromRgb(255, 197, 34));
        private static Brush clRed = new SolidColorBrush(Color.FromRgb(255, 34, 91));
        private static Brush clGreen = new SolidColorBrush(Color.FromRgb(91, 255, 34));
        private static Brush clBlue = new SolidColorBrush(Color.FromRgb(34, 202, 255));
        private static Brush clBlack = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        private static Brush clLightOrange = new SolidColorBrush(Color.FromRgb(255, 204, 188));

        public static Brush ClYellow { get => clYellow;  }
        public static Brush ClRed { get => clRed;  }
        public static Brush ClGreen { get => clGreen;  }
        public static Brush ClBlue { get => clBlue;  }
        public static Brush ClBlack { get => clBlack; }
        public static Brush ClLightOrange { get => clLightOrange; }
    }
}
