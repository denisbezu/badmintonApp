using System.Windows;
using badmintonDataBase.DataAccess;
using Excel = Microsoft.Office.Interop.Excel;
using System;
using System.Data.Entity;
using System.Linq;
using Syncfusion.Windows.Reports;
using Syncfusion.Windows.Reports.Viewer;

namespace BadmintonWPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BadmintonContext context = new BadmintonContext();
        public MainWindow()
        {
            InitializeComponent();
            viewer.ReportPath = @"C:\Users\Денис\Documents\visual studio 2017\Projects\badmintonApp\BadmintonWPF\Report1.rdlc";
            viewer.ProcessingMode = ProcessingMode.Local;
            context.Cities.Load();
            ReportDataSource report = new ReportDataSource("BadmintonDataSet",context.Cities.Local.ToList());
            viewer.DataSources.Add(report);

            viewer.RefreshReport();
        }


    }
}
