using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using Thesis_SCADA.Model;

namespace Thesis_SCADA.ViewModel
{
    public class TrendViewModel : BaseViewModel
    {
        #region Properties
        private DispatcherTimer timer;

        public delegate void Anonym();
        private ObservableCollection<ProcessData> model;
        public ObservableCollection<ProcessData> Model
        {
            get => model;
            set
            {
                model = value;
                if (model != null)
                {
                    Anonym update = async () =>
                    {
                        await Task.Run(() =>
                        {

                        });
                    };

                    update();
                }
            }
        }



        public string[] Labels { get; set; }
        private SeriesCollection s;
        public SeriesCollection S { get => s; set { s = value; } }


        #region Plot Object
        private ChartValues<double> turbineOutPress = new ChartValues<double>();
        public ChartValues<double> TurbineOutPress { get => turbineOutPress; set { turbineOutPress = value; OnPropertyChanged(); } }

        #endregion
        #endregion

        #region commands
        public ICommand LoadedCommand { get; set; }
        public ICommand UnloadedCommand { get; set; }


        #endregion

        public TrendViewModel()
        {
            S = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Motor 2",
                    Values = TurbineOutPress
                //    PointGeometry = DefaultGeometries.Triangle,
                //    PointGeometrySize = 15
                }
            };
            Labels = new[] { "Motor 1", "Motor 2" };

            //timer = new DispatcherTimer();
            //timer.Interval = TimeSpan.FromSeconds(1);
            //timer.Tick -= timer_Tick;
            //timer.Tick += timer_Tick;
            //timer.Start();

            Model = new ObservableCollection<ProcessData>(DataProvider.Ins.DB.ProcessData.OrderByDescending(p => p.Timestamp).Take(120));
            foreach (var item in Model)
            {
                TurbineOutPress.Add((double)item.TurbineL_Press);
            }
            GlobalVar.Ins.DatabaseUpdated -= OnTrendDatabaseUpdated;
            GlobalVar.Ins.DatabaseUpdated += OnTrendDatabaseUpdated;

            //LoadedCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
            //    timer.Start();
            //});

            //UnloadedCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
            //    timer.Stop();
            //});

        }

        private void OnTrendDatabaseUpdated(object sender, EventArgs e)
        {
            if (Model.Count > 120)
            {
                Model.RemoveAt(0);
                TurbineOutPress.RemoveAt(0);
            }
            Model.Add(GlobalVar.Ins.DbProcessdata);

            TurbineOutPress.Add((double)GlobalVar.Ins.DbProcessdata.TurbineL_Press);
        }

    //    private void timer_Tick(object sender, EventArgs e)
    //    {

    //    }
    //}
}
