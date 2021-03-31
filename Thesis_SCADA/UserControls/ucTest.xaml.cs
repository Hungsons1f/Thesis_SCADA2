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
    /// Interaction logic for ucTest.xaml
    /// </summary>
    public partial class ucTest : UserControl
    {
        #region DP NavRegion
        public Frame NavFrame
        {
            get { return (Frame)GetValue(NavFrameProperty); }
            set { SetValue(NavFrameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for navframe.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NavFrameProperty =
            DependencyProperty.Register("NavFrame", typeof(Frame), typeof(ucTest), new FrameworkPropertyMetadata(null));
        #endregion

        public ucTestViewModel ViewModel { get; set; }
        public ucTest()
        {
            InitializeComponent();
            this.DataContext = ViewModel = new ucTestViewModel();
        }
    }
}
