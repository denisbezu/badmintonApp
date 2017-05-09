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
    /// Interaction logic for Seed.xaml
    /// </summary>
    public partial class Seed : Window
    {
        public BadmintonContext Context { get; set; }
        public BindingList<TeamsTournament> TeamsList { get; set; }
        public Event SelectedEvent { get; set; }
        public Seed(BadmintonContext context, Event selectedEvent)
        {
            InitializeComponent();

            Context = context;
            SelectedEvent = selectedEvent;
            TeamsLoad();
            gridPlayers.RowBackground = new SolidColorBrush(Colors.Transparent);

        }
        private void BtnOk_OnClick(object sender, RoutedEventArgs e)
        {
            Context.SaveChanges();
            Close();
        }
        private void TeamsLoad()
        {
            Context.TeamsTournaments.Local.Where(p => p.EventId == SelectedEvent.EventId)
                .ToList();
            TeamsList = new BindingList<TeamsTournament>(Context.TeamsTournaments.Where(p => p.EventId == SelectedEvent.EventId).OrderBy(p => p.SeedingNumber).ToList());
            gridPlayers.ItemsSource = TeamsList;
        }
        private void RefreshGrid()
        {
            gridPlayers.ItemsSource = null;
            TeamsList = null;
            TeamsLoad();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            foreach (var ev in TeamsList)
            {
                bool new_chiffre = true;
                bool changed = false;
                if (ev.SeedingNumber == 0)
                {
                    while (new_chiffre)
                    {
                        int k = rnd.Next(1, TeamsList.Count + 1);
                        foreach (var item in TeamsList)
                        {
                            if (k == item.SeedingNumber)
                            {
                                changed = true;
                                break;
                            }
                        }
                        if (!changed)
                        {
                            new_chiffre = false;
                            ev.SeedingNumber = k;
                        }
                        changed = false;
                    }
                }
            }
            RefreshGrid();
        }
    }
}
