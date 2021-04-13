using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Thesis_SCADA.Model;
using Thesis_SCADA.UserControls;

namespace Thesis_SCADA.ViewModel
{
    public class ucGaugeViewModel : BaseViewModel
    {
        #region Properties
        private DataTemplate symbol;
        public DataTemplate Symbol { get => symbol; set { symbol = value; OnPropertyChanged(); } }
        #endregion

        public ucGaugeViewModel()
        {
        }
    }
}
