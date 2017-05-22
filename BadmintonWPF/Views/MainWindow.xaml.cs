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
            viewer.ReportPath = @"..\..\Report2.rdlc";
            viewer.ProcessingMode = ProcessingMode.Local;
            context.Players.Load();
            ReportDataSource report = new ReportDataSource("BadmintonDataSet",context.Players.Local.ToList());
            viewer.DataSources.Add(report);

            viewer.RefreshReport();
        }


    }
}
