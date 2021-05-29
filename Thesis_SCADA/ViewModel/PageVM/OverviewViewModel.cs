using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Thesis_SCADA.Model;

namespace Thesis_SCADA.ViewModel
{
    public class OverviewViewModel : BaseViewModel
    {
        #region Properties
        public delegate void Anonym();
        private MainInterface componentModel;
        public MainInterface ComponentModel
        {
            get => componentModel;
            set
            {
                componentModel = value;
                if (componentModel != null)
                {
                    Anonym update = async () => 
                    {
                        await Task.Run(() =>
                        {
                            CondensePumpObj = componentModel?.Components?.CondensePump;
                            SupplyPumpObj = componentModel?.Components?.SupplyPump;
                            CircularPumpObj = componentModel?.Components?.CircularPump;
                            InterPumpObj = componentModel?.Components?.InterPump;
                            ForceFan1Obj = componentModel?.Components?.ForceFan1;
                            ForceFan2Obj = componentModel?.Components?.ForceFan2;
                            ForceFan3Obj = componentModel?.Components?.ForceFan3;
                            LPHValveObj = componentModel?.Components?.LPHValve;
                            DeaeratorValveObj = componentModel?.Components?.AirEjValve;
                            HPHValveObj = componentModel?.Components?.HPHValve;
                            TurbineObj = componentModel?.Components?.TurbineValve;

                            LPHeater = componentModel?.Components?.LPHeater;
                            Deaerator = componentModel?.Components?.Deaerator;
                            HPHeater = componentModel?.Components?.HPHeater;
                            Boiler = componentModel?.Components?.Boiler;
                            Condenser = componentModel?.Components?.Condenser;
                            Turbine = componentModel?.Components?.Turbine;
                            Furnace = componentModel?.Components?.Furnace;
                            InterTank = componentModel?.Components?.InterTank;
                        });
                    };

                    update();
                }
            }
        }

        private AutoCtrlCmds processModel;
        public AutoCtrlCmds ProcessModel
        {
            get => processModel;
            set
            {
                processModel = value;
                if (processModel != null)
                {
                    ProcessReady = (bool)processModel?.ProcessReady;
                    ProcessRun = (bool)processModel?.ProcessRun;
                    ProcessFault = (bool)processModel?.ProcessFault;
                }
            }
        }

        #region Process States
        private bool processReady = false;
        public bool ProcessReady { get => processReady; set { processReady = value; OnPropertyChanged(); } }

        private bool processRun = false;
        public bool ProcessRun { get => processRun; set { processRun = value; OnPropertyChanged(); } }

        private bool processFault = false;
        public bool ProcessFault { get => processFault; set { processFault = value; OnPropertyChanged(); } }
        #endregion

        #region Components
        private aFb_Motor condensePumpObj = new aFb_Motor();
        public aFb_Motor CondensePumpObj { get => condensePumpObj; set { condensePumpObj = value; OnPropertyChanged(); } }

        private string condensePumpTag = "PnID.CondensePump";
        public string CondensePumpTag { get => condensePumpTag;  }

        private aFb_Motor supplyPumpObj = new aFb_Motor();
        public aFb_Motor SupplyPumpObj { get => supplyPumpObj; set { supplyPumpObj = value; OnPropertyChanged(); } }

        private string supplyPumpTag = "PnID.SupplyPump";
        public string SupplyPumpTag { get => supplyPumpTag; }

        private aFb_Motor circularPumpObj = new aFb_Motor();
        public aFb_Motor CircularPumpObj { get => circularPumpObj; set { circularPumpObj = value; OnPropertyChanged(); } }

        private string circularPumpTag = "PnID.CircularPump";
        public string CircularPumpTag { get => circularPumpTag; }

        private aFb_Motor interPumpObj = new aFb_Motor();
        public aFb_Motor InterPumpObj { get => interPumpObj; set { interPumpObj = value; OnPropertyChanged(); } }

        private string interPumpTag = "PnID.InterPump";
        public string InterPumpTag { get => interPumpTag; }

        private aFb_Motor forceFan1Obj = new aFb_Motor();
        public aFb_Motor ForceFan1Obj { get => forceFan1Obj; set { forceFan1Obj = value; OnPropertyChanged(); } }

        private string forceFan1Tag = "PnID.ForceFan1";
        public string ForceFan1Tag { get => forceFan1Tag; }

        private aFb_Motor forceFan2Obj = new aFb_Motor();
        public aFb_Motor ForceFan2Obj { get => forceFan2Obj; set { forceFan2Obj = value; OnPropertyChanged(); } }

        private string forceFan2Tag = "PnID.ForceFan2";
        public string ForceFan2Tag { get => forceFan2Tag; }

        private aFb_Motor forceFan3Obj = new aFb_Motor();
        public aFb_Motor ForceFan3Obj { get => forceFan3Obj; set { forceFan3Obj = value; OnPropertyChanged(); } }

        private string forceFan3Tag = "PnID.ForceFan3";
        public string ForceFan3Tag { get => forceFan3Tag; }

        private aFb_Valve lPHValveObj = new aFb_Valve();
        public aFb_Valve LPHValveObj { get => lPHValveObj; set { lPHValveObj = value; OnPropertyChanged(); } }

        private string lPHValveTag = "PnID.LPHeaterValve";
        public string LPHValveTag { get => lPHValveTag; }

        private aFb_Valve deaeratorValveObj = new aFb_Valve();
        public aFb_Valve DeaeratorValveObj { get => deaeratorValveObj; set { deaeratorValveObj = value; OnPropertyChanged(); } }

        private string deaeratorTag = "PnID.AirEjectorValve";
        public string DeaeratorTag { get => deaeratorTag; }

        private aFb_Valve hPHValveObj = new aFb_Valve();
        public aFb_Valve HPHValveObj { get => hPHValveObj; set { hPHValveObj = value; OnPropertyChanged(); } }

        private string hPHValveTag = "PnID.HPHeaterValve";
        public string HPHValveTag { get => hPHValveTag; }

        private aFb_Valve turbineObj = new aFb_Valve();
        public aFb_Valve TurbineObj { get => turbineObj; set { turbineObj = value; OnPropertyChanged(); } }

        private string turbineTag = "PnID.TurbineValve";
        public string TurbineTag { get => turbineTag; }

        private aT_Heater lPHeater = new aT_Heater();
        public aT_Heater LPHeater { get => lPHeater; set { lPHeater = value; OnPropertyChanged(); } }

        private aT_Heater deaerator = new aT_Heater();
        public aT_Heater Deaerator { get => deaerator; set { deaerator = value; OnPropertyChanged(); } }

        private aT_Heater hPHeater = new aT_Heater();
        public aT_Heater HPHeater { get => hPHeater; set { hPHeater = value; OnPropertyChanged(); } }

        private aT_Heater boiler = new aT_Heater();
        public aT_Heater Boiler { get => boiler; set { boiler = value; OnPropertyChanged(); } }

        private aT_Heater condenser = new aT_Heater();
        public aT_Heater Condenser { get => condenser; set { condenser = value; OnPropertyChanged(); } }

        private aT_Furnace furnace = new aT_Furnace();
        public aT_Furnace Furnace { get => furnace; set { furnace = value; OnPropertyChanged(); } }

        private aT_Turbine turbine = new aT_Turbine();
        public aT_Turbine Turbine { get => turbine; set { turbine = value; OnPropertyChanged(); } }

        private aT_Tank interTank = new aT_Tank();
        public aT_Tank InterTank { get => interTank; set { interTank = value; OnPropertyChanged(); } }

        #endregion
        #endregion

        #region commands
        public ICommand LoadedCommand { get; set; }
        public ICommand UnloadedCommand { get; set; }

        public ICommand StartPressedCommand { get; set; }
        public ICommand StartReleasedCommand { get; set; }
        public ICommand StopPressedCommand { get; set; }
        public ICommand StopReleasedCommand { get; set; }
        public ICommand ResetPressedCommand { get; set; }
        public ICommand ResetReleasedCommand { get; set; }
        public ICommand EmergencyPressedCommand { get; set; }
        public ICommand EmergencyReleasedCommand { get; set; }

        #endregion

        public OverviewViewModel()
        {
            
            ComponentModel = GlobalVar.Ins.IpcData;
            GlobalVar.Ins.DataChanged -= OnModelChanged;
            GlobalVar.Ins.DataChanged += OnModelChanged;

            LoadedCommand = new RelayCommand<Grid>((p) => { return true; }, (p) => {
                var role = GlobalVar.Ins.User.UserRole.Id;
                if (role == 5) p.Visibility = Visibility.Collapsed;
            });

            UnloadedCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
                //GlobalVar.Ins.DataChanged -= OnModelChanged; 
            });

            #region Button Command

            StartPressedCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
                GlobalVar.Ins.WriteData<bool>("Interfacex.AutoCtrlCmds.Start", true);
            });

            StartReleasedCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
                GlobalVar.Ins.WriteData<bool>("Interfacex.AutoCtrlCmds.Start", false);
            });

            StopPressedCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
                GlobalVar.Ins.WriteData<bool>("Interfacex.AutoCtrlCmds.Stop", true);
            });

            StopReleasedCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
                GlobalVar.Ins.WriteData<bool>("Interfacex.AutoCtrlCmds.Stop", false);
            });

            ResetPressedCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
                GlobalVar.Ins.WriteData<bool>("Interfacex.AutoCtrlCmds.Reset", true);
            });

            ResetReleasedCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
                GlobalVar.Ins.WriteData<bool>("Interfacex.AutoCtrlCmds.Reset", false);
            });

            EmergencyPressedCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
                GlobalVar.Ins.WriteData<bool>("Interfacex.AutoCtrlCmds.Emergency", true);
            });

            EmergencyReleasedCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
                GlobalVar.Ins.WriteData<bool>("Interfacex.AutoCtrlCmds.Emergency", false);
            });

            #endregion

        }

        private async void OnModelChanged(object sender, EventArgs e)
        {
            await Task.Run(() => 
            {
                ComponentModel = GlobalVar.Ins.IpcData;
                ProcessModel = GlobalVar.Ins.ProcessState;
            });
        }
    }
}
