using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Thesis_SCADA.Model;

namespace Thesis_SCADA.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        public bool isLogin { get; set; }
        private string _userName = "";
        private string _password = "";

        public string UserName { get => _userName; set { _userName = value; OnPropertyChanged(); } }
        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }

        public Users LoginUser;
        #region commands
        public ICommand LoginCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }

        #endregion

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { Login(p); });
            CloseCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { p.Close(); });
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });

        }

        void Login(Window p)
        {
            if (p == null) return;

            string passEncode = Encode.MD5Hash(Encode.Base64Encode(Password));
            //var countAcc = DataProvider.Ins.DB.Users.Where(x => x.UserName == UserName && x.Password == passEncode).Count();

            //if (countAcc > 0)
            //{
            //    isLogin = true;
            //    p.Close();
            //}
            //else
            //{
            //    isLogin = false;
            //    MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            //}

            Users countAcc = DataProvider.Ins.DB.Users.Where(x => x.UserName == UserName && x.Password == passEncode).FirstOrDefault();

            if (countAcc != null)
            {
                isLogin = true;
                p.Close();
                LoginUser = countAcc;
            }
            else
            {
                isLogin = false;
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
