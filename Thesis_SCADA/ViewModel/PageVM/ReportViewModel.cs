using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Thesis_SCADA.Model;
using Ookii.Dialogs.Wpf;
using CsvHelper.Configuration;
using System.IO;
using CsvHelper;
using System.Windows;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Thesis_SCADA.ViewModel
{
    public class ReportViewModel : BaseViewModel
    {
        #region Properties
        private string folderPath;
        public string FolderPath { get => folderPath; set { folderPath = value; OnPropertyChanged(); } }

        private string folderPath2;
        public string FolderPath2 { get => folderPath2; set { folderPath2 = value; OnPropertyChanged(); } }

        private string folderPath3;
        public string FolderPath3 { get => folderPath3; set { folderPath3 = value; OnPropertyChanged(); } }

        private DateTime? sampleTime;
        public DateTime? SampleTime { get => sampleTime; set { sampleTime = value; OnPropertyChanged(); } }
        #endregion

        #region Commands
        public ICommand LoadedCommand { get; set; }
        public ICommand PathCommand { get; set; }
        public ICommand Path2Command { get; set; }
        public ICommand Path3Command { get; set; }
        public ICommand EventCSVCommand { get; set; }
        public ICommand EventPDFCommand { get; set; }
        public ICommand ProcessCSVCommand { get; set; }
        public ICommand ProcessPDFCommand { get; set; }
        public ICommand ComponentPDFCommand { get; set; }
        #endregion

        public ReportViewModel()
        {
            LoadedCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
            });

            PathCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                var dialog = new VistaFolderBrowserDialog();
                dialog.ShowDialog();

                if (dialog.SelectedPath != null)
                    FolderPath = dialog.SelectedPath;
            });

            Path2Command = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                var dialog = new VistaFolderBrowserDialog();
                dialog.ShowDialog();

                if (dialog.SelectedPath != null)
                    FolderPath2 = dialog.SelectedPath;
            });

            Path3Command = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                var dialog = new VistaFolderBrowserDialog();
                dialog.ShowDialog();

                if (dialog.SelectedPath != null)
                    FolderPath3 = dialog.SelectedPath;
            });

            EventCSVCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                var records = new List<ProcessEvent>(DataProvider.Ins.DB.ProcessEvent.OrderByDescending(x => x.Id));
                if (FolderPath == null) return;

                try
                {
                    var path = FolderPath + "\\EventReport_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + ".csv";
                    using (var writer = new StreamWriter(path))
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        csv.Context.RegisterClassMap<EventCSVMap>();
                        csv.WriteField("sep=,", false);
                        csv.NextRecord();
                        csv.WriteHeader<RecordFormat>();
                        csv.NextRecord();
                        int id = 1;

                        foreach (var item in records)
                        {
                            var line = new RecordFormat();
                            line = FormatInCSV(item);
                            line.Id = id++;

                            csv.WriteRecord(line);
                            csv.NextRecord();
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(), "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });

            EventPDFCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                var records = new List<ProcessEvent>(DataProvider.Ins.DB.ProcessEvent.OrderByDescending(x => x.Id));
                if (FolderPath == null) return;

                try
                {
                    var doc = new Document(PageSize.A4);
                    var path = "\\EventReport_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + ".pdf";
                    var allfonts = FontFactory.RegisterDirectory("C:\\WINDOWS\\Fonts");
                    var myfont30 = FontFactory.GetFont("Cambria", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 30);
                    var myfont12 = FontFactory.GetFont("Cambria", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 12);
                    var myfont8 = FontFactory.GetFont("Calibri", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 9);
                    var space = new Paragraph("") { SpacingBefore = 10f, SpacingAfter = 10f };

                    PdfWriter.GetInstance(doc, new FileStream(FolderPath + path, FileMode.Create));
                    doc.Open();

                    doc.Add(new Paragraph("BÁO CÁO SỰ KIỆN", myfont30) { Alignment = 1 });
                    doc.Add(new Paragraph("Ngày tạo: " + DateTime.Now.ToString(), myfont12) { Alignment = 1});
                    doc.Add(space);


                    var table = new PdfPTable(new[] { 0.3f, 0.8f, 1.1f, 1.9f, 1.5f, 0.8f, 0.8f, 0.8f}) { HorizontalAlignment = 0, WidthPercentage = 100, };
                    List<string> header = new List<string>() { "STT", "Loại", "Nhóm", "Nội dung", "Đối tượng", "Thời gian xảy ra", "Thời gian xác nhận", "Thời gian vô hiệu"};
                    header.ForEach(c => table.AddCell(new PdfPCell(new Phrase(c, myfont8))));

                    int id = 1;
                    foreach (var item in records)
                    {
                        var line = new RecordFormat();
                        line = FormatInPDF(item);
                        line.Id = id++;

                        table.AddCell(new PdfPCell(new Phrase(line.Id.ToString(), myfont8)));
                        table.AddCell(new PdfPCell(new Phrase(line.Type, myfont8)));
                        table.AddCell(new PdfPCell(new Phrase(line.Group, myfont8)));
                        table.AddCell(new PdfPCell(new Phrase(line.Content, myfont8)));
                        table.AddCell(new PdfPCell(new Phrase(line.Source, myfont8)));
                        table.AddCell(new PdfPCell(new Phrase(line.TimeRaised, myfont8)));
                        table.AddCell(new PdfPCell(new Phrase(line.TimeConfirmed, myfont8)));
                        table.AddCell(new PdfPCell(new Phrase(line.TimeCleared, myfont8)));
                    }
                    doc.Add(table);
                    doc.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(), "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });

            ProcessPDFCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (FolderPath2 == null) return;

                try
                {
                    var doc = new Document(PageSize.A4);
                    var path = "\\ProcessReport_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + ".pdf";
                    var allfonts = FontFactory.RegisterDirectory("C:\\WINDOWS\\Fonts");
                    var myfont30 = FontFactory.GetFont("Cambria", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 30);
                    var myfont14 = FontFactory.GetFont("Cambria", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 14, 1);
                    var myfont12 = FontFactory.GetFont("Calibri", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 12);
                    var space = new Paragraph("") { SpacingBefore = 10f, SpacingAfter = 10f };

                    PdfWriter.GetInstance(doc, new FileStream(FolderPath2 + path, FileMode.Create));
                    doc.Open();

                    doc.Add(new Paragraph("BÁO CÁO TRẠNG THÁI QUÁ TRÌNH", myfont30) { Alignment = 1 });
                    doc.Add(new Paragraph("Ngày tạo: " + DateTime.Now.ToString(), myfont12) { Alignment = 1 });
                    doc.Add(space);

                    doc.Add(new Paragraph("1. Bình gia nhiệt hạ áp", myfont14));
                    doc.Add(new Paragraph("- Nhiệt độ đầu vào (oC): " + GlobalVar.Ins.IpcData.Components.Condenser.Temperature.ToString("0.00"), myfont12) { IndentationLeft = 20f});
                    doc.Add(new Paragraph("- Áp suất đầu vào (bar): " + GlobalVar.Ins.IpcData.Components.LPHeater.InPressure.ToString("0.00"), myfont12) { IndentationLeft = 20f });
                    doc.Add(new Paragraph("- Lưu lượng đầu vào (m3/h): " + GlobalVar.Ins.IpcData.Components.LPHeater.InFlow.ToString("0.00"), myfont12) { IndentationLeft = 20f });
                    doc.Add(new Paragraph("- Nhiệt độ đầu ra (oC): " + GlobalVar.Ins.IpcData.Components.LPHeater.Temperature.ToString("0.00"), myfont12) { IndentationLeft = 20f });
                    doc.Add(new Paragraph("- Áp suất đầu ra (bar): " + GlobalVar.Ins.IpcData.Components.LPHeater.Pressure.ToString("0.00"), myfont12) { IndentationLeft = 20f });

                    doc.Add(new Paragraph("2. Bình khử khí", myfont14));
                    doc.Add(new Paragraph("- Nhiệt độ đầu vào (oC): " + GlobalVar.Ins.IpcData.Components.LPHeater.Temperature.ToString("0.00"), myfont12) { IndentationLeft = 20f });
                    doc.Add(new Paragraph("- Áp suất đầu vào (bar): " + GlobalVar.Ins.IpcData.Components.LPHeater.Pressure.ToString("0.00"), myfont12) { IndentationLeft = 20f });
                    doc.Add(new Paragraph("- Nhiệt độ đầu ra (oC): " + GlobalVar.Ins.IpcData.Components.Deaerator.Temperature.ToString("0.00"), myfont12) { IndentationLeft = 20f });
                    doc.Add(new Paragraph("- Áp suất đầu ra (bar): " + GlobalVar.Ins.IpcData.Components.Deaerator.Pressure.ToString("0.00"), myfont12) { IndentationLeft = 20f });

                    doc.Add(new Paragraph("3. Bình gia nhiệt cao áp", myfont14));
                    doc.Add(new Paragraph("- Nhiệt độ đầu vào (oC): " + GlobalVar.Ins.IpcData.Components.Deaerator.Temperature.ToString("0.00"), myfont12) { IndentationLeft = 20f });
                    doc.Add(new Paragraph("- Áp suất đầu vào (bar): " + GlobalVar.Ins.IpcData.Components.HPHeater.InPressure.ToString("0.00"), myfont12) { IndentationLeft = 20f });
                    doc.Add(new Paragraph("- Lưu lượng đầu vào (m3/h): " + GlobalVar.Ins.IpcData.Components.HPHeater.InFlow.ToString("0.00"), myfont12) { IndentationLeft = 20f });
                    doc.Add(new Paragraph("- Nhiệt độ đầu ra (oC): " + GlobalVar.Ins.IpcData.Components.HPHeater.Temperature.ToString("0.00"), myfont12) { IndentationLeft = 20f });
                    doc.Add(new Paragraph("- Áp suất đầu ra (bar): " + GlobalVar.Ins.IpcData.Components.HPHeater.Pressure.ToString("0.00"), myfont12) { IndentationLeft = 20f });

                    doc.Add(new Paragraph("4. Bình ngưng", myfont14));
                    doc.Add(new Paragraph("- Nhiệt độ đầu vào (oC): " + GlobalVar.Ins.IpcData.Components.Turbine.OutTemperature.ToString("0.00"), myfont12) { IndentationLeft = 20f });
                    doc.Add(new Paragraph("- Áp suất đầu vào (bar): " + GlobalVar.Ins.IpcData.Components.Turbine.OutPressure.ToString("0.00"), myfont12) { IndentationLeft = 20f });
                    doc.Add(new Paragraph("- Lưu lượng đầu vào (m3/h): " + GlobalVar.Ins.IpcData.Components.Condenser.InFlow.ToString("0.00"), myfont12) { IndentationLeft = 20f });
                    doc.Add(new Paragraph("- Áp suất đầu ra (bar): " + GlobalVar.Ins.IpcData.Components.Condenser.Pressure.ToString("0.00"), myfont12) { IndentationLeft = 20f });

                    doc.Add(new Paragraph("5. Lò hơi", myfont14));
                    doc.Add(new Paragraph("- Nhiệt độ đầu vào (oC): " + GlobalVar.Ins.IpcData.Components.HPHeater.Temperature.ToString("0.00"), myfont12) { IndentationLeft = 20f });
                    doc.Add(new Paragraph("- Áp suất đầu vào (bar): " + GlobalVar.Ins.IpcData.Components.HPHeater.Pressure.ToString("0.00"), myfont12) { IndentationLeft = 20f });
                    doc.Add(new Paragraph("- Nhiệt độ đầu ra (oC): " + GlobalVar.Ins.IpcData.Components.Boiler.Temperature.ToString("0.00"), myfont12) { IndentationLeft = 20f });
                    doc.Add(new Paragraph("- Áp suất đầu ra (bar): " + GlobalVar.Ins.IpcData.Components.Boiler.Pressure.ToString("0.00"), myfont12) { IndentationLeft = 20f });

                    doc.Add(new Paragraph("6. Tua bin", myfont14));
                    doc.Add(new Paragraph("- Nhiệt độ đầu vào (oC): " + GlobalVar.Ins.IpcData.Components.Boiler.Temperature.ToString("0.00"), myfont12) { IndentationLeft = 20f });
                    doc.Add(new Paragraph("- Áp suất đầu vào (bar): " + GlobalVar.Ins.IpcData.Components.Boiler.Pressure.ToString("0.00"), myfont12) { IndentationLeft = 20f });
                    doc.Add(new Paragraph("- Nhiệt độ tua bin cao áp (oC): " + GlobalVar.Ins.IpcData.Components.Turbine.HighTemperature.ToString("0.00"), myfont12) { IndentationLeft = 20f });
                    doc.Add(new Paragraph("- Áp suất tua bin cao áp (bar): " + GlobalVar.Ins.IpcData.Components.Turbine.HighPressure.ToString("0.00"), myfont12) { IndentationLeft = 20f });
                    doc.Add(new Paragraph("- Nhiệt độ tua bin trung áp (oC): " + GlobalVar.Ins.IpcData.Components.Turbine.ImmediateTemperature.ToString("0.00"), myfont12) { IndentationLeft = 20f });
                    doc.Add(new Paragraph("- Áp suất tua bin trung áp (bar): " + GlobalVar.Ins.IpcData.Components.Turbine.ImmediatePressure.ToString("0.00"), myfont12) { IndentationLeft = 20f });
                    doc.Add(new Paragraph("- Nhiệt độ đầu ra (oC): " + GlobalVar.Ins.IpcData.Components.Turbine.OutTemperature.ToString("0.00"), myfont12) { IndentationLeft = 20f });
                    doc.Add(new Paragraph("- Áp suất đầu ra (bar): " + GlobalVar.Ins.IpcData.Components.Turbine.OutPressure.ToString("0.00"), myfont12) { IndentationLeft = 20f });
                    doc.Add(new Paragraph("- Tốc độ quay tua bin (RPM): " + GlobalVar.Ins.IpcData.Components.Turbine.Rotation.ToString("0.00"), myfont12) { IndentationLeft = 20f });

                    doc.Add(new Paragraph("7. Lò đốt", myfont14));
                    doc.Add(new Paragraph("- Công suất đầu vào (%): " + GlobalVar.Ins.IpcData.Components.Furnace.InPercent.ToString("0.00"), myfont12) { IndentationLeft = 20f });
                    doc.Add(new Paragraph("- Nhiệt độ đầu ra (oC): " + GlobalVar.Ins.IpcData.Components.Furnace.Temperature.ToString("0.00"), myfont12) { IndentationLeft = 20f });

                    doc.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(), "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });

            ComponentPDFCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (FolderPath3 == null) return;

                try
                {
                    var doc = new Document(PageSize.A4);
                    var path = "\\ComponentReport_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + ".pdf";
                    var allfonts = FontFactory.RegisterDirectory("C:\\WINDOWS\\Fonts");
                    var myfont30 = FontFactory.GetFont("Cambria", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 30);
                    var myfont14 = FontFactory.GetFont("Cambria", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 14, 1);
                    var myfont12 = FontFactory.GetFont("Calibri", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 12);
                    var space = new Paragraph("") { SpacingBefore = 10f, SpacingAfter = 10f };

                    PdfWriter.GetInstance(doc, new FileStream(FolderPath3 + path, FileMode.Create));
                    doc.Open();

                    doc.Add(new Paragraph("BÁO CÁO TRẠNG THÁI THIẾT BỊ", myfont30) { Alignment = 1 });
                    doc.Add(new Paragraph("Ngày tạo: " + DateTime.Now.ToString(), myfont12) { Alignment = 1 });
                    doc.Add(space);

                    var table = new PdfPTable(7) { HorizontalAlignment = 0, WidthPercentage = 100, };
                    List<string> header = new List<string>() { "Tên thiết bị", "Chế độ", "Trạng thái", "Giá trị", "Thời gian chạy", "Thời gian chạy tích lũy", "Lỗi" };
                    header.ForEach(c => table.AddCell(new PdfPCell(new Phrase(c, myfont12))));

                    table.AddCell(new PdfPCell(new Phrase("Bơm ngưng", myfont12)));
                    FormatComponentInPDF(GlobalVar.Ins.IpcData.Components.CondensePump).ForEach(i => table.AddCell(new PdfPCell(new Phrase(i, myfont12))));
                    table.AddCell(new PdfPCell(new Phrase("Bơm cấp", myfont12)));
                    FormatComponentInPDF(GlobalVar.Ins.IpcData.Components.SupplyPump).ForEach(i => table.AddCell(new PdfPCell(new Phrase(i, myfont12))));
                    table.AddCell(new PdfPCell(new Phrase("Bơm tuần hoàn", myfont12)));
                    FormatComponentInPDF(GlobalVar.Ins.IpcData.Components.CircularPump).ForEach(i => table.AddCell(new PdfPCell(new Phrase(i, myfont12))));
                    table.AddCell(new PdfPCell(new Phrase("Bơm trung gian", myfont12)));
                    FormatComponentInPDF(GlobalVar.Ins.IpcData.Components.InterPump).ForEach(i => table.AddCell(new PdfPCell(new Phrase(i, myfont12))));
                    table.AddCell(new PdfPCell(new Phrase("Van GNHA", myfont12)));
                    FormatComponentInPDF(GlobalVar.Ins.IpcData.Components.LPHValve).ForEach(i => table.AddCell(new PdfPCell(new Phrase(i, myfont12))));
                    table.AddCell(new PdfPCell(new Phrase("Van BKK", myfont12)));
                    FormatComponentInPDF(GlobalVar.Ins.IpcData.Components.AirEjValve).ForEach(i => table.AddCell(new PdfPCell(new Phrase(i, myfont12))));
                    table.AddCell(new PdfPCell(new Phrase("Van GNCA", myfont12)));
                    FormatComponentInPDF(GlobalVar.Ins.IpcData.Components.HPHValve).ForEach(i => table.AddCell(new PdfPCell(new Phrase(i, myfont12))));
                    table.AddCell(new PdfPCell(new Phrase("Van Tua bin", myfont12)));
                    FormatComponentInPDF(GlobalVar.Ins.IpcData.Components.TurbineValve).ForEach(i => table.AddCell(new PdfPCell(new Phrase(i, myfont12))));
                    table.AddCell(new PdfPCell(new Phrase("Quạt cấp 1", myfont12)));
                    FormatComponentInPDF(GlobalVar.Ins.IpcData.Components.ForceFan1).ForEach(i => table.AddCell(new PdfPCell(new Phrase(i, myfont12))));
                    table.AddCell(new PdfPCell(new Phrase("Quạt cấp 2", myfont12)));
                    FormatComponentInPDF(GlobalVar.Ins.IpcData.Components.ForceFan2).ForEach(i => table.AddCell(new PdfPCell(new Phrase(i, myfont12))));
                    table.AddCell(new PdfPCell(new Phrase("Quạt cấp 3", myfont12)));
                    FormatComponentInPDF(GlobalVar.Ins.IpcData.Components.ForceFan3).ForEach(i => table.AddCell(new PdfPCell(new Phrase(i, myfont12))));

                    doc.Add(table);
                    doc.Close();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString(), "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });

            ProcessCSVCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SampleTime == null) return;
                if (FolderPath2 == null) return;
                var records = DataProvider.Ins.DB.ProcessData.Where(x => x.Timestamp > SampleTime).OrderByDescending(x => x.Id).ToList<ProcessData>();

                try
                {
                    var path = FolderPath2 + "\\ProcessReport_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + ".csv";
                    using (var writer = new StreamWriter(path))
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        csv.Context.RegisterClassMap<ProcessCSVMap>();
                        csv.WriteField("sep=,", false);
                        csv.NextRecord();
                        csv.WriteHeader<RecordProcessData>();
                        csv.NextRecord();
                        int id = 1;

                        foreach (var item in records)
                        {
                            var line = new RecordProcessData();
                            line.Id = id++;
                            line.Timestamp = item.Timestamp.ToString();

                            line.LPHeater_InTemp = ((double)item.HCondenser_Temp).ToString("0.00");
                            line.LPHeater_InPress = ((double)item.PCondense_Press).ToString("0.00");
                            line.LPHeater_InFlow = ((double)item.PCondense_Flow).ToString("0.00");
                            line.LPHeater_OutTemp = ((double)item.HLPHeater_Temp).ToString("0.00");
                            line.LPHeater_OutPress = ((double)item.HLPHeater_Press).ToString("0.00");

                            line.Deaerator_InTemp = ((double)item.HLPHeater_Temp).ToString("0.00");
                            line.Deaerator_InPress = ((double)item.HLPHeater_Press).ToString("0.00");
                            line.Deaerator_OutTemp = ((double)item.HDeaerator_Temp).ToString("0.00");
                            line.Deaerator_OutPress = ((double)item.HDeaerator_Press).ToString("0.00");

                            line.HPHeater_InTemp = ((double)item.HDeaerator_Temp).ToString("0.00");
                            line.HPHeater_InPress = ((double)item.PSupply_Press).ToString("0.00");
                            line.HPHeater_InFlow = ((double)item.PSupply_Flow).ToString("0.00");
                            line.HPHeater_OutTemp = ((double)item.HHPHeater_Temp).ToString("0.00");
                            line.HPHeater_OutPress = ((double)item.HHPHeater_Press).ToString("0.00");

                            line.Condenser_InTemp = ((double)item.TurbineL_Temp).ToString("0.00");
                            line.Condenser_InPress = ((double)item.TurbineL_Press).ToString("0.00");
                            line.Condenser_InFlow = ((double)item.PCondense_Flow).ToString("0.00");
                            line.Condenser_OutPress = ((double)item.PCondense_Press).ToString("0.00");

                            line.Boiler_InTemp = ((double)item.HHPHeater_Temp).ToString("0.00");
                            line.Boiler_InPress = ((double)item.HHPHeater_Press).ToString("0.00");
                            line.Boiler_OutTemp = ((double)item.HBoiler_Temp).ToString("0.00");
                            line.Boiler_OutPress = ((double)item.HBoiler_Press).ToString("0.00");

                            line.Turbine_InTemp = ((double)item.HBoiler_Temp).ToString("0.00");
                            line.Turbine_InPress = ((double)item.HBoiler_Press).ToString("0.00");
                            line.Turbine_HTemp = ((double)item.TurbineH_Temp).ToString("0.00");
                            line.Turbine_HPress = ((double)item.TurbineH_Press).ToString("0.00");
                            line.Turbine_ITemp = ((double)item.TurbineI_Temp).ToString("0.00");
                            line.Turbine_IPress = ((double)item.TurbineI_Press).ToString("0.00");
                            line.Turbine_OutTemp = ((double)item.TurbineL_Temp).ToString("0.00");
                            line.Turbine_OutPress = ((double)item.TurbineL_Press).ToString("0.00");
                            line.Turbine_Speed = ((double)item.Turbine_Freq).ToString("0.00");

                            line.Furnace_InPower = ((double)(item.Furnace_Temp*100/1688)).ToString("0.00");
                            line.Furnace_OutTemp = ((double)item.Furnace_Temp).ToString("0.00");

                            csv.WriteRecord(line);
                            csv.NextRecord();
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(), "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });

        }

        #region CSV Format
        public class RecordFormat
        {
            public int Id { get; set; }
            public string Type { get; set; }
            public string Group { get; set; }
            public string Content { get; set; }
            public string Source { get; set; }
            public string TimeRaised { get; set; }
            public string TimeConfirmed { get; set; }
            public string TimeCleared { get; set; }
        }

        public class EventCSVMap: ClassMap<RecordFormat>
        {
            public EventCSVMap()
            {
                Map(m => m.Id).Index(0).Name("Id");
                Map(m => m.Type).Index(1).Name("Type");
                Map(m => m.Group).Index(2).Name("Group");
                Map(m => m.Content).Index(3).Name("Content");
                Map(m => m.Source).Index(4).Name("Source");
                Map(m => m.TimeRaised).Index(5).Name("TimeRaised");
                Map(m => m.TimeConfirmed).Index(6).Name("TimeConfirmed");
                Map(m => m.TimeCleared).Index(7).Name("TimeCleared");
            }
        }

        public class RecordProcessData
        {
            public int Id { get; set; }
            public string Timestamp { get; set; }

            public string LPHeater_InTemp { get; set; }
            public string LPHeater_InPress { get; set; }
            public string LPHeater_InFlow { get; set; }
            public string LPHeater_OutTemp { get; set; }
            public string LPHeater_OutPress { get; set; }

            public string Deaerator_InTemp { get; set; }
            public string Deaerator_InPress { get; set; }
            public string Deaerator_OutTemp { get; set; }
            public string Deaerator_OutPress { get; set; }

            public string HPHeater_InTemp { get; set; }
            public string HPHeater_InPress { get; set; }
            public string HPHeater_InFlow { get; set; }
            public string HPHeater_OutTemp { get; set; }
            public string HPHeater_OutPress { get; set; }

            public string Condenser_InTemp { get; set; }
            public string Condenser_InPress { get; set; }
            public string Condenser_InFlow { get; set; }
            public string Condenser_OutPress { get; set; }

            public string Boiler_InTemp { get; set; }
            public string Boiler_InPress { get; set; }
            public string Boiler_OutTemp { get; set; }
            public string Boiler_OutPress { get; set; }

            public string Turbine_InTemp { get; set; }
            public string Turbine_InPress { get; set; }
            public string Turbine_HTemp { get; set; }
            public string Turbine_HPress { get; set; }
            public string Turbine_ITemp { get; set; }
            public string Turbine_IPress { get; set; }
            public string Turbine_OutTemp { get; set; }
            public string Turbine_OutPress { get; set; }
            public string Turbine_Speed { get; set; }

            public string Furnace_InPower { get; set; }
            public string Furnace_OutTemp { get; set; }
        }

        public class ProcessCSVMap: ClassMap<RecordProcessData>
        {
            public ProcessCSVMap()
            {
                Map(m => m.Id).Index(0).Name("Id");
                Map(m => m.Timestamp).Index(1).Name("Timestamp");

                Map(m => m.LPHeater_InTemp).Index(2).Name("LPHeater_InTemp");
                Map(m => m.LPHeater_InPress).Index(3).Name("LPHeater_InPress");
                Map(m => m.LPHeater_InFlow).Index(4).Name("LPHeater_InFlow");
                Map(m => m.LPHeater_OutTemp).Index(5).Name("LPHeater_OutTemp");
                Map(m => m.LPHeater_OutPress).Index(6).Name("LPHeater_OutPress");

                Map(m => m.Deaerator_InTemp).Index(7).Name("Deaerator_InTemp");
                Map(m => m.Deaerator_InPress).Index(8).Name("Deaerator_InPress");
                Map(m => m.Deaerator_OutTemp).Index(9).Name("Deaerator_OutTemp");
                Map(m => m.Deaerator_OutPress).Index(10).Name("Deaerator_OutPress");

                Map(m => m.HPHeater_InTemp).Index(11).Name("HPHeater_InTemp");
                Map(m => m.HPHeater_InPress).Index(12).Name("HPHeater_InPress");
                Map(m => m.HPHeater_InFlow).Index(13).Name("HPHeater_InFlow");
                Map(m => m.HPHeater_OutTemp).Index(14).Name("HPHeater_OutTemp");
                Map(m => m.HPHeater_OutPress).Index(15).Name("HPHeater_OutPress");

                Map(m => m.Condenser_InTemp).Index(16).Name("Condenser_InTemp");
                Map(m => m.Condenser_InPress).Index(17).Name("Condenser_InPress");
                Map(m => m.Condenser_InFlow).Index(18).Name("Condenser_InFlow");
                Map(m => m.Condenser_OutPress).Index(19).Name("Condenser_OutPress");

                Map(m => m.Boiler_InTemp).Index(20).Name("Boiler_InTemp");
                Map(m => m.Boiler_InPress).Index(21).Name("Boiler_InPress");
                Map(m => m.Boiler_OutTemp).Index(22).Name("Boiler_OutTemp");
                Map(m => m.Boiler_OutPress).Index(23).Name("Boiler_OutPress");

                Map(m => m.Turbine_InTemp).Index(24).Name("Turbine_InTemp");
                Map(m => m.Turbine_InPress).Index(25).Name("Turbine_InPress");
                Map(m => m.Turbine_HTemp).Index(26).Name("Turbine_HTemp");
                Map(m => m.Turbine_HPress).Index(27).Name("Turbine_HPress");
                Map(m => m.Turbine_ITemp).Index(28).Name("Turbine_ITemp");
                Map(m => m.Turbine_IPress).Index(29).Name("Turbine_IPress");
                Map(m => m.Turbine_OutTemp).Index(30).Name("Turbine_OutTemp");
                Map(m => m.Turbine_OutPress).Index(31).Name("Turbine_OutPress");
                Map(m => m.Turbine_Speed).Index(32).Name("Turbine_Speed");

                Map(m => m.Furnace_InPower).Index(33).Name("Furnace_InPower");
                Map(m => m.Furnace_OutTemp).Index(34).Name("Furnace_OutTemp");
            }
        }

        private RecordFormat FormatInCSV (ProcessEvent input)
        {
            var result = new RecordFormat();

            switch (input.SeverityLevel)
            {
                case 1:
                    result.Type = "Info";
                    break;
                case 2:
                    result.Type = "Alarm";
                    break;
                case 3:
                    result.Type = "Error";
                    break;
                default:
                    result.Type = "";
                    break;
            }

            result.Group = input.EventClass;

            switch (input.EventClass)
            {
                case "Component Event":
                    switch (input.EventID)
                    {
                        case 1:
                            result.Content = "Started";
                            break;
                        case 2:
                            result.Content = "Stopped";
                            break;
                        case 3:
                            result.Content = "Mode changed to Manual";
                            break;
                        case 4:
                            result.Content = "Mode changed to Automatic";
                            break;
                        case 5:
                            result.Content = "Mode changed to Service";
                            break;
                        case 6:
                            result.Content = "Error";
                            break;
                        case 7:
                            result.Content = "Resetted";
                            break;
                        default:
                            result.Content = "";
                            break;
                    }
                    break;
                case "Process Event":
                    switch (input.EventID)
                    {
                        case 1:
                            result.Content = "Initialized";
                            break;
                        case 2:
                            result.Content = "Enabled";
                            break;
                        case 3:
                            result.Content = "Starting";
                            break;
                        case 4:
                            result.Content = "Started";
                            break;
                        case 5:
                            result.Content = "Process is in Steady State";
                            break;
                        case 6:
                            result.Content = "Stopping";
                            break;
                        case 7:
                            result.Content = "Stopped";
                            break;
                        case 8:
                            result.Content = "Disabled";
                            break;
                        case 9:
                            result.Content = "Emergency Stop";
                            break;
                        case 10:
                            result.Content = "Error: Process is in Emergency Stop";
                            break;
                        case 11:
                            result.Content = "Resetted";
                            break;
                        default:
                            result.Content = "";
                            break;
                    }
                    break;
                case "Analog Alarm":
                    switch (input.EventID)
                    {
                        case 1:
                            result.Content = "HIHI";
                            break;
                        case 2:
                            result.Content = "HI";
                            break;
                        case 3:
                            result.Content = "LO";
                            break;
                        case 4:
                            result.Content = "LOLO";
                            break;
                        default:
                            result.Content = "";
                            break;
                    }
                    break;
                case "Digital Alarm":
                    switch (input.EventID)
                    {
                        case 1:
                            result.Content = "ON";
                            break;
                        case 2:
                            result.Content = "OFF";
                            break;
                        default:
                            result.Content = "";
                            break;
                    }
                    break;
                default:
                    result.Content = input.EventClass + ", ID = " + input.EventID;
                    break;
            }

            result.Source = input.SourceName;
            result.TimeRaised = (input.TimeRaised == new DateTime(1899, 12, 30, 00, 00, 00, 000) || input.TimeRaised == null) ? null : ((DateTime)input.TimeRaised).ToString(CultureInfo.InvariantCulture);
            result.TimeConfirmed = (input.TimeConfirmed == new DateTime(1899, 12, 30, 00, 00, 00, 000) || input.TimeConfirmed == null) ? null : ((DateTime)input.TimeConfirmed).ToString(CultureInfo.InvariantCulture);
            result.TimeCleared = (input.TimeCleared == new DateTime(1899, 12, 30, 00, 00, 00, 000) || input.TimeCleared == null) ? null : ((DateTime)input.TimeCleared).ToString(CultureInfo.InvariantCulture);
            return result;
        }
        #endregion

        #region PDF Format
        private RecordFormat FormatInPDF (ProcessEvent input)
        {
            var result = new RecordFormat();

            switch (input.SeverityLevel)
            {
                case 1:
                    result.Type = "Thông báo";
                    break;
                case 2:
                    result.Type = "Cảnh báo";
                    break;
                case 3:
                    result.Type = "Lỗi";
                    break;
                default:
                    result.Type = "";
                    break;
            }

            switch (input.EventClass)
            {
                case "Component Event":
                    result.Group = "Sự kiện thiết bị";
                    break;
                case "Process Event":
                    result.Group = "Sự kiện quá trình";
                    break;
                case "Analog Alarm":
                    result.Group = "Cảnh báo Analog";
                    break;
                case "Digital Alarm":
                    result.Group = "Cảnh báo Digital";
                    break;
                default:
                    result.Group = "";
                    break;
            }

            switch (input.EventClass)
            {
                case "Component Event":
                    switch (input.EventID)
                    {
                        case 1:
                            result.Content = "Đã bật";
                            break;
                        case 2:
                            result.Content = "Đã tắt";
                            break;
                        case 3:
                            result.Content = "Chuyển sang Chế độ bằng tay";
                            break;
                        case 4:
                            result.Content = "Chuyển sang Chế độ tự động";
                            break;
                        case 5:
                            result.Content = "Chuyển sang Chế độ bảo trì";
                            break;
                        case 6:
                            result.Content = "Lỗi";
                            break;
                        case 7:
                            result.Content = "Đã reset";
                            break;
                        default:
                            result.Content = "";
                            break;
                    }
                    break;
                case "Process Event":
                    switch (input.EventID)
                    {
                        case 1:
                            result.Content = "Đã khởi tạo";
                            break;
                        case 2:
                            result.Content = "Đã kích hoạt";
                            break;
                        case 3:
                            result.Content = "Đang khởi động";
                            break;
                        case 4:
                            result.Content = "Đã khởi động";
                            break;
                        case 5:
                            result.Content = "Đã ở trạng thái xác lập";
                            break;
                        case 6:
                            result.Content = "Đang dừng";
                            break;
                        case 7:
                            result.Content = "Đã dừng";
                            break;
                        case 8:
                            result.Content = "Đã vô hiệu";
                            break;
                        case 9:
                            result.Content = "Đã dừng khẩn cấp";
                            break;
                        case 10:
                            result.Content = "Lỗi: Đã dừng khẩn cấp quá trình";
                            break;
                        case 11:
                            result.Content = "Đã reset";
                            break;
                        default:
                            result.Content = "";
                            break;
                    }
                    break;
                case "Analog Alarm":
                    switch (input.EventID)
                    {
                        case 1:
                            result.Content = "HIHI";
                            break;
                        case 2:
                            result.Content = "HI";
                            break;
                        case 3:
                            result.Content = "LO";
                            break;
                        case 4:
                            result.Content = "LOLO";
                            break;
                        default:
                            result.Content = "";
                            break;
                    }
                    break;
                case "Digital Alarm":
                    switch (input.EventID)
                    {
                        case 1:
                            result.Content = "ON";
                            break;
                        case 2:
                            result.Content = "OFF";
                            break;
                        default:
                            result.Content = "";
                            break;
                    }
                    break;
                default:
                    result.Content = input.EventClass + ", ID = " + input.EventID;
                    break;
            }

            switch (input.SourceName)
            {
                case "Condense Pump":
                    result.Source = "Bơm ngưng";
                    break;
                case "Supply Pump":
                    result.Source = "Bơm cấp";
                    break;
                case "Circular Pump":
                    result.Source = "Bơm tuần hoàn";
                    break;
                case "Low Pressure Heater Valve":
                    result.Source = "Van Gia nhiệt hạ áp";
                    break;
                case "Deaerator Valve":
                    result.Source = "Van Bình khử khí";
                    break;
                case "High Pressure Heater Valve":
                    result.Source = "Van Gia nhiệt cao áp";
                    break;
                case "Turbine Valve":
                    result.Source = "Van Tua bin";
                    break;
                case "Force Fan 1":
                    result.Source = "Quạt cấp 1";
                    break;
                case "Force Fan 2":
                    result.Source = "Quạt cấp 2";
                    break;
                case "Force Fan 3":
                    result.Source = "Quạt cấp 3";
                    break;
                case "Process Controller":
                    result.Source = "Bộ điều khiển quá trình";
                    break;
                case "LP Heater Temperature":
                    result.Source = "Nhiệt độ GN hạ áp";
                    break;
                case "LP Heater Pressure":
                    result.Source = "Áp suất GN hạ áp";
                    break;
                case "Deaerator Temperature":
                    result.Source = "Nhiệt độ Bình khử khí";
                    break;
                case "HP Heater Temperature":
                    result.Source = "Nhiệt độ GN cao áp";
                    break;
                case "HP Heater Pressure":
                    result.Source = "Áp suất GN cao áp";
                    break;
                case "Boiler Temperature":
                    result.Source = "Nhiệt độ Lò hơi";
                    break;
                case "Condenser Temperature":
                    result.Source = "Nhiệt độ Bình ngưng";
                    break;
                case "Turbine Temperature":
                    result.Source = "Nhiệt độ Tua bin";
                    break;
                case "Turbine Pressure":
                    result.Source = "Áp suất Tua bin";
                    break;
                case "Turbine Speed":
                    result.Source = "Tốc độ quay Tua bin";
                    break;
                default:
                    result.Source = input.SourceName;
                    break;
            }

            result.TimeRaised = (input.TimeRaised == new DateTime(1899, 12, 30, 00, 00, 00, 000) || input.TimeRaised == null) ? null : ((DateTime)input.TimeRaised).ToString(CultureInfo.InvariantCulture);
            result.TimeConfirmed = (input.TimeConfirmed == new DateTime(1899, 12, 30, 00, 00, 00, 000) || input.TimeConfirmed == null) ? null : ((DateTime)input.TimeConfirmed).ToString(CultureInfo.InvariantCulture);
            result.TimeCleared = (input.TimeCleared == new DateTime(1899, 12, 30, 00, 00, 00, 000) || input.TimeCleared == null) ? null : ((DateTime)input.TimeCleared).ToString(CultureInfo.InvariantCulture);
            return result;
        }

        private List<string> FormatComponentInPDF<T> (T com)
        {
            dynamic temp;
            List<string> result = new List<string> { "","","","","",""};
            if (com is aFb_Motor) { temp = com as aFb_Motor; result[2] = temp.ActualSpeed.ToString("0"); }
            else { temp = com as aFb_Valve; result[2] = temp.OpenPercent.ToString("0.00"); }

            switch (temp.Mode)
            {
                case (short)0:
                    result[0] = "Bằng tay";
                    break;
                case (short)1:
                    result[0] = "Tự động";
                    break;
                case (short)2:
                    result[0] = "Bảo trì";
                    break;
            }

            if (temp.RunFeedback == true) result[1] = "Đang chạy";
            else result[1] = "Đang dừng";

            var runtime = TimeSpan.FromSeconds(temp.Runtime);
            var aruntime = TimeSpan.FromSeconds(temp.AccRuntime);
            result[3] = string.Format("{0}:{1:00}:{2:00}", (int)runtime.TotalHours, runtime.Minutes, runtime.Seconds);
            result[4] = string.Format("{0}:{1:00}:{2:00}", (int)aruntime.TotalHours, aruntime.Minutes, aruntime.Seconds);

            if (temp.Fault != true) result[5] = "";
            else result[5] = "Lỗi";
            return result;
        }
        #endregion
    }
}
