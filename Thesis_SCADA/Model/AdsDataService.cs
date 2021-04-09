using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinCAT.Ads;
using System.Diagnostics;
using System.Timers;
using System.Runtime.InteropServices;
using System.Windows;

namespace Thesis_SCADA.Model
{
    public class AdsDataService
    {
        #region Properties
        private TcAdsClient adsClient;
        private readonly System.Timers.Timer _timer;
        private DateTime _lastScanTime;
        public TimeSpan ScanTime { get; private set; }
        public TimeSpan Testtime { get; private set; }
        private volatile object _locker = new object();
        public event EventHandler ValuesRefreshed;

        public MainInterface PlcData;
        #endregion

        public AdsDataService()
        {
            try
            {
                StateInfo state;
                adsClient = new TcAdsClient();
                adsClient.Connect(851);
                if (adsClient.TryReadState(out state) != AdsErrorCode.NoError)
                {
                    MessageBox.Show("Chưa kết nối được IPC.\n Lỗi: AdsState: " + state.AdsState + ", DeviceState: " + state.DeviceState, 
                                    "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                MessageBox.Show("Chưa kết nối được IPC", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            PlcData = new MainInterface();
            OnValuesRefreshed();

            _timer = new System.Timers.Timer();
            _timer.Interval = 100;
            _timer.Elapsed += OnTimerElapsed;
            _timer.Start();
        }

        public void DisposeService()
        {
            _timer.Stop();
        }

        public async Task Write(string varname, object value)
        {
            await Task.Run(() =>
            {
                switch (varname)
                {
                    case "val1cmd":
                        adsClient.WriteSymbol("MAIN.val1cmd", (bool)value, false);
                        break;
                    case "val2cmd":
                        adsClient.WriteSymbol("MAIN.val2cmd", (bool)value, false);
                        break;
                    case "start":
                        adsClient.WriteSymbol("MAIN.start", (bool)value, false);
                        break;
                    case "stop":
                        adsClient.WriteSymbol("MAIN.stop", (bool)value, false);
                        break;
                    default:
                        break;
                }
            });
        }

        private async void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                _timer.Stop();
                ScanTime = DateTime.Now - _lastScanTime;
                await RefreshValues();
                OnValuesRefreshed();
            }
            finally
            {
                _timer.Start();
            }
            _lastScanTime = DateTime.Now;
        }

        DateTime scaned;
        private Task RefreshValues()
        {
            return Task.Run(() =>
            {
                lock (_locker)
                {
                    try
                    {
                        scaned = DateTime.Now;
                        PlcData = adsClient.ReadSymbol("Interfacex.ScadaInterface", typeof(MainInterface), false) as MainInterface;

                        Testtime = DateTime.Now - scaned;
                    }
                    catch (Exception e)
                    {
                        _timer.Stop();
                        MessageBox.Show("Không đọc được dữ liệu tuần hoàn từ IPC" + e.ToString(), "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        
                    }
                }
            });
        }

        private void OnValuesRefreshed()
        {
            ValuesRefreshed?.Invoke(this, new EventArgs());
        }

    }


    #region PLC Data Structures
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public class MainInterface
    {
        public Components Components;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public class Components
    {
        public aFb_Motor ForceFan1;
        public aFb_Motor ForceFan2;
        public aFb_Motor ForceFan3;

        public aFb_Motor CondensePump;
        public aFb_Motor SupplyPump;
        public aFb_Motor CircularPump;

        public aFb_Valve LPHValve;
        public aFb_Valve AirEjValve;
        public aFb_Valve HPHValve;
        public aFb_Valve TurbineValve;

        public aT_Furnace Furnace;
        public aT_Heater LPHeater;
        public aT_Heater Deaerator;
        public aT_Heater HPHeater;
        public aT_Heater Boiler;
        public aT_Turbine Turbine;
        public aT_Heater Condenser;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public class aFb_Motor
    {
        public short mode;
	    //Manual
	    public bool start;
        public bool stop;
        public float setSpeed;
        public bool reset;
        //Auto
        public bool runCondition;
        public bool stopCondition;
        public float speedCondition;
        //Others
        public bool runFeedback;
        public bool interlock;
        public float maxspeed;
        public float actualSpeed;
        //Output
        public bool cmd;
        public bool fault;
        public short state;
        public float speed;
        public uint runtime;
        public uint accRuntime;

        public short Mode { get => mode; set => mode = value; }
        public bool Start { get => start; set => start = value; }
        public bool Stop { get => stop; set => stop = value; }
        public float SetSpeed { get => setSpeed; set => setSpeed = value; }
        public bool Reset { get => reset; set => reset = value; }
        public bool RunCondition { get => runCondition; set => runCondition = value; }
        public bool StopCondition { get => stopCondition; set => stopCondition = value; }
        public float SpeedCondition { get => speedCondition; set => speedCondition = value; }
        public bool RunFeedback { get => runFeedback; set => runFeedback = value; }
        public bool Interlock { get => interlock; set => interlock = value; }
        public float Maxspeed { get => maxspeed; set => maxspeed = value; }
        public float ActualSpeed { get => actualSpeed; set => actualSpeed = value; }
        public bool Cmd { get => cmd; set => cmd = value; }
        public bool Fault { get => fault; set => fault = value; }
        public short State { get => state; set => state = value; }
        public float Speed { get => speed; set => speed = value; }
        public uint Runtime { get => runtime; set => runtime = value; }
        public uint AccRuntime { get => accRuntime; set => accRuntime = value; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public class aFb_Valve
    {
        public short mode;
        //Manual
        public bool start;
        public bool stop;
        public float setPercent;
        public bool reset;
        //Auto
        public bool runCondition;
        public bool stopCondition;
        public float percentCondition;
        //Others
        public bool runFeedback;
        public bool interlock;
        //Output
        public bool cmd;
        public bool fault;
        public short state;
        public float openPercent;
        public uint runtime;
        public uint accRuntime;

        public short Mode { get => mode; set => mode = value; }
        public bool Start { get => start; set => start = value; }
        public bool Stop { get => stop; set => stop = value; }
        public float SetPercent { get => setPercent; set => setPercent = value; }
        public bool Reset { get => reset; set => reset = value; }
        public bool RunCondition { get => runCondition; set => runCondition = value; }
        public bool StopCondition { get => stopCondition; set => stopCondition = value; }
        public float PercentCondition { get => percentCondition; set => percentCondition = value; }
        public bool RunFeedback { get => runFeedback; set => runFeedback = value; }
        public bool Interlock { get => interlock; set => interlock = value; }
        public bool Cmd { get => cmd; set => cmd = value; }
        public bool Fault { get => fault; set => fault = value; }
        public short State { get => state; set => state = value; }
        public float OpenPercent { get => openPercent; set => openPercent = value; }
        public uint Runtime { get => runtime; set => runtime = value; }
        public uint AccRuntime { get => accRuntime; set => accRuntime = value; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public class aT_Furnace
    {
        public float temperature;

        public float Temperature { get => temperature; set => temperature = value; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public class aT_Heater
    {
        public float temperature;
        public float pressure;

        public float Temperature { get => temperature; set => temperature = value; }
        public float Pressure { get => pressure; set => pressure = value; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public class aT_Turbine
    {
        public float rotation;
        public float highPressure;
        public float immediatePressure;
        public float outPressure;
        public float highTemperature;
        public float immediateTemperature;
        public float outTemperature;

        public float Rotation { get => rotation; set => rotation = value; }
        public float HighPressure { get => highPressure; set => highPressure = value; }
        public float ImmediatePressure { get => immediatePressure; set => immediatePressure = value; }
        public float OutPressure { get => outPressure; set => outPressure = value; }
        public float HighTemperature { get => highTemperature; set => highTemperature = value; }
        public float ImmediateTemperature { get => immediateTemperature; set => immediateTemperature = value; }
        public float OutTemperature { get => outTemperature; set => outTemperature = value; }
    }

    public enum aE_Mode { Manual = 0, Auto = 1, Service = 2}
    public enum aE_State { Fault = 0, Auto = 1, ManRun = 2, ManStop = 3}
    #endregion
}
