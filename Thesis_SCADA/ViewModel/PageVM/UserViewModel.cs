using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Thesis_SCADA.Model;

namespace Thesis_SCADA.ViewModel
{
    public class UserViewModel : BaseViewModel
    {
        #region Properties
        //Danh sách hiển thị trên ListView
        private ObservableCollection<Users> _List;
        public ObservableCollection<Users> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private ObservableCollection<UserRole> _Role;
        public ObservableCollection<UserRole> Role { get => _Role; set { _Role = value; OnPropertyChanged(); } }

        //Thuộc tính để khi chọn một dòng trên ListView, dòng đó sẽ được truyền vào (dùng Mode Onewaytosource)
        private Users _SelectedItem;
        public Users SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    //Khi dòng được truyền thì các textbox và combobox sẽ nhận dữ liệu để hiển thị và cho phép người dùng chỉnh sửa
                    UserName = SelectedItem.UserName;
                    SelectedRole = SelectedItem.UserRole;
                }
            }
        }

        private string _UserName;
        public string UserName { get => _UserName; set { _UserName = value; OnPropertyChanged(); } }

        private UserRole _SelectedRole;
        public UserRole SelectedRole { get => _SelectedRole; set { _SelectedRole = value; OnPropertyChanged(); } }

        private string _Password;
        public string Password { get => _Password; set { _Password = value; OnPropertyChanged(); } }

        private string currentUser;
        public string CurrentUser { get => currentUser; set { currentUser = value; OnPropertyChanged(); } }

        private string oldPass;
        public string OldPass { get => oldPass; set { oldPass = value; OnPropertyChanged(); } }

        private string newPass;
        public string NewPass { get => newPass; set { newPass = value; OnPropertyChanged(); } }

        private Visibility adminExtra;
        public Visibility AdminExtra { get => adminExtra; set { adminExtra = value; OnPropertyChanged(); } }

        #endregion

        #region Commands
        public ICommand AdminLoadedCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand OldPassChangedCommand { get; set; }
        public ICommand NewPassChangedCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        public ICommand ChangePasswordCommand { get; set; }

        #endregion

        public UserViewModel()
        {
            //Cập nhật danh sách vào ListView khi mở window
            List = new ObservableCollection<Users>(DataProvider.Ins.DB.Users);
            Role = new ObservableCollection<UserRole>(DataProvider.Ins.DB.UserRole);

            CurrentUser = GlobalVar.Ins.User.UserName;

            AdminLoadedCommand = new RelayCommand<Grid>((p) => { return true; }, (p) => {
                var role = GlobalVar.Ins.User.UserRole.Id;
                if (role != 1) AdminExtra = Visibility.Collapsed; //p.Visibility = Visibility.Collapsed;
                else AdminExtra = Visibility.Visible;
            });

            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });

            AddCommand = new RelayCommand<object>((p) =>
            {
                if (String.IsNullOrEmpty(UserName)) return false;

                var displayList = DataProvider.Ins.DB.Users.Where(x => x.UserName == UserName);
                if (displayList == null || displayList.Count() != 0) return false;

                if (SelectedRole == null) return false;

                return true;
            }, (p) =>
            {
                var user = new Users() { UserName = UserName, Password = "", IdRole = SelectedRole.Id };
                if (Password != "" && Password != null)
                    user.Password = Encode.MD5Hash(Encode.Base64Encode(Password));
                DataProvider.Ins.DB.Users.Add(user);
                DataProvider.Ins.DB.SaveChanges();

                List.Add(user);
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (String.IsNullOrEmpty(UserName) || SelectedItem == null) return false;

                var displayList = DataProvider.Ins.DB.Users.Where(x => x.Id == SelectedItem.Id);
                if (displayList == null || displayList.Count() == 0) return false;

                //var displayListName = DataProvider.Ins.DB.Users.Where(x => x.UserName == UserName);
                //if (displayListName == null || displayListName.Count() != 0) return false;

                if (SelectedRole == null) return false;

                return true;
            }, (p) =>
            {
                var user = DataProvider.Ins.DB.Users.Where(x => x.Id == SelectedItem.Id).SingleOrDefault();
                user.UserName = UserName;
                user.IdRole = SelectedRole.Id;
                if (Password != "" && Password != null)
                    user.Password = Encode.MD5Hash(Encode.Base64Encode(Password));
                DataProvider.Ins.DB.SaveChanges();

            });

            DeleteCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null) return false;

                return true;
            }, (p) =>
            {
                DataProvider.Ins.DB.Users.Remove(SelectedItem);
                DataProvider.Ins.DB.SaveChanges();

                List.Remove(SelectedItem);
            });

            LogoutCommand = new RelayCommand<Page>((p) => { return true; }, (p) =>
            {
                if (p == null) return;
                var w = Window.GetWindow(p);
                if (w == null) return;
                w.Hide();

                LoginWindow login = new LoginWindow();
                login.ShowDialog();
                if (login.DataContext == null) return;

                var loginVM = login.DataContext as LoginViewModel;
                if (loginVM.isLogin)
                {
                    w.Show();
                    GlobalVar.Ins.User = loginVM.LoginUser;
                    CurrentUser = GlobalVar.Ins.User.UserName;
                    var role = GlobalVar.Ins.User.UserRole.Id;
                    if (role != 1) AdminExtra = Visibility.Collapsed; 
                    else AdminExtra = Visibility.Visible;
                }
                else
                {
                    w.Close();
                }
            });

            OldPassChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { OldPass = p.Password; });

            NewPassChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { NewPass = p.Password; });

            ChangePasswordCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                string oldPassEncode = Encode.MD5Hash(Encode.Base64Encode(OldPass));

                if (oldPassEncode == GlobalVar.Ins.User.Password)
                {
                    var user = DataProvider.Ins.DB.Users.Where(x => x.Id == GlobalVar.Ins.User.Id).SingleOrDefault();
                    if (NewPass != null)
                        user.Password = Encode.MD5Hash(Encode.Base64Encode(NewPass));
                    DataProvider.Ins.DB.SaveChanges();
                    MessageBox.Show("Sai mật khẩu cũ!!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Sai mật khẩu cũ!!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            });

        }
    }
}
