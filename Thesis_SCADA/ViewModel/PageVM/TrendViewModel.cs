using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
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
        private bool updated;
        public bool Updated { get => updated; set => updated = value; }

        #region Plot Model
        private PlotModel furTempTrend;
        public PlotModel FurTempTrend { get => furTempTrend; set { furTempTrend = value; OnPropertyChanged(); } }

        private PlotModel vesTempTrend;
        public PlotModel VesTempTrend { get => vesTempTrend; set { vesTempTrend = value; OnPropertyChanged(); } }

        private PlotModel vesPressTrend;
        public PlotModel VesPressTrend { get => vesPressTrend; set { vesPressTrend = value; OnPropertyChanged(); } }

        private PlotModel pumpFlowTrend;
        public PlotModel PumpFlowTrend { get => pumpFlowTrend; set { pumpFlowTrend = value; OnPropertyChanged(); } }

        private PlotModel pumpPressTrend;
        public PlotModel PumpPressTrend { get => pumpPressTrend; set { pumpPressTrend = value; OnPropertyChanged(); } }

        private PlotModel turPressTrend;
        public PlotModel TurPressTrend { get => turPressTrend; set { turPressTrend = value; OnPropertyChanged(); } }

        private PlotModel turTempTrend;
        public PlotModel TurTempTrend { get => turTempTrend; set { turTempTrend = value; OnPropertyChanged(); } }

        private PlotModel turSpeedTrend;
        public PlotModel TurSpeedTrend { get => turSpeedTrend; set { turSpeedTrend = value; OnPropertyChanged(); } }

        private PlotModel conTempTrend;
        public PlotModel ConTempTrend { get => conTempTrend; set { conTempTrend = value; OnPropertyChanged(); } }

        private PlotModel cirFlowTrend;
        public PlotModel CirFlowTrend { get => cirFlowTrend; set { cirFlowTrend = value; OnPropertyChanged(); } }

        #endregion

        #region Data Series
        private LineSeries furTemp = new LineSeries() { };
        public LineSeries FurTemp { get => furTemp; set { furTemp = value;  } }

        private LineSeries lPHeaterTemp = new LineSeries() { Title = "GNHA"};
        public LineSeries LPHeaterTemp { get => lPHeaterTemp; set { lPHeaterTemp = value; } }
        private LineSeries deaeratorTemp = new LineSeries() { Title = "BKK" };
        public LineSeries DeaeratorTemp { get => deaeratorTemp; set { deaeratorTemp = value; } }
        private LineSeries hPHeaterTemp = new LineSeries() { Title = "GNCA" };
        public LineSeries HPHeaterTemp { get => hPHeaterTemp; set { hPHeaterTemp = value; } }

        private LineSeries lPHeaterPress = new LineSeries() { Title = "GNHA" };
        public LineSeries LPHeaterPress { get => lPHeaterPress; set { lPHeaterPress = value; } }
        private LineSeries deaeratorPress = new LineSeries() { Title = "BKK" };
        public LineSeries DeaeratorPress { get => deaeratorPress; set { deaeratorPress = value; } }
        private LineSeries hPHeaterPress = new LineSeries() { Title = "GNCA" };
        public LineSeries HPHeaterPress { get => hPHeaterPress; set { hPHeaterPress = value; } }

        private LineSeries conPumpPress = new LineSeries() { Title = "BNgưng" };
        public LineSeries ConPumpPress { get => conPumpPress; set { conPumpPress = value; } }
        private LineSeries supPumpPress = new LineSeries() { Title = "BCấp" };
        public LineSeries SupPumpPress { get => supPumpPress; set { supPumpPress = value; } }

        private LineSeries conPumpFlow = new LineSeries() { Title = "BNgưng" };
        public LineSeries ConPumpFlow { get => conPumpFlow; set { conPumpFlow = value; } }
        private LineSeries supPumpFlow = new LineSeries() { Title = "BCấp" };
        public LineSeries SupPumpFlow { get => supPumpFlow; set { supPumpFlow = value; } }

        private LineSeries boiTemp = new LineSeries() { Title = "Lò hơi" };
        public LineSeries BoiTemp { get => boiTemp; set { boiTemp = value; } }
        private LineSeries turHTemp = new LineSeries() { Title = "Tuabin CA" };
        public LineSeries TurHTemp { get => turHTemp; set { turHTemp = value; } }
        private LineSeries turITemp = new LineSeries() { Title = "Tuabin TA" };
        public LineSeries TurITemp { get => turITemp; set { turITemp = value; } }
        private LineSeries turLTemp = new LineSeries() { Title = "Tuabin HA" };
        public LineSeries TurLTemp { get => turLTemp; set { turLTemp = value; } }

        private LineSeries boiPress = new LineSeries() { Title = "Lò hơi" };
        public LineSeries BoiPress { get => boiPress; set { boiPress = value; } }
        private LineSeries turHPress = new LineSeries() { Title = "Tuabin CA" };
        public LineSeries TurHPress { get => turHPress; set { turHPress = value; } }
        private LineSeries turIPress = new LineSeries() { Title = "Tuabin TA" };
        public LineSeries TurIPress { get => turIPress; set { turIPress = value; } }
        private LineSeries turLPress = new LineSeries() { Title = "Tuabin HA" };
        public LineSeries TurLPress { get => turLPress; set { turLPress = value; } }

        private LineSeries turSpeed = new LineSeries() { Color = OxyColor.FromAColor(255, OxyColors.Red)};
        public LineSeries TurSpeed { get => turSpeed; set { turSpeed = value; } }

        private LineSeries conTemp = new LineSeries() { Color = OxyColor.FromAColor(255, OxyColors.DarkMagenta)};
        public LineSeries ConTemp { get => conTemp; set { conTemp = value; } }

        private LineSeries cirFlow = new LineSeries() { Color = OxyColor.FromAColor(255, OxyColors.OrangeRed)};
        public LineSeries CirFlow { get => cirFlow; set { cirFlow = value; } }

        #endregion
        #endregion

        #region commands
        public ICommand LoadedCommand { get; set; }
        public ICommand UnloadedCommand { get; set; }


        #endregion

        public TrendViewModel()
        {
            #region Create Plot Model
            FurTempTrend = CreateNewPlotModel();
            FurTempTrend.Series.Add(FurTemp);

            VesTempTrend = CreateNewPlotModel();
            VesTempTrend.Series.Add(LPHeaterTemp);
            VesTempTrend.Series.Add(DeaeratorTemp);
            VesTempTrend.Series.Add(HPHeaterTemp);

            VesPressTrend = CreateNewPlotModel();
            VesPressTrend.Series.Add(LPHeaterPress);
            VesPressTrend.Series.Add(DeaeratorPress);
            VesPressTrend.Series.Add(HPHeaterPress);

            PumpFlowTrend = CreateNewPlotModel();
            PumpFlowTrend.Series.Add(ConPumpFlow);
            PumpFlowTrend.Series.Add(SupPumpFlow);

            PumpPressTrend = CreateNewPlotModel();
            PumpPressTrend.Series.Add(ConPumpPress);
            PumpPressTrend.Series.Add(SupPumpPress);

            TurTempTrend = CreateNewPlotModel();
            TurTempTrend.Series.Add(BoiTemp);
            TurTempTrend.Series.Add(TurHTemp);
            TurTempTrend.Series.Add(TurITemp);
            TurTempTrend.Series.Add(TurLTemp);

            TurPressTrend = CreateNewPlotModel();
            TurPressTrend.Series.Add(BoiPress);
            TurPressTrend.Series.Add(TurHPress);
            TurPressTrend.Series.Add(TurIPress);
            TurPressTrend.Series.Add(TurLPress);

            TurSpeedTrend = CreateNewPlotModel();
            TurSpeedTrend.Series.Add(TurSpeed);

            ConTempTrend = CreateNewPlotModel();
            ConTempTrend.Series.Add(ConTemp);

            CirFlowTrend = CreateNewPlotModel();
            CirFlowTrend.Series.Add(CirFlow);

            #endregion

            #region Initialize Data
            var last = DateTime.Now - TimeSpan.FromMinutes(2);
            var init = new ObservableCollection<ProcessData>(DataProvider.Ins.DB.ProcessData.Where(x => x.Timestamp > last).OrderByDescending(p => p.Timestamp) );
            for (var i = init.Count - 1; i >= 0; i--)
            {
                FurTemp.Points.Add(new DataPoint(DateTimeAxis.ToDouble(init[i].Timestamp), (double)init[i].TurbineL_Press));

                LPHeaterTemp.Points.Add(new DataPoint(DateTimeAxis.ToDouble(init[i].Timestamp), (double)init[i].HLPHeater_Temp));
                DeaeratorTemp.Points.Add(new DataPoint(DateTimeAxis.ToDouble(init[i].Timestamp), (double)init[i].HDeaerator_Temp));
                HPHeaterTemp.Points.Add(new DataPoint(DateTimeAxis.ToDouble(init[i].Timestamp), (double)init[i].HHPHeater_Temp));

                LPHeaterPress.Points.Add(new DataPoint(DateTimeAxis.ToDouble(init[i].Timestamp), (double)init[i].HLPHeater_Press));
                DeaeratorPress.Points.Add(new DataPoint(DateTimeAxis.ToDouble(init[i].Timestamp), (double)init[i].HDeaerator_Press));
                HPHeaterPress.Points.Add(new DataPoint(DateTimeAxis.ToDouble(init[i].Timestamp), (double)init[i].HHPHeater_Press));

                ConPumpFlow.Points.Add(new DataPoint(DateTimeAxis.ToDouble(init[i].Timestamp), (double)init[i].PCondense_Flow));
                SupPumpFlow.Points.Add(new DataPoint(DateTimeAxis.ToDouble(init[i].Timestamp), (double)init[i].PSupply_Flow));

                ConPumpPress.Points.Add(new DataPoint(DateTimeAxis.ToDouble(init[i].Timestamp), (double)init[i].PCondense_Press));
                SupPumpPress.Points.Add(new DataPoint(DateTimeAxis.ToDouble(init[i].Timestamp), (double)init[i].PSupply_Press));

                BoiTemp.Points.Add(new DataPoint(DateTimeAxis.ToDouble(init[i].Timestamp), (double)init[i].HBoiler_Temp));
                TurHTemp.Points.Add(new DataPoint(DateTimeAxis.ToDouble(init[i].Timestamp), (double)init[i].TurbineH_Temp));
                TurITemp.Points.Add(new DataPoint(DateTimeAxis.ToDouble(init[i].Timestamp), (double)init[i].TurbineI_Temp));
                TurLTemp.Points.Add(new DataPoint(DateTimeAxis.ToDouble(init[i].Timestamp), (double)init[i].TurbineL_Temp));

                BoiPress.Points.Add(new DataPoint(DateTimeAxis.ToDouble(init[i].Timestamp), (double)init[i].HBoiler_Press));
                TurHPress.Points.Add(new DataPoint(DateTimeAxis.ToDouble(init[i].Timestamp), (double)init[i].TurbineH_Press));
                TurIPress.Points.Add(new DataPoint(DateTimeAxis.ToDouble(init[i].Timestamp), (double)init[i].TurbineI_Press));
                TurLPress.Points.Add(new DataPoint(DateTimeAxis.ToDouble(init[i].Timestamp), (double)init[i].TurbineL_Press));

                TurSpeed.Points.Add(new DataPoint(DateTimeAxis.ToDouble(init[i].Timestamp), (double)init[i].Turbine_Freq));
                ConTemp.Points.Add(new DataPoint(DateTimeAxis.ToDouble(init[i].Timestamp), (double)init[i].HCondenser_Temp));
                CirFlow.Points.Add(new DataPoint(DateTimeAxis.ToDouble(init[i].Timestamp), (double)init[i].PCircular_Flow));
            }
            #endregion

            GlobalVar.Ins.DatabaseUpdated -= OnTrendDatabaseUpdated;
            GlobalVar.Ins.DatabaseUpdated += OnTrendDatabaseUpdated;

            //LoadedCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
            //    timer.Start();
            //});

            //UnloadedCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
            //    timer.Stop();
            //});
        }

        private PlotModel CreateNewPlotModel()
        {
            PlotModel plotModel = new PlotModel();
            plotModel.LegendOrientation = LegendOrientation.Vertical;
            plotModel.LegendPlacement = LegendPlacement.Outside;
            plotModel.LegendPosition = LegendPosition.RightMiddle;
            plotModel.LegendTextColor = OxyColor.FromUInt32(0xff808080);
            plotModel.PlotAreaBorderColor = OxyColor.FromUInt32(0x00000000);

            DateTimeAxis dateAxis = new DateTimeAxis()
            {
                Title = "Thời gian",
                Position = OxyPlot.Axes.AxisPosition.Bottom,
                StringFormat = "dd/MM/yy HH:mm:ss",
                AxislineStyle = LineStyle.Solid,
                AxislineColor = OxyColor.FromUInt32(0xff808080),
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                MajorGridlineColor = OxyColor.FromUInt32(0x77808080),
                MinorGridlineColor = OxyColor.FromUInt32(0x77808080),
                TextColor = OxyColor.FromUInt32(0xff808080),
                TitleColor = OxyColor.FromUInt32(0xff808080),
                IntervalLength = 100,
            };
            LinearAxis valueAxis = new LinearAxis()
            {
                Title = "Giá trị",
                Position = OxyPlot.Axes.AxisPosition.Left,
                StartPosition = 0,
                AxislineStyle = LineStyle.Solid,
                AxislineColor = OxyColor.FromUInt32(0xff808080),
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                MajorGridlineColor = OxyColor.FromUInt32(0x77808080),
                MinorGridlineColor = OxyColor.FromUInt32(0x77808080),
                TextColor = OxyColor.FromUInt32(0xff808080),
                TitleColor = OxyColor.FromUInt32(0xff808080),
            };
            plotModel.Axes.Add(dateAxis);
            plotModel.Axes.Add(valueAxis);
            return plotModel;
        }

        private async void OnTrendDatabaseUpdated(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                if (FurTemp.Points.Count > 120)
                {
                    FurTemp.Points.RemoveAt(0);

                    LPHeaterTemp.Points.RemoveAt(0);
                    DeaeratorTemp.Points.RemoveAt(0);
                    HPHeaterTemp.Points.RemoveAt(0);

                    LPHeaterPress.Points.RemoveAt(0);
                    DeaeratorPress.Points.RemoveAt(0);
                    HPHeaterPress.Points.RemoveAt(0);

                    ConPumpFlow.Points.RemoveAt(0);
                    SupPumpFlow.Points.RemoveAt(0);

                    ConPumpPress.Points.RemoveAt(0);
                    SupPumpPress.Points.RemoveAt(0);

                    BoiTemp.Points.RemoveAt(0);
                    TurHTemp.Points.RemoveAt(0);
                    TurITemp.Points.RemoveAt(0);
                    TurLTemp.Points.RemoveAt(0);

                    BoiPress.Points.RemoveAt(0);
                    TurHPress.Points.RemoveAt(0);
                    TurIPress.Points.RemoveAt(0);
                    TurLPress.Points.RemoveAt(0);

                    TurSpeed.Points.RemoveAt(0);
                    ConTemp.Points.RemoveAt(0);
                    CirFlow.Points.RemoveAt(0);
                }

                FurTemp.Points.Add(new DataPoint(DateTimeAxis.ToDouble(GlobalVar.Ins.DbProcessdata.Timestamp), (double)GlobalVar.Ins.DbProcessdata.Furnace_Temp));

                LPHeaterTemp.Points.Add(new DataPoint(DateTimeAxis.ToDouble(GlobalVar.Ins.DbProcessdata.Timestamp), (double)GlobalVar.Ins.DbProcessdata.HLPHeater_Temp));
                DeaeratorTemp.Points.Add(new DataPoint(DateTimeAxis.ToDouble(GlobalVar.Ins.DbProcessdata.Timestamp), (double)GlobalVar.Ins.DbProcessdata.HDeaerator_Temp));
                HPHeaterTemp.Points.Add(new DataPoint(DateTimeAxis.ToDouble(GlobalVar.Ins.DbProcessdata.Timestamp), (double)GlobalVar.Ins.DbProcessdata.HHPHeater_Temp));

                LPHeaterPress.Points.Add(new DataPoint(DateTimeAxis.ToDouble(GlobalVar.Ins.DbProcessdata.Timestamp), (double)GlobalVar.Ins.DbProcessdata.HLPHeater_Press));
                HPHeaterPress.Points.Add(new DataPoint(DateTimeAxis.ToDouble(GlobalVar.Ins.DbProcessdata.Timestamp), (double)GlobalVar.Ins.DbProcessdata.HDeaerator_Press));
                DeaeratorPress.Points.Add(new DataPoint(DateTimeAxis.ToDouble(GlobalVar.Ins.DbProcessdata.Timestamp), (double)GlobalVar.Ins.DbProcessdata.HHPHeater_Press));

                ConPumpFlow.Points.Add(new DataPoint(DateTimeAxis.ToDouble(GlobalVar.Ins.DbProcessdata.Timestamp), (double)GlobalVar.Ins.DbProcessdata.PCondense_Flow));
                SupPumpFlow.Points.Add(new DataPoint(DateTimeAxis.ToDouble(GlobalVar.Ins.DbProcessdata.Timestamp), (double)GlobalVar.Ins.DbProcessdata.PSupply_Flow));

                ConPumpPress.Points.Add(new DataPoint(DateTimeAxis.ToDouble(GlobalVar.Ins.DbProcessdata.Timestamp), (double)GlobalVar.Ins.DbProcessdata.PCondense_Press));
                SupPumpPress.Points.Add(new DataPoint(DateTimeAxis.ToDouble(GlobalVar.Ins.DbProcessdata.Timestamp), (double)GlobalVar.Ins.DbProcessdata.PSupply_Press));

                BoiTemp.Points.Add(new DataPoint(DateTimeAxis.ToDouble(GlobalVar.Ins.DbProcessdata.Timestamp), (double)GlobalVar.Ins.DbProcessdata.HBoiler_Temp));
                TurHTemp.Points.Add(new DataPoint(DateTimeAxis.ToDouble(GlobalVar.Ins.DbProcessdata.Timestamp), (double)GlobalVar.Ins.DbProcessdata.TurbineH_Temp));
                TurITemp.Points.Add(new DataPoint(DateTimeAxis.ToDouble(GlobalVar.Ins.DbProcessdata.Timestamp), (double)GlobalVar.Ins.DbProcessdata.TurbineI_Temp));
                TurLTemp.Points.Add(new DataPoint(DateTimeAxis.ToDouble(GlobalVar.Ins.DbProcessdata.Timestamp), (double)GlobalVar.Ins.DbProcessdata.TurbineL_Temp));

                BoiPress.Points.Add(new DataPoint(DateTimeAxis.ToDouble(GlobalVar.Ins.DbProcessdata.Timestamp), (double)GlobalVar.Ins.DbProcessdata.HBoiler_Press + 0));
                TurHPress.Points.Add(new DataPoint(DateTimeAxis.ToDouble(GlobalVar.Ins.DbProcessdata.Timestamp), (double)GlobalVar.Ins.DbProcessdata.TurbineH_Press));
                TurIPress.Points.Add(new DataPoint(DateTimeAxis.ToDouble(GlobalVar.Ins.DbProcessdata.Timestamp), (double)GlobalVar.Ins.DbProcessdata.TurbineI_Press));
                TurLPress.Points.Add(new DataPoint(DateTimeAxis.ToDouble(GlobalVar.Ins.DbProcessdata.Timestamp), (double)GlobalVar.Ins.DbProcessdata.TurbineL_Press));

                TurSpeed.Points.Add(new DataPoint(DateTimeAxis.ToDouble(GlobalVar.Ins.DbProcessdata.Timestamp), (double)GlobalVar.Ins.DbProcessdata.Turbine_Freq));
                ConTemp.Points.Add(new DataPoint(DateTimeAxis.ToDouble(GlobalVar.Ins.DbProcessdata.Timestamp), (double)GlobalVar.Ins.DbProcessdata.HCondenser_Temp));
                CirFlow.Points.Add(new DataPoint(DateTimeAxis.ToDouble(GlobalVar.Ins.DbProcessdata.Timestamp), (double)GlobalVar.Ins.DbProcessdata.PCircular_Flow));

                Updated = true;

                FurTempTrend.InvalidatePlot(true);
                VesPressTrend.InvalidatePlot(true);
                VesTempTrend.InvalidatePlot(true);
                PumpFlowTrend.InvalidatePlot(true);
                PumpPressTrend.InvalidatePlot(true);
                TurPressTrend.InvalidatePlot(true);
                TurTempTrend.InvalidatePlot(true);
                TurSpeedTrend.InvalidatePlot(true);
                ConTempTrend.InvalidatePlot(true);
                CirFlowTrend.InvalidatePlot(true);

            });
        }
    }
}
