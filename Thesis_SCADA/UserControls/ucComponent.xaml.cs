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
    /// Interaction logic for ucComponent.xaml
    /// </summary>
    public partial class ucComponent : UserControl
    {
        #region DependencyProperty
        public byte Data
        {
            get { return (byte)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for navframe.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(byte), typeof(ucComponent), new FrameworkPropertyMetadata((byte)0));

        public DataTemplate NormalSymbol
        {
            get { return (DataTemplate)GetValue(NormalSymbolProperty); }
            set { SetValue(NormalSymbolProperty, value); }
        }

        // Using a DependencyProperty as the backing store for navframe.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NormalSymbolProperty =
            DependencyProperty.Register("NormalSymbol", typeof(DataTemplate), typeof(ucComponent), new FrameworkPropertyMetadata(null));

        public DataTemplate RunSymbol
        {
            get { return (DataTemplate)GetValue(RunSymbolProperty); }
            set { SetValue(RunSymbolProperty, value); }
        }

        // Using a DependencyProperty as the backing store for navframe.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RunSymbolProperty =
            DependencyProperty.Register("RunSymbol", typeof(DataTemplate), typeof(ucComponent), new FrameworkPropertyMetadata(null));

        public DataTemplate FaultSymbol
        {
            get { return (DataTemplate)GetValue(FaultSymbolProperty); }
            set { SetValue(FaultSymbolProperty, value); }
        }

        // Using a DependencyProperty as the backing store for navframe.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FaultSymbolProperty =
            DependencyProperty.Register("FaultSymbol", typeof(DataTemplate), typeof(ucComponent), new FrameworkPropertyMetadata(null));

        public UserControl Faceplate
        {
            get { return (UserControl)GetValue(FaceplateProperty); }
            set { SetValue(FaceplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for navframe.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FaceplateProperty =
            DependencyProperty.Register("Faceplate", typeof(UserControl), typeof(ucComponent), new FrameworkPropertyMetadata(null));

        #endregion

        public ucComponentViewModel ViewModel { get; set; }
        public ucComponent()
        {
            InitializeComponent();
            this.DataContext = ViewModel = new ucComponentViewModel();
        }
    }
}
