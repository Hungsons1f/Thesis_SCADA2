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
                adsClient = new TcAdsClient();
                adsClient.Connect(851);
                if (!adsClient.IsConnected)
                {
                    MessageBox.Show("Chưa kết nối được IPC", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                MessageBox.Show("Chưa kết nối được IPC", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
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
                        MessageBox.Show("Cannot Read IPC data cyclically", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        _timer.Stop();
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
        public short Mode;
	    //Manual
	    public bool Start;
        public bool Stop;
        public float SetSpeed;
        public bool Reset;
        //Auto
        public bool RunCondition;
        public bool StopCondition;
        public float SpeedCondition;
        //Others
        public bool RunFeedback;
        public bool Interlock;
        public float Maxspeed;
        public float ActualSpeed;
        //Output
        public bool Cmd;
        public bool Fault;
        public short State;
        public float Speed;
        public uint Runtime;
        public uint AccRuntime;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public class aFb_Valve
    {
        public short Mode;
        //Manual
        public bool Start;
        public bool Stop;
        public float SetPercent;
        public bool Reset;
        //Auto
        public bool RunCondition;
        public bool StopCondition;
        public float PercentCondition;
        //Others
        public bool RunFeedback;
        public bool Interlock;
        //Output
        public bool Cmd;
        public bool Fault;
        public short State;
        public float OpenPercent;
        public uint Runtime;
        public uint AccRuntime;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public class aT_Furnace
    {
        public float Temperature;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public class aT_Heater
    {
        public float Temperature;
        public float Pressure;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public class aT_Turbine
    {
        public float Rotation;
        public float HighPressure;
        public float ImmediatePressure;
        public float OutPressure;
        public float HighTemperature;
        public float ImmediateTemperature;
        public float OutTemperature;
    }

    public enum aE_Mode { Manual = 0, Auto = 1, Service = 2}
    public enum aE_State { Fault = 0, Auto = 1, ManRun = 2, ManStop = 3}
    #endregion
}
