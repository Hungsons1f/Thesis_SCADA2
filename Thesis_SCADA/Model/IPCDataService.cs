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
using TcEventLoggerAdsProxyLib;

namespace Thesis_SCADA.Model
{
    public class IPCDataService                 //use Beckhoff ADS protocol
    {
        #region Types
        public enum ConnectionStatus
        {
            Offline,
            Connecting,
            Online
        }

        public struct NotificationHandler
        {
            public int ProcessReady;
            public int ProcessRun;
            public int ProcessFault;

            public int CondensePumpPress;
            public int SupplyPumpPress;
            //public int CondenserTemp;
            public int LPHeaterTemp;
            public int DeaeratorTemp;
            public int HPHeaterTemp;
            public int TurbineSpeed;
            public int BoilerTemp;

            public int MaxSpeed_CondensePump;
            public int MaxSpeed_SupplyPump;
            public int MaxSpeed_CircularPump;
            public int MaxSpeed_InterPump;
            public int MaxSpeed_ForceFan1;
            public int MaxSpeed_ForceFan2;
            public int MaxSpeed_ForceFan3;

            public int Condenser_Htemp;
            public int Condenser_Ltemp;

            public int SampleTime;
        }

        public class EventPropEventArgs : EventArgs
        {
            public byte EventType { get; set; }
            public aS_Event Event { get; set; }
            public EventPropEventArgs(byte eventType, aS_Event _event)
            {
                EventType = eventType;
                Event = _event;
            }

        }
        #endregion

        #region Properties
        private TcAdsClient client;
        public MainInterface ReadData;
        public AutoCtrlCmds ProcessState;
        public Parameters Parameter;
        private NotificationHandler notificationHandler;

        private TcEventLogger logger;
        private int langID = 1033;

        private readonly System.Timers.Timer _timer;
        private DateTime _lastScanTime;
        public TimeSpan ScanTime { get; private set; }
        public TimeSpan Testtime { get; private set; }

        private volatile object _locker = new object();
        public event EventHandler ValuesRefreshed;
        public event EventHandler<EventPropEventArgs> EventOccured;

        public ConnectionStatus ConnectStatus { get; private set; }

        private int scanCycle = 200;
        public int ScanCycle { get => scanCycle; set
            {
                scanCycle = value;
                try
                {
                    _timer.Stop();
                    _timer.Interval = scanCycle;
                }
                finally
                {
                    _timer.Start();
                }
            }
        }
        #endregion


        public IPCDataService()
        {
            client = new TcAdsClient();
            ReadData = new MainInterface();
            ProcessState = new AutoCtrlCmds();
            Parameter = new Parameters();

            logger = new TcEventLogger();
            logger.MessageSent += OnMessageSent;
            logger.AlarmRaised += OnAlarmRaised;
            logger.AlarmCleared += OnAlarmCleared;
            logger.AlarmConfirmed += OnAlarmConfirmed;

            _timer = new System.Timers.Timer();
            _timer.Interval = ScanCycle;
            _timer.Elapsed -= OnTimerElapsed;
            _timer.Elapsed += OnTimerElapsed;
        }

        public void DisposeService()
        {
            _timer.Stop();
        }

        public bool Connect(string netid, int port, bool showmsg)
        {
            bool result = false;
            try
            {
                StateInfo state;
                ConnectStatus = ConnectionStatus.Connecting;

                client.Connect(netid, port);
                if (client.TryReadState(out state) != AdsErrorCode.NoError)
                {
                    if (!showConnectMsg)
                    {
                        MessageBox.Show("Chưa kết nối được IPC.\n Lỗi: AdsState: " + state.AdsState + ", DeviceState: " + state.DeviceState,
                                        "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        showConnectMsg = true & !showmsg;
                    }
                    ConnectStatus = ConnectionStatus.Offline;
                }
                else
                {
                    logger.Connect(netid);
                    col = logger.GetLoggedEvents(1000);
                    _timer.Start();

                    ProcessState = client.ReadSymbol("Interfacex.AutoCtrlCmds", typeof(AutoCtrlCmds), false) as AutoCtrlCmds;
                    Parameter = client.ReadSymbol("Interfacex.Parameters", typeof(Parameters), false) as Parameters;

                    notificationHandler.ProcessReady = client.AddDeviceNotificationEx("Interfacex.AutoCtrlCmds.ProcessReady", AdsTransMode.OnChange, 200, 0, null, typeof(bool));
                    notificationHandler.ProcessRun = client.AddDeviceNotificationEx("Interfacex.AutoCtrlCmds.ProcessRun", AdsTransMode.OnChange, 200, 0, null, typeof(bool));
                    notificationHandler.ProcessFault = client.AddDeviceNotificationEx("Interfacex.AutoCtrlCmds.ProcessFault", AdsTransMode.OnChange, 200, 0, null, typeof(bool));

                    notificationHandler.CondensePumpPress = client.AddDeviceNotificationEx("Interfacex.Parameters.PID_CondensePump_Press", AdsTransMode.OnChange, 200, 0, null, typeof(aS_PidParameter));
                    notificationHandler.SupplyPumpPress = client.AddDeviceNotificationEx("Interfacex.Parameters.PID_SupplyPump_Press", AdsTransMode.OnChange, 200, 0, null, typeof(aS_PidParameter));
                    //notificationHandler.CondenserTemp = client.AddDeviceNotificationEx("Interfacex.Parameters.PID_Condenser_Temp", AdsTransMode.OnChange, 200, 0, null, typeof(aS_PidParameter));
                    notificationHandler.LPHeaterTemp = client.AddDeviceNotificationEx("Interfacex.Parameters.PID_LPHeater_Temp", AdsTransMode.OnChange, 200, 0, null, typeof(aS_PidParameter));
                    notificationHandler.DeaeratorTemp = client.AddDeviceNotificationEx("Interfacex.Parameters.PID_AirEjector_Temp", AdsTransMode.OnChange, 200, 0, null, typeof(aS_PidParameter));
                    notificationHandler.HPHeaterTemp = client.AddDeviceNotificationEx("Interfacex.Parameters.PID_HPHeater_Temp", AdsTransMode.OnChange, 200, 0, null, typeof(aS_PidParameter));
                    notificationHandler.TurbineSpeed = client.AddDeviceNotificationEx("Interfacex.Parameters.PID_Turbine_Speed", AdsTransMode.OnChange, 200, 0, null, typeof(aS_PidParameter));
                    notificationHandler.BoilerTemp = client.AddDeviceNotificationEx("Interfacex.Parameters.PID_Boiler_Temp", AdsTransMode.OnChange, 200, 0, null, typeof(aS_PidParameter));

                    notificationHandler.MaxSpeed_CondensePump = client.AddDeviceNotificationEx("Interfacex.Parameters.MaxSpeed_CondensePump", AdsTransMode.OnChange, 200, 0, null, typeof(float));
                    notificationHandler.MaxSpeed_SupplyPump = client.AddDeviceNotificationEx("Interfacex.Parameters.MaxSpeed_SupplyPump", AdsTransMode.OnChange, 200, 0, null, typeof(float));
                    notificationHandler.MaxSpeed_CircularPump = client.AddDeviceNotificationEx("Interfacex.Parameters.MaxSpeed_CircularPump", AdsTransMode.OnChange, 200, 0, null, typeof(float));
                    notificationHandler.MaxSpeed_InterPump = client.AddDeviceNotificationEx("Interfacex.Parameters.MaxSpeed_InterPump", AdsTransMode.OnChange, 200, 0, null, typeof(float));
                    notificationHandler.MaxSpeed_ForceFan1 = client.AddDeviceNotificationEx("Interfacex.Parameters.MaxSpeed_ForceFan1", AdsTransMode.OnChange, 200, 0, null, typeof(float));
                    notificationHandler.MaxSpeed_ForceFan2 = client.AddDeviceNotificationEx("Interfacex.Parameters.MaxSpeed_ForceFan2", AdsTransMode.OnChange, 200, 0, null, typeof(float));
                    notificationHandler.MaxSpeed_ForceFan3 = client.AddDeviceNotificationEx("Interfacex.Parameters.MaxSpeed_ForceFan3", AdsTransMode.OnChange, 200, 0, null, typeof(float));

                    notificationHandler.Condenser_Htemp = client.AddDeviceNotificationEx("Interfacex.Parameters.Condenser_Htemp", AdsTransMode.OnChange, 200, 0, null, typeof(float));
                    notificationHandler.Condenser_Ltemp = client.AddDeviceNotificationEx("Interfacex.Parameters.Condenser_Ltemp", AdsTransMode.OnChange, 200, 0, null, typeof(float));

                    notificationHandler.SampleTime = client.AddDeviceNotificationEx("Interfacex.Parameters.SampleTime", AdsTransMode.OnChange, 200, 0, null, typeof(float));

                    client.AdsNotificationEx -= OnAdsNotificationEx;
                    client.AdsNotificationEx += OnAdsNotificationEx;

                    ConnectStatus = ConnectionStatus.Online;
                    showConnectMsg = false;
                    result = true;
                }

                OnValuesRefreshed();
            }
            catch (Exception e)
            {
                if (!showConnectMsg)
                {
                    MessageBox.Show("Chưa kết nối được IPC.\n Lỗi: " + e.ToString(), "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    ConnectStatus = ConnectionStatus.Offline;
                    showConnectMsg = true & !showmsg;
                }
            }
            return result;
        }
        private bool showConnectMsg = false;
        public TcLoggedEventCollection col;

        public void Disconnect()
        {
            _timer.Stop();

            client.DeleteDeviceNotification(notificationHandler.ProcessReady);
            client.DeleteDeviceNotification(notificationHandler.ProcessRun);
            client.DeleteDeviceNotification(notificationHandler.ProcessFault);

            client.DeleteDeviceNotification(notificationHandler.CondensePumpPress);
            client.DeleteDeviceNotification(notificationHandler.SupplyPumpPress);
            //client.DeleteDeviceNotification(notificationHandler.CondenserTemp);
            client.DeleteDeviceNotification(notificationHandler.LPHeaterTemp);
            client.DeleteDeviceNotification(notificationHandler.DeaeratorTemp);
            client.DeleteDeviceNotification(notificationHandler.HPHeaterTemp);
            client.DeleteDeviceNotification(notificationHandler.TurbineSpeed);
            client.DeleteDeviceNotification(notificationHandler.BoilerTemp);

            client.DeleteDeviceNotification(notificationHandler.MaxSpeed_CondensePump);
            client.DeleteDeviceNotification(notificationHandler.MaxSpeed_SupplyPump);
            client.DeleteDeviceNotification(notificationHandler.MaxSpeed_CircularPump);
            client.DeleteDeviceNotification(notificationHandler.MaxSpeed_InterPump);
            client.DeleteDeviceNotification(notificationHandler.MaxSpeed_ForceFan1);
            client.DeleteDeviceNotification(notificationHandler.MaxSpeed_ForceFan2);
            client.DeleteDeviceNotification(notificationHandler.MaxSpeed_ForceFan3);
            client.DeleteDeviceNotification(notificationHandler.Condenser_Htemp);
            client.DeleteDeviceNotification(notificationHandler.Condenser_Ltemp);
            client.DeleteDeviceNotification(notificationHandler.SampleTime);

            client.AdsNotificationEx -= OnAdsNotificationEx;
            //client.Disconnect();
            logger.Disconnect();

            ConnectStatus = ConnectionStatus.Offline;
            OnValuesRefreshed();
        }

        public async Task Write<T>(string varname, T value)
        {
            await Task.Run(() =>
            {
                try
                {
                    client.WriteSymbol(varname, (T)value, false);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Không ghi được dữ liệu xuống IPC: Sai tên biến!\n Lỗi: " + e.ToString(), "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private Task RefreshValues()
        {
            return Task.Run(() =>
            {
                lock (_locker)
                {
                    try
                    {
                        scaned = DateTime.Now;
                        ReadData = client.ReadSymbol("Interfacex.ScadaInterface", typeof(MainInterface), false) as MainInterface;
                        Testtime = DateTime.Now - scaned;
                    }
                    catch (Exception e)
                    {
                        _timer.Stop();
                        MessageBox.Show("Không đọc được dữ liệu tuần hoàn từ IPC: " + e.ToString(), "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            });
        }
        private DateTime scaned;

        private void OnValuesRefreshed()
        {
            ValuesRefreshed?.Invoke(this, new EventArgs());
        }

        private void OnAdsNotificationEx(object sender, AdsNotificationExEventArgs e)
        {
            if (e.NotificationHandle == notificationHandler.ProcessReady)
                ProcessState.ProcessReady = (bool)e.Value;
            else if (e.NotificationHandle == notificationHandler.ProcessRun)
                ProcessState.ProcessRun = (bool)e.Value;
            else if (e.NotificationHandle == notificationHandler.ProcessFault)
                ProcessState.ProcessFault = (bool)e.Value;

            else if (e.NotificationHandle == notificationHandler.CondensePumpPress)
                Parameter.CondensePumpPress = e.Value as aS_PidParameter ;
            else if (e.NotificationHandle == notificationHandler.SupplyPumpPress)
                Parameter.SupplyPumpPress = e.Value as aS_PidParameter;
            //else if (e.NotificationHandle == notificationHandler.CondenserTemp)
            //    Parameter.CondenserTemp = e.Value as aS_PidParameter;
            else if (e.NotificationHandle == notificationHandler.LPHeaterTemp)
                Parameter.LPHeaterTemp = e.Value as aS_PidParameter;
            else if (e.NotificationHandle == notificationHandler.DeaeratorTemp)
                Parameter.DeaeratorTemp = e.Value as aS_PidParameter;
            else if (e.NotificationHandle == notificationHandler.HPHeaterTemp)
                Parameter.HPHeaterTemp = e.Value as aS_PidParameter;
            else if (e.NotificationHandle == notificationHandler.TurbineSpeed)
                Parameter.TurbineSpeed = e.Value as aS_PidParameter;
            else if (e.NotificationHandle == notificationHandler.BoilerTemp)
                Parameter.BoilerTemp = e.Value as aS_PidParameter;

            else if (e.NotificationHandle == notificationHandler.MaxSpeed_CondensePump)
                Parameter.MaxSpeed_CondensePump = (float)e.Value ;
            else if (e.NotificationHandle == notificationHandler.MaxSpeed_SupplyPump)
                Parameter.MaxSpeed_SupplyPump = (float)e.Value ;
            else if (e.NotificationHandle == notificationHandler.MaxSpeed_CircularPump)
                Parameter.MaxSpeed_CircularPump = (float)e.Value ;
            else if (e.NotificationHandle == notificationHandler.MaxSpeed_InterPump)
                Parameter.MaxSpeed_InterPump = (float)e.Value;
            else if (e.NotificationHandle == notificationHandler.MaxSpeed_ForceFan1)
                Parameter.MaxSpeed_ForceFan1 = (float)e.Value ;
            else if (e.NotificationHandle == notificationHandler.MaxSpeed_ForceFan2)
                Parameter.MaxSpeed_ForceFan2 = (float)e.Value ;
            else if (e.NotificationHandle == notificationHandler.MaxSpeed_ForceFan3)
                Parameter.MaxSpeed_ForceFan3 = (float)e.Value ;
            else if (e.NotificationHandle == notificationHandler.Condenser_Htemp)
                Parameter.Condenser_Htemp = (float)e.Value;
            else if (e.NotificationHandle == notificationHandler.Condenser_Ltemp)
                Parameter.Condenser_Ltemp = (float)e.Value;
            else if (e.NotificationHandle == notificationHandler.SampleTime)
                Parameter.SampleTime = (float)e.Value ;

            OnValuesRefreshed();
        }

        private void OnAlarmConfirmed(TcAlarm message, bool bRemove)
        {
            var e = ProcessAlarm(message);
            OnEventOccured(3, e);
        }

        private void OnAlarmCleared(TcAlarm message, bool bRemove)
        {
            var e = ProcessAlarm(message);
            OnEventOccured(4, e);
        }

        private void OnAlarmRaised(TcAlarm message)
        {
            var e = ProcessAlarm(message);
            OnEventOccured(2, e);
        }

        private void OnMessageSent(TcMessage message)
        {
            aS_Event EvMessage = new aS_Event();
            EvMessage.EventClass = message.GetEventClassName(langID);
            EvMessage.EventID = message.EventId;
            EvMessage.SeverityLevel = message.SeverityLevel;
            EvMessage.SourceName = message.SourceName;
            EvMessage.TimeRaised = message.TimeRaised;

            OnEventOccured(1, EvMessage);
        }

        private aS_Event ProcessAlarm (TcAlarm message)
        {
            aS_Event EvAlarm = new aS_Event();
            EvAlarm.EventClass = message.GetEventClassName(langID);
            EvAlarm.EventID = message.EventId;
            EvAlarm.IsConfirmed = message.ConfirmationState;
            EvAlarm.IsRaised = message.IsRaised;
            EvAlarm.SeverityLevel = message.SeverityLevel;
            EvAlarm.SourceName = message.SourceName;
            EvAlarm.TimeRaised = message.TimeRaised;
            EvAlarm.TimeConfirmed = message.TimeConfirmed;
            EvAlarm.TimeCleared = message.TimeCleared;
            return EvAlarm;
        }

        private void OnEventOccured(byte eventtype, aS_Event _event)
        {
            EventOccured?.Invoke(this, new EventPropEventArgs(eventtype, _event));
        }

        public void AckAllAlarms ()
        {
            logger.ConfirmAllAlarms();

        }

        public List<aS_Event> GetAllEvents()
        {
            try
            {
                List<aS_Event> le = new List<aS_Event>();

                var logged = col;//logger.GetLoggedEvents(10);

                for (uint i = 0; i < logged.Count; i++)
                {
                    aS_Event temp = new aS_Event();
                    temp.EventClass = logged[i].GetEventClassName(langID);
                    temp.EventID = logged[i].EventId;
                    temp.SeverityLevel = logged[i].SeverityLevel;
                    temp.SourceName = logged[i].SourceName;
                    temp.TimeRaised = logged[i].TimeRaised;
                    temp.TimeConfirmed = logged[i].TimeConfirmed;
                    temp.TimeCleared = logged[i].TimeCleared;
                    le.Add(temp);
                }
                return le;
            }
            catch (Exception e)
            {
                return null;
            }
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
        public aFb_Motor InterPump;

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

        public aT_Tank InterTank;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public class AutoCtrlCmds
    {
        public bool Start;
        public bool Stop;
        public bool Emergency;
        public bool Reset;
        public bool processReady;
        public bool processRun;
        public bool processFault;

        public bool ProcessReady { get => processReady; set => processReady = value; }
        public bool ProcessRun { get => processRun; set => processRun = value; }
        public bool ProcessFault { get => processFault; set => processFault = value; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public class Parameters
    {
        public aS_PidParameter CondensePumpPress;
        public aS_PidParameter SupplyPumpPress;
        //public aS_PidParameter CondenserTemp;
        public aS_PidParameter LPHeaterTemp;
        public aS_PidParameter DeaeratorTemp;
        public aS_PidParameter HPHeaterTemp;
        public aS_PidParameter TurbineSpeed;
        public aS_PidParameter BoilerTemp;

        public float maxSpeed_CondensePump;
        public float maxSpeed_SupplyPump;
        public float maxSpeed_CircularPump;
        public float maxSpeed_InterPump;
        public float maxSpeed_ForceFan1;
        public float maxSpeed_ForceFan2;
        public float maxSpeed_ForceFan3;

        public float condenser_Htemp;
        public float condenser_Ltemp;

        public float sampleTime;

        public float MaxSpeed_CondensePump { get => maxSpeed_CondensePump; set => maxSpeed_CondensePump = value; }
        public float MaxSpeed_SupplyPump { get => maxSpeed_SupplyPump; set => maxSpeed_SupplyPump = value; }
        public float MaxSpeed_CircularPump { get => maxSpeed_CircularPump; set => maxSpeed_CircularPump = value; }
        public float MaxSpeed_InterPump { get => maxSpeed_InterPump; set => maxSpeed_InterPump = value; }
        public float MaxSpeed_ForceFan1 { get => maxSpeed_ForceFan1; set => maxSpeed_ForceFan1 = value; }
        public float MaxSpeed_ForceFan2 { get => maxSpeed_ForceFan2; set => maxSpeed_ForceFan2 = value; }
        public float MaxSpeed_ForceFan3 { get => maxSpeed_ForceFan3; set => maxSpeed_ForceFan3 = value; }
        public float Condenser_Htemp { get => condenser_Htemp; set => condenser_Htemp = value; }
        public float Condenser_Ltemp { get => condenser_Ltemp; set => condenser_Ltemp = value; }
        public float SampleTime { get => sampleTime; set => sampleTime = value; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public class aS_Event
    {
        private string eventClass;
        private uint eventID;
        private SeverityLevelEnum severityLevel;
        private string sourceName;
        private bool isRaised;
        private ConfirmationStateEnum isConfirmed;
        private DateTime timeRaised;
        private DateTime timeConfirmed;
        private DateTime timeCleared;

        public string EventClass { get => eventClass; set => eventClass = value; }
        public uint EventID { get => eventID; set => eventID = value; }
        public SeverityLevelEnum SeverityLevel { get => severityLevel; set => severityLevel = value; }
        public string SourceName { get => sourceName; set => sourceName = value; }
        public DateTime TimeRaised { get => timeRaised; set => timeRaised = value; }
        public bool IsRaised { get => isRaised; set => isRaised = value; }
        public ConfirmationStateEnum IsConfirmed { get => isConfirmed; set => isConfirmed = value; }
        public DateTime TimeConfirmed { get => timeConfirmed; set => timeConfirmed = value; }
        public DateTime TimeCleared { get => timeCleared; set => timeCleared = value; }
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
        public float inPercent;
        public float temperature;

        public float Temperature { get => temperature; set => temperature = value; }
        public float InPercent { get => inPercent; set => inPercent = value; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public class aT_Heater
    {
        public float inPressure;
        public float inFlow;
        public float temperature;
        public float pressure;

        public float InPressure { get => inPressure; set => inPressure = value; }
        public float InFlow { get => inFlow; set => inFlow = value; }
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

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public class aT_Tank
    {
        public bool hI;
        public bool lO;
        public float level;

        public bool HI { get => hI; set => hI = value; }
        public bool LO { get => lO; set => lO = value; }
        public float Level { get => level; set => level = value; }
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public class aS_PidParameter
    {
        public double kp;
        public double ti;
        public double td;
        public double tn;

        public double Kp { get => kp; set => kp = value; }
        public double Ti { get => ti; set => ti = value; }
        public double Td { get => td; set => td = value; }
        public double Tn { get => tn; set => tn = value; }
    }

    public enum aE_Mode { Manual = 0, Auto = 1, Service = 2}
    public enum aE_State { Fault = 0, Auto = 1, ManRun = 2, ManStop = 3}
    #endregion
}
