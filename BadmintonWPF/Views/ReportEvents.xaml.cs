using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.Windows.Shapes;
using badmintonDataBase.DataAccess;
using Syncfusion.Windows.Reports;
using Syncfusion.Windows.Reports.Viewer;

namespace BadmintonWPF.Views
{
    /// <summary>
    /// Interaction logic for ReportEvents.xaml
    /// </summary>
    public partial class ReportEvents : Window
    {
        BadmintonContext context = new BadmintonContext();
        public ReportEvents()
        {
            InitializeComponent();
            viewer.ReportPath = @"..\..\Report1.rdlc";
            viewer.ProcessingMode = ProcessingMode.Local;
            context.Tournaments.Load();
            ReportDataSource report = new ReportDataSource("BadmintonDataSet", context.Tournaments.Local.ToList());
            viewer.DataSources.Add(report);

            viewer.RefreshReport();
        }
    }
}
