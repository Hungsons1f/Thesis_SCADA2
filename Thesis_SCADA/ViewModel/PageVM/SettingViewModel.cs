using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Thesis_SCADA.Model;

namespace Thesis_SCADA.ViewModel
{
    public class SettingViewModel : BaseViewModel
    {
        #region Properties
        private bool isDarkTheme;
        public bool IsDarkTheme
        {
            get => isDarkTheme;
            set
            {
                isDarkTheme = value;
                OnPropertyChanged();

                var paletteHelper = new PaletteHelper();
                var theme = paletteHelper.GetTheme();
                theme.SetBaseTheme(isDarkTheme ? Theme.Dark : Theme.Light);
                paletteHelper.SetTheme(theme);
            }
        }

        private string conNetId;
        public string ConNetId { get => conNetId; set { conNetId = value; OnPropertyChanged(); } }

        private int conPort;
        public int ConPort { get => conPort; set { conPort = value; OnPropertyChanged(); } }

        private int conCycle;
        public int ConCycle { get => conCycle; set { conCycle = value; OnPropertyChanged(); } }

        private int dbCycle;
        public int DbCycle { get => dbCycle; set { dbCycle = value; OnPropertyChanged(); } }

        private bool isConnected;
        #endregion

        #region Commands
        public ICommand LoadedCommand { get; set; }
        public ICommand ConnectCommand { get; set; }
        public ICommand DisconnectCommand { get; set; }
        public ICommand SetDBCycleCommand { get; set; }
        public ICommand SetConCycleCommand { get; set; }
        #endregion

        public SettingViewModel()
        {
            PaletteHelper paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();
            IsDarkTheme = theme.GetBaseTheme() == BaseTheme.Dark;

            isConnected = (GlobalVar.Ins.ConnectStatus == ConnectionStatus.Online) ? true : false;
            ConNetId = GlobalVar.Ins.NetID;
            ConPort = GlobalVar.Ins.Port;
            ConCycle = GlobalVar.Ins.IpcCycle;
            DbCycle = GlobalVar.Ins.DbCycle;

            LoadedCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                isConnected = (GlobalVar.Ins.ConnectStatus == ConnectionStatus.Online) ? true : false;
            });

            ConnectCommand = new RelayCommand<object>((p) => { return isConnected ? false : true; }, (p) => 
            {
                if (GlobalVar.Ins.Connect(ConNetId, ConPort, true))
                    isConnected = true;
            });

            DisconnectCommand = new RelayCommand<object>((p) => { return isConnected ? true : false; }, (p) =>
            {
                GlobalVar.Ins.Disconnect();
                GlobalVar.Ins.ScanTime = TimeSpan.FromMilliseconds(0);
                isConnected = false;
            });

            SetConCycleCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                GlobalVar.Ins.IpcCycle = ConCycle;
            });

            SetDBCycleCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                GlobalVar.Ins.DbCycle = DbCycle;
            });
        }
    }

}
