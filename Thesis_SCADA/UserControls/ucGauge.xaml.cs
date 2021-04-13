using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
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
using Microsoft.Expression.Shapes;

namespace Thesis_SCADA.UserControls
{
    /// <summary>
    /// Interaction logic for ucComponent.xaml
    /// </summary>
    public partial class ucGauge : UserControl
    {
        #region DependencyProperty
        public double Data
        {
            get { return (double)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for navframe.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(double), typeof(ucGauge), new FrameworkPropertyMetadata((double)1));

        public double Max
        {
            get { return (double)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for navframe.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxProperty =
            DependencyProperty.Register("Max", typeof(double), typeof(ucGauge), new FrameworkPropertyMetadata((double)1));

        #endregion

        public ucGauge()
        {
            InitializeComponent();
        }
    }

    public class DataConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double? data = (double)values[0];
            double? max = (double)values[1];
            if (data == null || max == null || max == 0) return -140;
            if (data > max) return max;
            if (max != 0)
            {
                if (data <= 0) data = 0.1; 
                return (double)(data * 280 / max - 140);
            }
            else return -140;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            float back = (float)value;
            object[] a = new object[2];
            a[0] = (back + 140) * 100 / 280;
            a[1] = 100;
            return a;
        }
    }
}
