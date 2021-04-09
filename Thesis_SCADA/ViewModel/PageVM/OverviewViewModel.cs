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
                    CondensePumpObj = model.Components.CondensePump;
                }
            }
        }

        private aFb_Motor condensePumpObj = new aFb_Motor();
        public aFb_Motor CondensePumpObj { get => condensePumpObj; set { condensePumpObj = value; OnPropertyChanged(); } }

        #endregion

        #region commands
        public ICommand UnloadedCommand { get; set; }
        

        #endregion

        public OverviewViewModel()
        {
            
            Model = GlobalVar.Ins.IpcData;
            GlobalVar.Ins.DataChanged += OnModelChanged;

            UnloadedCommand = new RelayCommand<object>((p) => { return true; }, (p) => { GlobalVar.Ins.DataChanged -= OnModelChanged; });

            MainInterface mainInterface = new MainInterface();
            mainInterface.Components = new Components();
            mainInterface.Components.CondensePump = new aFb_Motor();
            mainInterface.Components.CondensePump.ActualSpeed = 1400;
            mainInterface.Components.CondensePump.Maxspeed = 1500;
            GlobalVar.Ins.IpcData = mainInterface;

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
