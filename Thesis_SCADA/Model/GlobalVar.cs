using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Thesis_SCADA.ViewModel;

namespace Thesis_SCADA.Model
{
    public class GlobalVar
    {
        private static GlobalVar _ins;
        public static GlobalVar Ins
        {
            get
            {
                if (_ins == null) _ins = new GlobalVar();
                return _ins;
            }
            set
            {
                _ins = value;
            }
        }

        private MainInterface ipcData;
        public MainInterface IpcData { get => ipcData; set {ipcData = value; OnDataChanged(); } }

        private event EventHandler dataChanged;
        public event EventHandler DataChanged
        {
            add { dataChanged += value; }
            remove { dataChanged -= value; }
        }

        void OnDataChanged()
        {
            if (dataChanged != null)
            {
                dataChanged(this, new EventArgs());
            }
        }

        //Đặt pt khởi tạo là private để không thể tạo đối tượng bằng lớp này từ bên ngoài
        private GlobalVar()
        {
            ipcData = new MainInterface();
        }

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
