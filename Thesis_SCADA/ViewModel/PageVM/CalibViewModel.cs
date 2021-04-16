using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Thesis_SCADA.Model;

namespace Thesis_SCADA.ViewModel
{
    public class CalibViewModel: BaseViewModel
    {
        #region Properties
        public delegate void Anonym();
        private Parameters model;
        public Parameters Model
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
                            CondensePumpPress = model?.CondensePumpPress;
                            SupplyPumpPress = model?.SupplyPumpPress;
                            CondenserTemp = model?.CondenserTemp;
                            LPHeaterTemp = model?.LPHeaterTemp;
                            DeaeratorTemp = model?.DeaeratorTemp;
                            HPHeaterTemp = model?.HPHeaterTemp;
                            TurbineSpeed = model?.TurbineSpeed;
                            BoilerTemp = model?.BoilerTemp;

                            MaxSpeed_CondensePump = (float)model?.MaxSpeed_CondensePump;
                            MaxSpeed_SupplyPump = (float)model?.MaxSpeed_SupplyPump;
                            MaxSpeed_CircularPump = (float)model?.MaxSpeed_CircularPump;
                            MaxSpeed_ForceFan1 = (float)model?.MaxSpeed_ForceFan1;
                            MaxSpeed_ForceFan2 = (float)model?.MaxSpeed_ForceFan2;
                            MaxSpeed_ForceFan3 = (float)model?.MaxSpeed_ForceFan3;
                            SampleTime = (float)model?.SampleTime;
                        });
                    };

                    update();
                }
            }
        }

        #region Parameters
        private aS_PidParameter condensePumpPress = new aS_PidParameter();
        public aS_PidParameter CondensePumpPress { get => condensePumpPress; set { condensePumpPress = value; OnPropertyChanged(); } }

        private aS_PidParameter supplyPumpPress = new aS_PidParameter();
        public aS_PidParameter SupplyPumpPress { get => supplyPumpPress; set { supplyPumpPress = value; OnPropertyChanged(); } }

        private aS_PidParameter condenserTemp = new aS_PidParameter();
        public aS_PidParameter CondenserTemp { get => condenserTemp; set { condenserTemp = value; OnPropertyChanged(); } }

        private aS_PidParameter lPHeaterTemp = new aS_PidParameter();
        public aS_PidParameter LPHeaterTemp { get => lPHeaterTemp; set { lPHeaterTemp = value; OnPropertyChanged(); } }

        private aS_PidParameter deaeratorTemp = new aS_PidParameter();
        public aS_PidParameter DeaeratorTemp { get => deaeratorTemp; set { deaeratorTemp = value; OnPropertyChanged(); } }

        private aS_PidParameter hPHeaterTemp = new aS_PidParameter();
        public aS_PidParameter HPHeaterTemp { get => hPHeaterTemp; set { hPHeaterTemp = value; OnPropertyChanged(); } }

        private aS_PidParameter turbineSpeed = new aS_PidParameter();
        public aS_PidParameter TurbineSpeed { get => turbineSpeed; set { turbineSpeed = value; OnPropertyChanged(); } }

        private aS_PidParameter boilerTemp = new aS_PidParameter();
        public aS_PidParameter BoilerTemp { get => boilerTemp; set { boilerTemp = value; OnPropertyChanged(); } }

        private float maxSpeed_CondensePump = 0;
        public float MaxSpeed_CondensePump { get => maxSpeed_CondensePump; set { maxSpeed_CondensePump = value; OnPropertyChanged(); } }

        private float maxSpeed_SupplyPump = 0;
        public float MaxSpeed_SupplyPump { get => maxSpeed_SupplyPump; set { maxSpeed_SupplyPump = value; OnPropertyChanged(); } }

        private float maxSpeed_CircularPump = 0;
        public float MaxSpeed_CircularPump { get => maxSpeed_CircularPump; set { maxSpeed_CircularPump = value; OnPropertyChanged(); } }

        private float maxSpeed_ForceFan1 = 0;
        public float MaxSpeed_ForceFan1 { get => maxSpeed_ForceFan1; set { maxSpeed_ForceFan1 = value; OnPropertyChanged(); } }

        private float maxSpeed_ForceFan2 = 0;
        public float MaxSpeed_ForceFan2 { get => maxSpeed_ForceFan2; set { maxSpeed_ForceFan2 = value; OnPropertyChanged(); } }

        private float maxSpeed_ForceFan3 = 0;
        public float MaxSpeed_ForceFan3 { get => maxSpeed_ForceFan3; set { maxSpeed_ForceFan3 = value; OnPropertyChanged(); } }

        private float sampleTime = 0;
        public float SampleTime { get => sampleTime; set { sampleTime = value; OnPropertyChanged(); } }

        #endregion
        #endregion

        #region commands
        public ICommand LoadedCommand { get; set; }
        public ICommand UnloadedCommand { get; set; }

        public ICommand SampleTimeCommand { get; set; }

        public ICommand MaxSpeedCondensePumpCommand { get; set; }
        public ICommand KpCondensePumpCommand { get; set; }
        public ICommand TiCondensePumpCommand { get; set; }
        public ICommand TdCondensePumpCommand { get; set; }
        public ICommand TnCondensePumpCommand { get; set; }

        public ICommand MaxSpeedSupplyPumpCommand { get; set; }
        public ICommand KpSupplyPumpCommand { get; set; }
        public ICommand TiSupplyPumpCommand { get; set; }
        public ICommand TdSupplyPumpCommand { get; set; }
        public ICommand TnSupplyPumpCommand { get; set; }

        public ICommand MaxSpeedCircularPumpCommand { get; set; }
        public ICommand KpCircularPumpCommand { get; set; }
        public ICommand TiCircularPumpCommand { get; set; }
        public ICommand TdCircularPumpCommand { get; set; }
        public ICommand TnCircularPumpCommand { get; set; }

        public ICommand MaxSpeedForceFan1Command { get; set; }
        public ICommand MaxSpeedForceFan2Command { get; set; }
        public ICommand MaxSpeedForceFan3Command { get; set; }
        public ICommand KpForceFanCommand { get; set; }
        public ICommand TiForceFanCommand { get; set; }
        public ICommand TdForceFanCommand { get; set; }
        public ICommand TnForceFanCommand { get; set; }

        public ICommand KpLPHValveCommand { get; set; }
        public ICommand TiLPHValveCommand { get; set; }
        public ICommand TdLPHValveCommand { get; set; }
        public ICommand TnLPHValveCommand { get; set; }

        public ICommand KpDeaerValveCommand { get; set; }
        public ICommand TiDeaerValveCommand { get; set; }
        public ICommand TdDeaerValveCommand { get; set; }
        public ICommand TnDeaerValveCommand { get; set; }

        public ICommand KpHPHValveCommand { get; set; }
        public ICommand TiHPHValveCommand { get; set; }
        public ICommand TdHPHValveCommand { get; set; }
        public ICommand TnHPHValveCommand { get; set; }

        public ICommand KpTurbineCommand { get; set; }
        public ICommand TiTurbineCommand { get; set; }
        public ICommand TdTurbineCommand { get; set; }
        public ICommand TnTurbineCommand { get; set; }

        #endregion

        public CalibViewModel()
        {

            Model = GlobalVar.Ins.Parameter;
            GlobalVar.Ins.DataChanged -= OnCalibModelChanged;
            GlobalVar.Ins.DataChanged += OnCalibModelChanged;

            LoadedCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
                //GlobalVar.Ins.DataChanged += OnModelChanged;
            });

            UnloadedCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
                //GlobalVar.Ins.DataChanged -= OnModelChanged; 
            });

            #region Parameter Commands
            SampleTimeCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "" ) return;
                GlobalVar.Ins.WriteData<float>("Interfacex.Parameters.SampleTime", float.Parse(p, CultureInfo.InvariantCulture));
            });

            MaxSpeedCondensePumpCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<float>("Interfacex.Parameters.MaxSpeed_CondensePump", float.Parse(p, CultureInfo.InvariantCulture));
            });

            KpCondensePumpCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<double>("Interfacex.Parameters.PID_CondensePump_Press.Kp", double.Parse(p, CultureInfo.InvariantCulture));
            });

            TiCondensePumpCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<double>("Interfacex.Parameters.PID_CondensePump_Press.Ti", double.Parse(p, CultureInfo.InvariantCulture));
            });

            TdCondensePumpCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<double>("Interfacex.Parameters.PID_CondensePump_Press.Td", double.Parse(p, CultureInfo.InvariantCulture));
            });

            TnCondensePumpCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<double>("Interfacex.Parameters.PID_CondensePump_Press.Tn", double.Parse(p, CultureInfo.InvariantCulture));
            });

            MaxSpeedSupplyPumpCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<double>("Interfacex.Parameters.MaxSpeed_SupplyPump", double.Parse(p, CultureInfo.InvariantCulture));
            });

            KpSupplyPumpCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<double>("Interfacex.Parameters.PID_SupplyPump_Press.Kp", double.Parse(p, CultureInfo.InvariantCulture));
            });

            TiSupplyPumpCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<double>("Interfacex.Parameters.PID_SupplyPump_Press.Ti", double.Parse(p, CultureInfo.InvariantCulture));
            });

            TdSupplyPumpCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<double>("Interfacex.Parameters.PID_SupplyPump_Press.Td", double.Parse(p, CultureInfo.InvariantCulture));
            });

            TnSupplyPumpCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<double>("Interfacex.Parameters.PID_SupplyPump_Press.Tn", double.Parse(p, CultureInfo.InvariantCulture));
            });

            MaxSpeedCircularPumpCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<double>("Interfacex.Parameters.MaxSpeed_CircularPump", double.Parse(p, CultureInfo.InvariantCulture));
            });

            KpCircularPumpCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<double>("Interfacex.Parameters.PID_Condenser_Temp.Kp", double.Parse(p, CultureInfo.InvariantCulture));
            });

            TiCircularPumpCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<double>("Interfacex.Parameters.PID_Condenser_Temp.Ti", double.Parse(p, CultureInfo.InvariantCulture));
            });

            TdCircularPumpCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<double>("Interfacex.Parameters.PID_Condenser_Temp.Td", double.Parse(p, CultureInfo.InvariantCulture));
            });

            TnCircularPumpCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<double>("Interfacex.Parameters.PID_Condenser_Temp.Tn", double.Parse(p, CultureInfo.InvariantCulture));
            });

            MaxSpeedForceFan1Command = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<double>("Interfacex.Parameters.MaxSpeed_ForceFan1", double.Parse(p, CultureInfo.InvariantCulture));
            });

            MaxSpeedForceFan2Command = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<double>("Interfacex.Parameters.MaxSpeed_ForceFan2", double.Parse(p, CultureInfo.InvariantCulture));
            });

            MaxSpeedForceFan3Command = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<double>("Interfacex.Parameters.MaxSpeed_ForceFan3", double.Parse(p, CultureInfo.InvariantCulture));
            });

            KpForceFanCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<double>("Interfacex.Parameters.PID_Boiler_Temp.Kp", double.Parse(p, CultureInfo.InvariantCulture));
            });

            TiForceFanCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<double>("Interfacex.Parameters.PID_Boiler_Temp.Ti", double.Parse(p, CultureInfo.InvariantCulture));
            });

            TdForceFanCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<double>("Interfacex.Parameters.PID_Boiler_Temp.Td", double.Parse(p, CultureInfo.InvariantCulture));
            });

            TnForceFanCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<double>("Interfacex.Parameters.PID_Boiler_Temp.Tn", double.Parse(p, CultureInfo.InvariantCulture));
            });

            KpLPHValveCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<double>("Interfacex.Parameters.PID_LPHeater_Temp.Kp", double.Parse(p, CultureInfo.InvariantCulture));
            });

            TiLPHValveCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<double>("Interfacex.Parameters.PID_LPHeater_Temp.Ti", double.Parse(p, CultureInfo.InvariantCulture));
            });

            TdLPHValveCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<double>("Interfacex.Parameters.PID_LPHeater_Temp.Td", double.Parse(p, CultureInfo.InvariantCulture));
            });

            TnLPHValveCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<double>("Interfacex.Parameters.PID_LPHeater_Temp.Tn", double.Parse(p, CultureInfo.InvariantCulture));
            });

            KpDeaerValveCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<double>("Interfacex.Parameters.PID_AirEjector_Temp.Kp", double.Parse(p, CultureInfo.InvariantCulture));
            });

            TiDeaerValveCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<double>("Interfacex.Parameters.PID_AirEjector_Temp.Ti", double.Parse(p, CultureInfo.InvariantCulture));
            });

            TdDeaerValveCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<double>("Interfacex.Parameters.PID_AirEjector_Temp.Td", double.Parse(p, CultureInfo.InvariantCulture));
            });

            TnDeaerValveCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<double>("Interfacex.Parameters.PID_AirEjector_Temp.Tn", double.Parse(p, CultureInfo.InvariantCulture));
            });

            KpHPHValveCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<double>("Interfacex.Parameters.PID_HPHeater_Temp.Kp", double.Parse(p, CultureInfo.InvariantCulture));
            });

            TiHPHValveCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<double>("Interfacex.Parameters.PID_HPHeater_Temp.Ti", double.Parse(p, CultureInfo.InvariantCulture));
            });

            TdHPHValveCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<double>("Interfacex.Parameters.PID_HPHeater_Temp.Td", double.Parse(p, CultureInfo.InvariantCulture));
            });

            TnHPHValveCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<double>("Interfacex.Parameters.PID_HPHeater_Temp.Tn", double.Parse(p, CultureInfo.InvariantCulture));
            });

            KpTurbineCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<double>("Interfacex.Parameters.PID_Turbine_Speed.Kp", double.Parse(p, CultureInfo.InvariantCulture));
            });

            TiTurbineCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<double>("Interfacex.Parameters.PID_Turbine_Speed.Ti", double.Parse(p, CultureInfo.InvariantCulture));
            });

            TdTurbineCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<double>("Interfacex.Parameters.PID_Turbine_Speed.Td", double.Parse(p, CultureInfo.InvariantCulture));
            });

            TnTurbineCommand = new RelayCommand<string>((p) => { return true; }, (p) => {
                if (p == null || p == "") return;
                GlobalVar.Ins.WriteData<double>("Interfacex.Parameters.PID_Turbine_Speed.Tn", double.Parse(p, CultureInfo.InvariantCulture));
            });

            #endregion
        }

        private async void OnCalibModelChanged(object sender, EventArgs e)
        {
            await Task.Run(() => { Model = GlobalVar.Ins.Parameter; });
        }


    }
}
