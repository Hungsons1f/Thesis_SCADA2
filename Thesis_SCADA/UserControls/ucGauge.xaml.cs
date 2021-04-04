using System;
using System.Collections.Generic;
using System.Globalization;
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
    public partial class ucGauge : UserControl
    {
        #region DependencyProperty
        public float Data
        {
            get { return (float)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for navframe.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(float), typeof(ucGauge), new FrameworkPropertyMetadata((float)60));


        #endregion

        public ucGauge()
        {
            InitializeComponent();
        }
    }

    public class DataConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            float? data = (float)value;
            if (data == null) return null;
            return (int)(data * 280 / 100 - 140);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            float back = (float)value;
            return (back + 140) * 100 / 280;
        }
    }

}
