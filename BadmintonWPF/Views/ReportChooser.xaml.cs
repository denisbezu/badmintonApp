using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using badmintonDataBase.Models;
using BadmintonWPF.Helpers;

namespace BadmintonWPF.Views
{
    /// <summary>
    /// Interaction logic for ReportChooser.xaml
    /// </summary>
    public partial class ReportChooser : Window
    {
        private BadmintonContext Context { get; set; }
        private Tournament Tournament { get; set; }
        private Category Category { get; set; }
        public BindingList<Category> CategoriesList { get; set; }
        public ReportChooser(BadmintonContext context, Tournament tournament, EventsHelper eventsHelper)
        {
            InitializeComponent();
            Context = context;
            Tournament = tournament;
            Context.Categories.Load();
            Context.Events.Where(p => p.TournamentId == Tournament.TournamentId).Load();

            CategoriesList = new BindingList<Category>(eventsHelper.EventsList.Where(p => p.TournamentId == Tournament.TournamentId).Select(p => p.Category).Distinct().ToList());
            lstBoxReports.ItemsSource = CategoriesList;
        }
        private void LstBoxReports_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lstBoxReports.SelectedItem == null)
                return;
            ReportFormer reportFormer = new ReportFormer(Tournament);
            Category = lstBoxReports.SelectedItem as Category;
            reportFormer.Category = Category;
            //reportFormer.Tournament = Tournament;
            if (reportFormer.OpenWorkbook())
            {
                reportFormer.WriteHeaderPlayersList();
                reportFormer.WriteHeaderWS1(reportFormer.Worksheet, "A4", "H4");
                int lastMan = reportFormer.PlayerListFormer("A6", "H6", "Юноши");
                lastMan +=8 + reportFormer.PlayerListFormer("A" + (8 + lastMan), "H" + (8 + lastMan), "Женщины");
                reportFormer.JudgeWriter("E" + (lastMan + 2), "G" + (lastMan + 2));
            }
        }
    }
}
