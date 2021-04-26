using LiveCharts;
using LiveCharts.Wpf;
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

namespace Thesis_SCADA.Pages
{
    /// <summary>
    /// Interaction logic for Trend.xaml
    /// </summary>
    public partial class Trend : Page
    {
        public TrendViewModel ViewModel { get; set; }
        public Trend()
        {
            DataContext = ViewModel = new TrendViewModel();
            CompositionTarget.Rendering += CompositionTargetRendering;

            InitializeComponent();
        }

        private async void CompositionTargetRendering(object sender, EventArgs e)
        {
            //if (ViewModel.Updated)
            //{
            //    await Task.Run(() =>
            //    {
            //        //FurTemp.InvalidatePlot();
            //        VesPress.InvalidatePlot();
            //        VesTemp.InvalidatePlot();
            //    }); 
            //    ViewModel.Updated = false;
            //}
        }
    }

}
