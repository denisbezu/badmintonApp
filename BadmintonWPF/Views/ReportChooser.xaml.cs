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
        private List<string> sheetsList = new List<string>() { "MS", "WS", "MD", "WD", "XD" };
        private List<string> categoriesList = new List<string>() { "ЧОЛОВІЧА ОДИНОЧНА", "ЖІНОЧА ОДИНОЧНА", "ЧОЛОВІЧА ПАРНА", "ЖІНОЧА ПАРНА", "ЗМІШАНА ПАРНА" };
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
            if (reportFormer.OpenWorkbook())
            {
                reportFormer.WriteHeaderPlayersList();
                reportFormer.WriteHeaderWS1(reportFormer.Worksheet, "A4", "H4");
                int lastMan = reportFormer.PlayerListFormer("A6", "H6", "Юноши");
                lastMan += 8 + reportFormer.PlayerListFormer("A" + (8 + lastMan), "H" + (8 + lastMan), "Женщины");
                reportFormer.JudgeWriter(reportFormer.Sheets[1], "E" + (lastMan + 2), "G" + (lastMan + 2));
                for (int i = 2; i < 7; i++)
                {
                    reportFormer.WriteHeaderResults(reportFormer.Sheets[i], sheetsList[i - 2]);
                    reportFormer.WriteHeaderNameResults(reportFormer.Sheets[i], categoriesList[i - 2]);
                    reportFormer.WriteHeaderForTableResults(reportFormer.Sheets[i], "A4", "I4");
                    lastMan = reportFormer.ResultsPlayersWithPlaces(reportFormer.Sheets[i], i);
                    reportFormer.JudgeWriter(reportFormer.Sheets[i], "E" + (lastMan + 8), "G" + (lastMan + 8));
                }
                
            }
        }
    }
}
