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
        private string pageName = "SCADA - Trang chủ";
        public string PageName { get => pageName; set { pageName = value; OnPropertyChanged(); } }

        private bool menuToggle = false;
        public bool MenuToggle { get => menuToggle; set { menuToggle = value; OnPropertyChanged(); } }

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
        public ICommand PageTrendBtnCommand { get; set; }
        public ICommand PageEventBtnCommand { get; set; }
        public ICommand PageReportBtnCommand { get; set; }
        public ICommand PageUserBtnCommand { get; set; }
        public ICommand PageSettingBtnCommand { get; set; }
        #endregion

        public MainViewModel()
        {
            var a = GlobalVar.Ins.ConnectStatus;

            WindowLoadedCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                if (p == null) return;

                p.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight - 15;
                p.MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
                p.WindowState = WindowState.Maximized;
                p.Hide();

                if (!DataProvider.Ins.DB.Database.Exists())
                {
                    MessageBox.Show("Không có SQL Server", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    p.Close();
                }

                LoginWindow login = new LoginWindow();
                login.ShowDialog();

                if (login.DataContext == null) return;

                var loginVM = login.DataContext as LoginViewModel;
                if (loginVM.isLogin)
                {
                    p.Show();
                    GlobalVar.Ins.User = loginVM.LoginUser;
                }
                else
                {
                    p.Close();
                }
            });

            OpenDrawerCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
                MenuToggle = true;
                DrawerHost.OpenDrawerCommand.Execute(Dock.Left, null);
            });

            CloseDrawerCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
                MenuToggle = false;
                DrawerHost.CloseDrawerCommand.Execute(Dock.Left, null);
            });

            #region PageCommands
            PageOverviewBtnCommand = new RelayCommand<Frame>((p) => { return true; }, (p) => {
                p.Navigate(new Uri("Pages/Overview.xaml", UriKind.RelativeOrAbsolute));
                DrawerHost.CloseDrawerCommand.Execute(null, null);
                PageName = "SCADA - P&ID";
                MenuToggle = false;
            });

            PageZone1BtnCommand = new RelayCommand<Frame>((p) => { return true; }, (p) => {
                p.Navigate(new Uri("Pages/Zone1.xaml", UriKind.RelativeOrAbsolute));
                DrawerHost.CloseDrawerCommand.Execute(null, null);
                MenuToggle = false;
            });

            PageZone2BtnCommand = new RelayCommand<Frame>((p) => { return true; }, (p) => {
                p.Navigate(new Uri("Pages/Zone2.xaml", UriKind.RelativeOrAbsolute));
                DrawerHost.CloseDrawerCommand.Execute(null, null);
                MenuToggle = false;
            });

            PageCalibBtnCommand = new RelayCommand<Frame>((p) => {
                var role = GlobalVar.Ins.User.UserRole.Id;
                if (role == 3 || role == 4 || role == 5) return false;
                else return true;
            }, (p) => {
                p.Navigate(new Uri("Pages/Calib.xaml", UriKind.RelativeOrAbsolute));
                DrawerHost.CloseDrawerCommand.Execute(null, null);
                PageName = "SCADA - Hiệu chỉnh";
                MenuToggle = false;
            });

            PageTrendBtnCommand = new RelayCommand<Frame>((p) => { return true; }, (p) => {
                p.Navigate(new Uri("Pages/Trend.xaml", UriKind.RelativeOrAbsolute));
                DrawerHost.CloseDrawerCommand.Execute(null, null);
                PageName = "SCADA - Đồ thị";
                MenuToggle = false;
            });

            PageEventBtnCommand = new RelayCommand<Frame>((p) => { return true; }, (p) => {
                p.Navigate(new Uri("Pages/Event.xaml", UriKind.RelativeOrAbsolute));
                DrawerHost.CloseDrawerCommand.Execute(null, null);
                PageName = "SCADA - Sự kiện";
                MenuToggle = false;
            });

            PageReportBtnCommand = new RelayCommand<Frame>((p) => {
                var role = GlobalVar.Ins.User.UserRole.Id;
                if (role == 5) return false;
                else return true;
            }, (p) => {
                p.Navigate(new Uri("Pages/Report.xaml", UriKind.RelativeOrAbsolute));
                DrawerHost.CloseDrawerCommand.Execute(null, null);
                PageName = "SCADA - Báo cáo";
                MenuToggle = false;
            });

            PageUserBtnCommand = new RelayCommand<Frame>((p) => { return true; }, (p) => {
                p.Navigate(new Uri("Pages/User.xaml", UriKind.RelativeOrAbsolute));
                DrawerHost.CloseDrawerCommand.Execute(null, null);
                PageName = "SCADA - Người dùng";
                MenuToggle = false;
            });

            PageSettingBtnCommand = new RelayCommand<Frame>((p) => {
                var role = GlobalVar.Ins.User.UserRole.Id;
                if (role == 3 || role == 4 || role == 5) return false;
                else return true;
            }, (p) => {
                p.Navigate(new Uri("Pages/Setting.xaml", UriKind.RelativeOrAbsolute));
                DrawerHost.CloseDrawerCommand.Execute(null, null);
                PageName = "SCADA - Cài đặt";
                MenuToggle = false;
            });
            #endregion
        }
    }
}
