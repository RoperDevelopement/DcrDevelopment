using AlanoClubInventory.Models;
using AlanoClubInventory.Utilites;
using ScottPlot;
using ScottPlot.Plottables;
using ScottPlot.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using static ScottPlot.WPF.WpfPlot;
using static ScottPlot.Plottables.Heatmap.RenderStrategies;
using static ScottPlot.Plottables.Heatmap.RenderStrategies.Bitmap;
using Scmd = AlanoClubInventory.SqlServices;
//https://scottplot.net/
namespace AlanoClubInventory.ViewModels
{
    public class AlanoClubDashBoardModle:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool isGraphVisable;
        private DateTime reportEDate;
        private DateTime reportSDate;
        private ICommand createGraph;
        private ICommand printGraph;
        public AlanoClubDashBoardModle()
        {
            IsGraphVisable = false;
            GetDatesReport();
           // GetSalesData();

        }
        public DateTime ReportSDate
        {
            get { return reportSDate; }
            set
            {
                if (reportSDate != value)
                {
                    reportSDate = value;
                    OnPropertyChanged(nameof(ReportSDate));
                }
            }
        }
        public DateTime ReportEDate
        {
            get { return reportEDate; }
            set
            {
                if (reportEDate != value)
                {
                    reportEDate = value;
                    OnPropertyChanged(nameof(ReportEDate));
                }
            }
        }
        public bool IsGraphVisable
        {
            get => isGraphVisable;
            set
            {
                isGraphVisable = value;
                OnPropertyChanged(nameof(IsGraphVisable));

            }
        }
        public ICommand CreateGraph
        {
            get
            {
                if (createGraph == null)
                {
                    createGraph = new RelayCommd(GenerateGraph, param => CanGenerateReport());
                }
                return createGraph;
            }
        }
        public ICommand PrintGraph
        {
            get
            {
                if (printGraph == null)
                {
                    printGraph = new RelayCommd(param => GraphPrint(), param => true);
                }
                return printGraph;
            }
        }
        private async void GetSqlConn()
        {
            SqlConnectionStr = await Utilites.ALanoClubUtilites.GetConnectionStr();
        }
        private string SqlConnectionStr { get; set; }
        private IList<AlanoCLubGraphModel> ACGraphModel { get; set; }
       public WpfPlot PlotControl { get; } = new WpfPlot();
        private bool CanGenerateReport()
        {
            // Add logic to determine if the command can execute
            return true; // For now, always return true
        }
        private async void GetDatesReport()
        {
            GetSqlConn();
            var eDate = DateTime.Now;
            var sDate = DateTime.Now;
            Scmd.AlClubSqlCommands.SqlCmdInstance.GetReportDate(SqlConnectionStr, Scmd.SqlConstProp.SPGetDateReoprtLastRan, ref sDate, ref eDate);
            ReportEDate = eDate;
            ReportSDate = sDate;
        }
        public async void GenerateGraph(object parameter)
        {
            if (ReportSDate > ReportEDate)
            {
                Utilites.ALanoClubUtilites.ShowMessageBox($"Error Report dates End Date Invalid {ReportEDate.ToString("MM-dd-yyyy")} has to be bigger or equal to Start Date {ReportSDate.ToString("MM-dd-yyyy")}", "Invalid Report Dates", MessageBoxButton.OK, MessageBoxImage.Error);
                ReportEDate = DateTime.Now.AddDays(1);
                return;
            }
            if (ReportSDate.Year != ReportEDate.Year)
            {
                Utilites.ALanoClubUtilites.ShowMessageBox($"Error report years have to be the same {ReportSDate.Year} and {ReportEDate.Year}", "Error Years", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            GetSalesData();
        }
        private async void Plot()
        {
            
            PlotControl.Plot.Clear();

            double[] x = new double[ACGraphModel.Count];
            double[] y = new double[ACGraphModel.Count];
            double  pos = 1;
            int tpos = 1;
            int post = 0;
            Tick[] ticks = new Tick[ACGraphModel.Count];
            //double[] positions = new double[ACGraphModel.Count];
            for (int i = 0; i < ACGraphModel.Count; i++)
            {
                var dc = ACGraphModel[i].DateCreated.ToString("MM-dd-yyyy");

                //PlotControl.Plot.Add.Bar(position: pos++, value: ACGraphModel[i].DailyProductTotal);
                 var barPlot = PlotControl.Plot.Add.Bar(position: pos++, value:(double)ACGraphModel[i].ItemsSold);

                 barPlot.LegendText = $"Items Sold {ACGraphModel[i].ItemsSold} Daily Sales ${ACGraphModel[i].DailyProductTotal}";
                ticks[post++] = new(tpos++,dc);
                y[i] = ACGraphModel[i].DailyProductTotal;
                //x[i] = (double)(ACGraphModel[i].ItemsSold);
            }
          //  PlotControl.Plot.Add.Bars(y);
            //  double[] positions = Array.ConvertAll(x, x => DateTime.Parse(x).ToOADate());
            // var bars = PlotControl.Plot.Add.Bar(y);
            //   var bar1 = PlotControl.Plot.Add.Bars(x);
            //foreach (var bar in bars.Bars)
            // {
            //   bar.Label = "Hewllo";
            // }


            //bars.LegendText = "Daily Sales";
            //  for (int i = 0; i < x.Length; i++)
            //  {
            //     string dateLabel = DateTime.FromOADate(positions[i]).ToString("MM-dd-yyyy");
            //      plt.Add.Text(dateLabel, positions[i], x[i] + 2, size: 12, color: System.Drawing.Color.Black);
            //  }

            //        double[] xs1 = { 1, 2, 3, 4 };
            //      double[] ys1 = { 5, 10, 7, 13 };
            //    var bars1 = PlotControl.Plot.Add.Bars(xs1, ys1);
            //  bars1.LegendText = "Alpha";

            //double[] xs2 = { 6, 7, 8, 9 };
            //double[] ys2 = { 7, 12, 9, 15 };
            //var bars2 = PlotControl.Plot.Add.Bars(xs2, ys2);
            //bars2.LegendText = "Beta";

            //PlotControl.Plot.ShowLegend(Alignment.UpperLeft);
            // PlotControl.Plot.Axes.Margins(bottom: 0);
            //     List<PieSlice> slices = new()
            //{
            //  new PieSlice() { Value = 5, FillColor = Colors.Red, Label = "Red" },
            // new PieSlice() { Value = 2, FillColor = Colors.Orange, Label = "Orange" },
            //new PieSlice() { Value = 8, FillColor = Colors.Gold, Label = "Yellow" },
            // new PieSlice() { Value = 4, FillColor = Colors.Green, Label = "Green" },
            // new PieSlice() { Value = 8, FillColor = Colors.Blue, Label = "Blue" },
            //};

            //          var pie = PlotControl.Plot.Add.Pie(slices);
            //        pie.DonutFraction = .5;
            //
            //      PlotControl.Plot.ShowLegend();

            // hide unnecessary plot components
            //    PlotControl.Plot.Axes.Frameless();
            //  PlotControl.Plot.HideGrid();
            //    PlotControl.Plot.Axes.DateTimeTicksBottom();

               PlotControl.Plot.ShowLegend(Alignment.UpperLeft);
            PlotControl.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(ticks);
            PlotControl.Plot.Axes.Bottom.MajorTickStyle.Length = 0;
            PlotControl.Plot.Axes.Margins(bottom: 0);
            
            //   PlotControl.Width = 500;
            // PlotControl.Height = 1000;
            //PlotControl.Plot.Add.RadialGaugePlot(bars1);



            PlotControl.Refresh();
            
            
        }
        private async void GetSalesData()
        {
            if (string.IsNullOrWhiteSpace(SqlConnectionStr))
                    GetSqlConn();
            var sp = new List<StoredParValuesModel>();
            sp.Add(new StoredParValuesModel { ParmaName = Scmd.SqlConstProp.SPParmaRepSDate, ParmaValue = ReportSDate.ToString()});
            sp.Add(new StoredParValuesModel { ParmaName = Scmd.SqlConstProp.SPParmaRepEDate, ParmaValue = ReportEDate.ToString()});


         
        ACGraphModel = await Scmd.AlClubSqlCommands.SqlCmdInstance.CallStoreProdByParmaters<AlanoCLubGraphModel>(SqlConnectionStr, Scmd.SqlConstProp.SPALanoClubGetDailySalesGraphChart,sp);
            if (ACGraphModel.Count > 0)
            {
                Plot();
                IsGraphVisable = true;

            }
        }
        private async void GraphPrint()
        {
            var bmp = PlotControl.Plot.GetImage(1000, 1000);
            string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            tempPath = $"{tempPath}\\Temp\\AlanoClubInventory";
            Utilites.ALanoClubUtilites.CreateFolder(tempPath);
            string filePath = System.IO.Path.Combine(tempPath, "AlanoClubSalesGraph.png");
            bmp.Save(filePath, ImageFormat.Png);
            Utilites.ALanoClubUtilites.ShowMessageBox($"Graph saved to {filePath}", "Graph Saved", MessageBoxButton.OK, MessageBoxImage.Information);   
            PGraphPrint(filePath);
        }
        private async void PGraphPrint(string filePath)
        {
         
               

                //bmp.Save(ms, ImageFormat.Png);
                

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(filePath, UriKind.Absolute);
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    DrawingVisual visual = new DrawingVisual();
                    using (DrawingContext dc = visual.RenderOpen())
                    {
                        dc.DrawImage(bitmapImage, new Rect(0, 0, bitmapImage.PixelWidth, bitmapImage.PixelHeight));
                    }
                    printDialog.PrintVisual(visual, "ScottPlot Graph");
                }
            }

         
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
