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
    /// Interaction logic for ucInOut.xaml
    /// </summary>
    public partial class ucLight : UserControl
    {
        #region Dependency Properties
        public bool IsOn
        {
            get { return (bool)GetValue(IsOnProperty); }
            set { SetValue(IsOnProperty, value); }
        }

        // Using a DependencyProperty as the backing store for navframe.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsOnProperty =
            DependencyProperty.Register("IsOn", typeof(bool), typeof(ucLight), new FrameworkPropertyMetadata(false));

        public DataTemplate OffSymbol
        {
            get { return (DataTemplate)GetValue(OffSymbolProperty); }
            set { SetValue(OffSymbolProperty, value); }
        }

        // Using a DependencyProperty as the backing store for navframe.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OffSymbolProperty =
            DependencyProperty.Register("OffSymbol", typeof(DataTemplate), typeof(ucLight), new FrameworkPropertyMetadata(null));

        public DataTemplate OnSymbol
        {
            get { return (DataTemplate)GetValue(OnSymbolProperty); }
            set { SetValue(OnSymbolProperty, value); }
        }

        // Using a DependencyProperty as the backing store for navframe.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnSymbolProperty =
            DependencyProperty.Register("OnSymbol", typeof(DataTemplate), typeof(ucLight), new FrameworkPropertyMetadata(null));
        #endregion

        public ucLight()
        {
            InitializeComponent();
        }
    }
}
