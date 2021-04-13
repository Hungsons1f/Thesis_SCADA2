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
        #region Test
        private readonly System.Timers.Timer _timer;

        #endregion

        #region Properties
        private MainInterface model;
        public MainInterface Model
        {
            get => model;
            set
            {
                model = value;
                //OnPropertyChanged();

                if (model != null)
                {
                    CondensePumpObj = model?.Components?.CondensePump;
                    SupplyPumpObj = model?.Components?.SupplyPump;
                    CircularPumpObj = model?.Components?.CircularPump;
                    ForceFan1Obj = model?.Components?.ForceFan1;
                    ForceFan2Obj = model?.Components?.ForceFan2;
                    ForceFan3Obj = model?.Components?.ForceFan3;
                    LPHValveObj = model?.Components?.LPHValve;
                    DeaeratorValveObj = model?.Components?.AirEjValve;
                    HPHValveObj = model?.Components?.HPHValve;
                    TurbineObj = model?.Components?.TurbineValve;
                }
            }
        }

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
        #endregion
        #endregion

        #region commands
        public ICommand LoadedCommand { get; set; }
        public ICommand UnloadedCommand { get; set; }
        

        #endregion

        public OverviewViewModel()
        {
            
            Model = GlobalVar.Ins.IpcData;
            GlobalVar.Ins.DataChanged -= OnModelChanged;
            GlobalVar.Ins.DataChanged += OnModelChanged;

            LoadedCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
                //GlobalVar.Ins.DataChanged += OnModelChanged;
            });

            UnloadedCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
                //GlobalVar.Ins.DataChanged -= OnModelChanged; 
            });

            //_timer = new System.Timers.Timer();
            //_timer.Interval = 1000;
            //_timer.Elapsed += OnTimerElapsed;
            //_timer.Start();
            //Test2 = "1";
        }

        private void OnModelChanged(object sender, EventArgs e)
        {
            Model = GlobalVar.Ins.IpcData;
        }

        //private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        //{
        //    //MainInterface mainInterface = new MainInterface();
        //    //mainInterface.Components = new Components();
        //    //mainInterface.Components.CondensePump = new aFb_Motor();
        //    //if ((GlobalVar.Ins.IpcData != null) && (GlobalVar.Ins.IpcData.Components.CondensePump.Fault == true))
        //    //    mainInterface.Components.CondensePump.Fault = false;
        //    //else
        //    //    mainInterface.Components.CondensePump.Fault = true;
        //    //GlobalVar.Ins.IpcData = mainInterface;
        //    int i = Int32.Parse(Test2) + 1;
        //    Test2 = i.ToString();
        //}
    }
}
