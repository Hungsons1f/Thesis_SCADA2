using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
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

        #region Settings
        private string netID = "";
        public string NetID { get => netID; set { netID = value;  } }

        private int port = 851;
        public int Port { get => port; set { port = value;  } }

        private int ipcCycle;
        public int IpcCycle { get => (ipcCycle = ipcDataService.ScanCycle); set { ipcCycle = value; ipcDataService.ScanCycle = ipcCycle; } }

        private int dbCycle = 1000;
        public int DbCycle { get => dbCycle; set
            {
                dbCycle = value;
                try
                {
                    timer.Stop();
                    timer.Interval = dbCycle;
                }
                finally
                {
                    timer.Start();
                }
            }
        }

        private Users user;
        public Users User { get => (user); set { user = value; } }

        #endregion

        #region Data
        private MainInterface ipcData;
        public MainInterface IpcData { get => ipcData; set {ipcData = value; OnDataChanged(); } }

        private AutoCtrlCmds processState;
        public AutoCtrlCmds ProcessState { get => processState; set { processState = value; OnDataChanged(); } }

        private Parameters parameter;
        public Parameters Parameter { get => parameter; set { parameter = value; OnDataChanged(); } }

        private aS_Event ipcEvent;
        public aS_Event IpcEvent { get => ipcEvent; set { ipcEvent = value; } }

        private byte ipcEventType;
        public byte IpcEventType { get => ipcEventType; set { ipcEventType = value; } }

        private ProcessData dbprocessdata;
        public ProcessData DbProcessdata { get => dbprocessdata; set => dbprocessdata = value; }

        private ConnectionStatus connectstatus;
        public ConnectionStatus ConnectStatus { get => connectstatus; set => connectstatus = value; }

        private TimeSpan scanTime;
        public TimeSpan ScanTime { get => scanTime; set => scanTime = value; }
        #endregion

        private event EventHandler dataChanged;
        public event EventHandler DataChanged { add { dataChanged += value; }  remove { dataChanged -= value; } }

        private event EventHandler databaseUpdated;
        public event EventHandler DatabaseUpdated { add { databaseUpdated += value; } remove { databaseUpdated -= value; } }

        private event EventHandler eventOccured;
        public event EventHandler EventOccured { add { eventOccured += value; } remove { eventOccured -= value; } }

        private volatile object _locker = new object();
        private volatile object _lockerdb = new object();
        private readonly System.Timers.Timer timer;
        #endregion

        //Đặt pt khởi tạo là private để không thể tạo đối tượng bằng lớp này từ bên ngoài
        private GlobalVar()
        {
            ipcDataService = new IPCDataService();
            IpcData = new MainInterface();
            ProcessState = new AutoCtrlCmds();
            Parameter = new Parameters();
            DbProcessdata = new ProcessData();
            IpcEvent = new aS_Event();
            OnIpcDataRefreshed(null, null);
            ipcDataService.ValuesRefreshed += OnIpcDataRefreshed;
            ipcDataService.EventOccured += OnIpcEventOccured;

            timer = new System.Timers.Timer();
            timer.Interval = DbCycle;
            timer.Elapsed -= OnTimerElapsed;
            timer.Elapsed += OnTimerElapsed;

            Connect(NetID, Port, false);
        }

        public bool Connect(string netid, int port, bool showmsg)
        {
            if (ipcDataService.Connect(netid, port, showmsg))//"5.57.208.6.1.1"
            {
                timer.Start();

                try
                {
                    FirstUpdate();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Không có SQL server của cơ sở dữ liệu: " + e.ToString(), "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return true;
            }
            else return false;
        }

        private async void FirstUpdate()
        {
            await Task.Run(() =>
            {
                Thread.Sleep(1000);
                var le = ipcDataService.GetAllEvents();
                if (le == null) return;
                if (!le.Any()) return;

                var max = DataProvider.Ins.DB.ProcessEvent.Max(x => x.TimeRaised);
                var cur = DataProvider.Ins.DB.ProcessEvent.FirstOrDefault(p => p.TimeRaised == max);
                if (cur == null) return;

                for (int i = 0; i < le.Count; i++)
                {
                    if (le[i].TimeRaised > cur.TimeRaised)
                    {
                        var e = new ProcessEvent();
                        e.EventClass = le[i].EventClass;
                        e.EventID = (int)le[i].EventID;
                        e.SeverityLevel = (int)le[i].SeverityLevel;
                        e.SourceName = le[i].SourceName;
                        e.TimeCleared = le[i].TimeCleared;
                        e.TimeConfirmed = le[i].TimeConfirmed;
                        e.TimeRaised = le[i].TimeRaised;
                        DataProvider.Ins.DB.ProcessEvent.Add(e);
                    }
                    else break;
                }

                DataProvider.Ins.DB.SaveChanges();
            });
        }

        public void Disconnect()
        {
            ipcDataService.Disconnect();
            timer.Stop();
        }

        public async void WriteData<T> (string varname, T value)
        {
            await ipcDataService.Write<T>(varname, value);
        }

        public void AckAllAlarms()
        {
            ipcDataService.AckAllAlarms();
        }

        private async void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            await UpdateDB();
            OnDatabaseUpdated();
        }

        private Task UpdateDB()
        {
            return Task.Run(() =>
            {
                lock (_locker)
                {
                    try
                    {
                        var processdata = new ProcessData();
                        processdata.Timestamp = DateTime.Now;
                        processdata.Furnace_Temp = ipcData.Components.Furnace.Temperature;
                        processdata.PCondense_Flow = ipcData.Components.LPHeater.InFlow;
                        processdata.PCondense_Press = ipcData.Components.LPHeater.InPressure;
                        processdata.PCircular_Flow = ipcData.Components.Condenser.InFlow;
                        processdata.PSupply_Flow = ipcData.Components.HPHeater.InFlow;
                        processdata.PSupply_Press = ipcData.Components.HPHeater.InPressure;
                        processdata.HBoiler_Press = ipcData.Components.Boiler.Pressure;
                        processdata.HBoiler_Temp = ipcData.Components.Boiler.Temperature;
                        processdata.HCondenser_Temp = ipcData.Components.Condenser.Temperature;
                        processdata.HDeaerator_Press = ipcData.Components.Deaerator.Pressure;
                        processdata.HDeaerator_Temp = ipcData.Components.Deaerator.Temperature;
                        processdata.HHPHeater_Press = ipcData.Components.HPHeater.Pressure;
                        processdata.HHPHeater_Temp = ipcData.Components.HPHeater.Temperature;
                        processdata.HLPHeater_Press = ipcData.Components.LPHeater.Pressure;
                        processdata.HLPHeater_Temp = ipcData.Components.LPHeater.Temperature;
                        processdata.TurbineH_Press = ipcData.Components.Turbine.HighPressure;
                        processdata.TurbineH_Temp = ipcData.Components.Turbine.HighTemperature;
                        processdata.TurbineI_Press = ipcData.Components.Turbine.ImmediatePressure;
                        processdata.TurbineI_Temp = ipcData.Components.Turbine.ImmediateTemperature;
                        processdata.TurbineL_Press = ipcData.Components.Turbine.OutPressure;
                        processdata.TurbineL_Temp = ipcData.Components.Turbine.OutTemperature;
                        processdata.Turbine_Freq = ipcData.Components.Turbine.Rotation;

                        DataProvider.Ins.DB.ProcessData.Add(processdata);
                        DataProvider.Ins.DB.SaveChanges();

                        DbProcessdata = processdata;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Không có SQL server của cơ sở dữ liệu: " + e.ToString(), "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            });
        }

        private void OnIpcDataRefreshed(object sender, EventArgs e)
        {
            IpcData = ipcDataService.ReadData;
            ProcessState = ipcDataService.ProcessState;
            Parameter = ipcDataService.Parameter;
            ConnectStatus = (ConnectionStatus)ipcDataService.ConnectStatus;
            ScanTime = ipcDataService.ScanTime;
        }

        private async void OnIpcEventOccured(object sender, IPCDataService.EventPropEventArgs e)
        {
            IpcEvent = e.Event;
            IpcEventType = e.EventType;
            await UpdateDBEvent(e.EventType, e.Event);
            OnEventOccured();
        }

        private Task UpdateDBEvent(byte eventType, aS_Event _event)
        {
            return Task.Run(() =>
            {
                lock (_locker)
                {
                    try
                    {
                        var processevent = new ProcessEvent();
                        processevent.EventClass = _event.EventClass;
                        processevent.EventID = (int)_event.EventID;
                        processevent.SeverityLevel = (int)_event.SeverityLevel;
                        processevent.SourceName = _event.SourceName;
                        processevent.TimeRaised = _event.TimeRaised;

                        switch (eventType)
                        {
                            case 1:
                            case 2:
                                processevent.TimeConfirmed = null;
                                processevent.TimeCleared = null;
                                try
                                {
                                    DataProvider.Ins.DB.ProcessEvent.Add(processevent);
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show("Không thể thêm dữ liệu: " + e.ToString(), "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                                break;
                            case 3:
                                try
                                {
                                    var d = DataProvider.Ins.DB.ProcessEvent.Where(x => (x.SourceName == processevent.SourceName && x.EventClass == processevent.EventClass
                                                && x.EventID == processevent.EventID && x.TimeRaised == processevent.TimeRaised)).SingleOrDefault();
                                    d.TimeConfirmed = _event.TimeConfirmed;
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show("Không thể thêm thời gian xác nhận: " + e.ToString(), "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                                break;
                            case 4:
                                try
                                {
                                    var dd = DataProvider.Ins.DB.ProcessEvent.Where(x => (x.SourceName == processevent.SourceName && x.EventClass == processevent.EventClass
                                                && x.EventID == processevent.EventID && x.TimeRaised == processevent.TimeRaised)).SingleOrDefault();
                                    dd.TimeCleared = _event.TimeCleared;
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show("Không thể thêm thời gian xóa: " + e.ToString(), "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                                break;
                        }

                        DataProvider.Ins.DB.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Không có SQL server của cơ sở dữ liệu: " + e.ToString(), "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            });
        }

        void OnDataChanged()
        {
            if (dataChanged != null)
            {
                dataChanged(this, new EventArgs());
            }
        }

        void OnDatabaseUpdated()
        {
            if (databaseUpdated != null)
            {
                databaseUpdated(this, new EventArgs());
            }
        }

        void OnEventOccured()
        {
            if (eventOccured != null)
            {
                eventOccured(this, new EventArgs());
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
