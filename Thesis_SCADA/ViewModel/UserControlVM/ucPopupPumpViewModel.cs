using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Thesis_SCADA.UserControls;

namespace Thesis_SCADA.ViewModel
{
    public class ucPopupPumpViewModel : BaseViewModel
    {
        #region Properties
        //private DataTemplate symbol;
        //public DataTemplate Symbol { get => symbol; set { symbol = value; OnPropertyChanged(); } }
        #endregion

        #region Commands
        public ICommand CloseCommand { get; set; }

        #endregion

        public ucPopupPumpViewModel()
        {
            CloseCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) => {
                if (p == null) return;

                if (p.Visibility == Visibility.Visible)
                    p.Visibility = Visibility.Collapsed;
                else
                    p.Visibility = Visibility.Visible;
            });

        }
    }
}
