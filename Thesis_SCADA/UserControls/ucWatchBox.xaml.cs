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

namespace Thesis_SCADA.UserControls
{
    /// <summary>
    /// Interaction logic for ucWatchBox.xaml
    /// </summary>
    public partial class ucWatchBox : UserControl
    {
        #region Dependency Properties
        public float Data
        {
            get { return (float)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for navframe.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(float), typeof(ucWatchBox), new FrameworkPropertyMetadata((float)0));
        #endregion


        public ucWatchBox()
        {
            InitializeComponent();
        }
    }
}
