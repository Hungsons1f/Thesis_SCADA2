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
    public class OverviewViewModel : BaseViewModel
    {
        #region Properties

        private string _userName = "";
        public string UserName { get => _userName; set { _userName = value; OnPropertyChanged(); } }
        #endregion

        #region commands
        public ICommand LoginCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }

        #endregion

        public OverviewViewModel()
        {

            LoginCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {  });
            CloseCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { p.Close(); });

        }

    }
}
