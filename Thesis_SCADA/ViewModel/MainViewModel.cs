using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Thesis_SCADA.Model;
using Thesis_SCADA.Pages;

namespace Thesis_SCADA.ViewModel
{
    public class MainViewModel: BaseViewModel
    {
        #region properties
        private AdsDataService ipcData;
        #endregion

        #region commands
        public ICommand UserManagementCommand { get; set; }
        public ICommand WindowLoadedCommand { get; set; }

        public ICommand OpenDrawerCommand { get; set; }
        public ICommand CloseDrawerCommand { get; set; }

        public ICommand PageOverviewBtnCommand { get; set; }
        public ICommand PageZone1BtnCommand { get; set; }
        public ICommand PageZone2BtnCommand { get; set; }
        public ICommand PageCalibBtnCommand { get; set; }
        public ICommand PageAlarmBtnCommand { get; set; }
        public ICommand PageTrendBtnCommand { get; set; }
        public ICommand PageEventBtnCommand { get; set; }
        public ICommand PageReportBtnCommand { get; set; }
        public ICommand PageUserBtnCommand { get; set; }
        public ICommand PageSettingBtnCommand { get; set; }
        #endregion

        public MainViewModel()
        {
            ipcData = new AdsDataService();
            OnIpcDataRefreshed(null, null);
            ipcData.ValuesRefreshed += OnIpcDataRefreshed;

            WindowLoadedCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                if (p == null) return;

                p.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight - 15;
                p.MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
                p.WindowState = WindowState.Maximized;

                //p.Hide();
                //LoginWindow login = new LoginWindow();
                //login.ShowDialog();

                //if (login.DataContext == null) return;

                //var loginVM = login.DataContext as LoginViewModel;
                //if (loginVM.isLogin)
                //{
                //    p.Show();
                //}
                //else
                //{
                //    p.Close();
                //}

            });

            UserManagementCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
                UserManagementWindow w = new UserManagementWindow();
                w.ShowDialog();
            });

            OpenDrawerCommand = new RelayCommand<ToggleButton>((p) => { return true; }, (p) => {
                p.IsChecked = true;
                DrawerHost.OpenDrawerCommand.Execute(Dock.Left, null);
            });

            CloseDrawerCommand = new RelayCommand<ToggleButton>((p) => { return true; }, (p) => {
                p.IsChecked = false;
                DrawerHost.CloseDrawerCommand.Execute(Dock.Left, null);
            });

            #region PageCommands
            PageOverviewBtnCommand = new RelayCommand<Frame>((p) => { return true; }, (p) => {
                p.Navigate(new Uri("Pages/Overview.xaml", UriKind.RelativeOrAbsolute));
                DrawerHost.CloseDrawerCommand.Execute(null, null);
            });

            PageZone1BtnCommand = new RelayCommand<Frame>((p) => { return true; }, (p) => {
                p.Navigate(new Uri("Pages/Zone1.xaml", UriKind.RelativeOrAbsolute));
                DrawerHost.CloseDrawerCommand.Execute(null, null);
            });

            PageZone2BtnCommand = new RelayCommand<Frame>((p) => { return true; }, (p) => {
                p.Navigate(new Uri("Pages/Zone2.xaml", UriKind.RelativeOrAbsolute));
                DrawerHost.CloseDrawerCommand.Execute(null, null);
            });

            PageCalibBtnCommand = new RelayCommand<Frame>((p) => { return true; }, (p) => {
                p.Navigate(new Uri("Pages/Calib.xaml", UriKind.RelativeOrAbsolute));
                DrawerHost.CloseDrawerCommand.Execute(null, null);
            });

            PageAlarmBtnCommand = new RelayCommand<Frame>((p) => { return true; }, (p) => {
                p.Navigate(new Uri("Pages/Alarm.xaml", UriKind.RelativeOrAbsolute));
                DrawerHost.CloseDrawerCommand.Execute(null, null);
            });

            PageTrendBtnCommand = new RelayCommand<Frame>((p) => { return true; }, (p) => {
                p.Navigate(new Uri("Pages/Trend.xaml", UriKind.RelativeOrAbsolute));
                DrawerHost.CloseDrawerCommand.Execute(null, null);
            });

            PageEventBtnCommand = new RelayCommand<Frame>((p) => { return true; }, (p) => {
                p.Navigate(new Uri("Pages/Event.xaml", UriKind.RelativeOrAbsolute));
                DrawerHost.CloseDrawerCommand.Execute(null, null);
            });

            PageReportBtnCommand = new RelayCommand<Frame>((p) => { return true; }, (p) => {
                p.Navigate(new Uri("Pages/Report.xaml", UriKind.RelativeOrAbsolute));
                DrawerHost.CloseDrawerCommand.Execute(null, null);
            });

            PageUserBtnCommand = new RelayCommand<Frame>((p) => { return true; }, (p) => {
                p.Navigate(new Uri("Pages/User.xaml", UriKind.RelativeOrAbsolute));
                DrawerHost.CloseDrawerCommand.Execute(null, null);
            });

            PageUserBtnCommand = new RelayCommand<Frame>((p) => { return true; }, (p) => {
                p.Navigate(new Uri("Pages/User.xaml", UriKind.RelativeOrAbsolute));
                DrawerHost.CloseDrawerCommand.Execute(null, null);
            });

            PageSettingBtnCommand = new RelayCommand<Frame>((p) => { return true; }, (p) => {
                p.Navigate(new Uri("Pages/Setting.xaml", UriKind.RelativeOrAbsolute));
                DrawerHost.CloseDrawerCommand.Execute(null, null);
            });
            #endregion
        }

        private void OnIpcDataRefreshed(object sender, EventArgs e)
        {
            GlobalVar.ipcData = ipcData.PlcData;
        }
    }
}
