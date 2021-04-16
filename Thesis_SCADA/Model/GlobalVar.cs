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
        #region Properties
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

        private IPCDataService ipcDataService;

        private MainInterface ipcData;
        public MainInterface IpcData { get => ipcData; set {ipcData = value; OnDataChanged(); } }

        private AutoCtrlCmds processState;
        public AutoCtrlCmds ProcessState { get => processState; set { processState = value; OnDataChanged(); } }

        private Parameters parameter;
        public Parameters Parameter { get => parameter; set { parameter = value; OnDataChanged(); } }

        private ConnectionStatus connectstatus;
        public ConnectionStatus ConnectStatus { get => connectstatus; set => connectstatus = value; }

        private TimeSpan scanTime;
        public TimeSpan ScanTime { get => scanTime; set => scanTime = value; }

        private event EventHandler dataChanged;
        public event EventHandler DataChanged { add { dataChanged += value; }  remove { dataChanged -= value; } }
        #endregion

        //Đặt pt khởi tạo là private để không thể tạo đối tượng bằng lớp này từ bên ngoài
        private GlobalVar()
        {
            ipcDataService = new IPCDataService();
            IpcData = new MainInterface();
            ProcessState = new AutoCtrlCmds();
            Parameter = new Parameters();
            OnIpcDataRefreshed(null, null);
            ipcDataService.ValuesRefreshed += OnIpcDataRefreshed;

            ipcDataService.Connect("", 851);//"5.57.208.6.1.1"
        }

        public async void WriteData<T> (string varname, T value)
        {
            await ipcDataService.Write<T>(varname, value);
        }

        private void OnIpcDataRefreshed(object sender, EventArgs e)
        {
            IpcData = ipcDataService.ReadData;
            ProcessState = ipcDataService.ProcessState;
            Parameter = ipcDataService.Parameter;
            ConnectStatus = (ConnectionStatus)ipcDataService.ConnectStatus;
            ScanTime = ipcDataService.ScanTime;
        }

        void OnDataChanged()
        {
            if (dataChanged != null)
            {
                dataChanged(this, new EventArgs());
            }
        }
    }



    public enum ConnectionStatus
    {
        Offline,
        Connecting,
        Online
    }




    public enum ComponentMode
    {
        Manual,
        Automatic,
        Service
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
