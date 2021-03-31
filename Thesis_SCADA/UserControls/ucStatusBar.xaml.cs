using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Thesis_SCADA.ViewModel;

namespace Thesis_SCADA.UserControls
{
    /// <summary>
    /// Interaction logic for ucStatusBar.xaml
    /// </summary>
    public partial class ucStatusBar : UserControl
    {
        public ucStatusBarViewModel ViewModel { get; set; }

        public ucStatusBar()
        {
            InitializeComponent();
            this.DataContext = ViewModel = new ucStatusBarViewModel();
        }
    }
}
