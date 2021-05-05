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
        #endregion

        #region Commands
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }

        #endregion

        public UserViewModel()
        {
            //Cập nhật danh sách vào ListView khi mở window
            List = new ObservableCollection<Users>(DataProvider.Ins.DB.Users);
            Role = new ObservableCollection<UserRole>(DataProvider.Ins.DB.UserRole);

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

        }
    }

}
