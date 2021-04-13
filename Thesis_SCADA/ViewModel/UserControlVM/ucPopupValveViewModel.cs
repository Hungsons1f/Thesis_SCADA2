using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Thesis_SCADA.Model;

namespace Thesis_SCADA.ViewModel.UserControlVM
{
    public class ucPopupValveViewModel: BaseViewModel
    {
        #region Properties
        private string setpointName = "Độ mở đặt (%): ";
        public string SetpointName { get => setpointName; set { setpointName = value; OnPropertyChanged(); } }

        #endregion

        #region Commands
        public ICommand CloseCommand { get; set; }

        public ICommand ModeChangedCommand { get; set; }

        public ICommand StartPressedCommand { get; set; }
        public ICommand StartReleasedCommand { get; set; }
        public ICommand StopPressedCommand { get; set; }
        public ICommand StopReleasedCommand { get; set; }
        public ICommand ResetPressedCommand { get; set; }
        public ICommand ResetReleasedCommand { get; set; }

        public ICommand SetpointEnteredCommand { get; set; }
        //public ICommand RpmSelectedCommand { get; set; }
        //public ICommand M3hSelectedCommand { get; set; }
        #endregion

        public ucPopupValveViewModel()
        {
            CloseCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) => {
                if (p == null) return;

                if (p.Visibility == Visibility.Visible)
                    p.Visibility = Visibility.Collapsed;
                else
                    p.Visibility = Visibility.Visible;
            });

            ModeChangedCommand = new RelayCommand<object[]>((p) => { return true; }, (p) => {
                if (p == null) return;
                int val = (int)p[0];
                string prefix = (string)p[1];

                GlobalVar.Ins.WriteData<short>(prefix + ".Mode", (short)val);
            });

            #region Button Command

            StartPressedCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null) return;

                GlobalVar.Ins.WriteData<bool>(p + ".Start", true);
            });

            StartReleasedCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null) return;

                GlobalVar.Ins.WriteData<bool>(p + ".Start", false);
            });

            StopPressedCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null) return;

                GlobalVar.Ins.WriteData<bool>(p + ".Stop", true);
            });

            StopReleasedCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null) return;

                GlobalVar.Ins.WriteData<bool>(p + ".Stop", false);
            });

            ResetPressedCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null) return;

                GlobalVar.Ins.WriteData<bool>(p + ".Reset1", true);
            });

            ResetReleasedCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null) return;

                GlobalVar.Ins.WriteData<bool>(p + ".Reset1", false);
            });

            #endregion

            #region Setpoint Command

            SetpointEnteredCommand = new RelayCommand<object[]>((p) => { return true; }, (p) => {
                if (p == null) return;
                string val = (string)p[0];
                string prefix = (string)p[1];

                GlobalVar.Ins.WriteData<float>(prefix + ".SetPercent", float.Parse(val)/100);
            });

            //RpmSelectedCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
            //    SetpointName = "Tốc độ đặt (RPM): ";
            //});

            //M3hSelectedCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
            //    SetpointName = "Lưu lượng đặt (m3/h): ";
            //});

            #endregion
        }

    }
}
