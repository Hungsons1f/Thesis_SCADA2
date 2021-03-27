using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Thesis_SCADA.Model;

namespace Thesis_SCADA.ViewModel
{
    public class MainViewModel: BaseViewModel
    {
        #region commands
        public ICommand UserManagementCommand { get; set; }
        public ICommand WindowLoadedCommand { get; set; }
        #endregion

        public MainViewModel()
        {
            WindowLoadedCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                if (p == null) return;

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


        }
    }
}
