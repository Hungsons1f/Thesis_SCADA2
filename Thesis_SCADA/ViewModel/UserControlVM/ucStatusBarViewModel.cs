using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Thesis_SCADA.Model;
//using static Thesis_SCADA.Model.GlobalVar;

namespace Thesis_SCADA.ViewModel
{
    public class ucStatusBarViewModel : BaseViewModel
    {

        #region Properties
        private ConnectionStatus connectionStatus = ConnectionStatus.Offline;
        public ConnectionStatus ConnectionStatus { get => connectionStatus; set { connectionStatus = value; OnPropertyChanged(); } }

        private TimeSpan scanTime = TimeSpan.Zero;
        public TimeSpan ScanTime { get => scanTime; set { scanTime = value; OnPropertyChanged(); } }

        private ushort alarmNum = 0;
        public ushort AlarmNum { get => alarmNum; set { alarmNum = value; OnPropertyChanged(); } }

        private DateTime currentTime = DateTime.Now;
        public DateTime CurrentTime { get => currentTime; set { currentTime = value; OnPropertyChanged(); } }

        #endregion

        #region commands 
        public ICommand LoadedCommand { get; set; }
        #endregion

        public ucStatusBarViewModel()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();

            LoadedCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) => {
            });

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            CurrentTime = DateTime.Now;
        }
    }
}
