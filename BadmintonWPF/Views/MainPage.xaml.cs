using System;
using System.Collections;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using badmintonDataBase.DataAccess;
using badmintonDataBase.Enums;
using badmintonDataBase.Models;
using BadmintonWPF.Helpers;

namespace BadmintonWPF.Views
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Window
    {
        public ListPage ListPage { get; set; }
        public Nums Nums { get; set; }
        public DrawsPage DrawsPage { get; set; }
        public TornamentPlayersHelper TornamentPlayersHelper { get; set; }
        public PlayersHelper PlayersHelper { get; set; }
        public BadmintonContext Context { get; }
        public EventsHelper EventsHelper { get; set; }
        public Tournament CurrentTournament { get; set; }
        public MainPage(Tournament tournament)
        {
            WaitWindow waitWindow = new WaitWindow();
            waitWindow.Show();
            InitializeComponent();
            CurrentTournament = tournament;
            Nums = new Nums();
            Context = new BadmintonContext();
            ListPage = new ListPage(this);
            DrawsPage = new DrawsPage(this);
            changerFrame.Navigate(ListPage);
            #region LoadContext
            Context.Cities.Load();
            Context.Grades.Load();
            Context.Clubs.Load();
            Context.Coaches.Load();
            Context.Unions.Load();
            Context.TeamsTournaments.Load();
            Context.PlayersTeams.Load();
            #endregion
            TornamentPlayersHelper = new TornamentPlayersHelper(Context, CurrentTournament);
            EventsHelper = new EventsHelper(Context, CurrentTournament);
            PlayersHelper = new PlayersHelper(Context);
            Context.Configuration.AutoDetectChangesEnabled = true;
            EventsHelper.EventsLoad();
            PlayersHelper.PlayersLoad();
            TornamentPlayersHelper.TeamTournamentsLoad();
            eventsListBox.ItemsSource = EventsHelper.EventsList;
            if (eventsListBox.Items.Count > 0)
                eventsListBox.SelectedIndex = 0;
            waitWindow.Close();
        }
        #region MenuEdit
        private void City_OnClick(object sender, RoutedEventArgs e)
        {
            Cities cities = new Cities(Context);
            cities.ShowDialog();
        }
        private void Category_OnClick(object sender, RoutedEventArgs e)
        {
            Categories categories = new Categories(Context);
            categories.ShowDialog();
        }
        private void Grade_OnClick(object sender, RoutedEventArgs e)
        {
            Grades grades = new Grades(Context);
            grades.ShowDialog();
        }
        private void Union_OnClick(object sender, RoutedEventArgs e)
        {
            Unions unions = new Unions();
            unions.ShowDialog();
        }
        private void Club_OnClick(object sender, RoutedEventArgs e)
        {
            Clubs clubs = new Clubs();
            clubs.ShowDialog();
        }
        private void Judge_OnClick(object sender, RoutedEventArgs e)
        {
            JudgesList judgesList = new JudgesList();
            judgesList.ShowDialog();
        }
        private void Coach_OnClick(object sender, RoutedEventArgs e)
        {
            CoachesList coachesList = new CoachesList(Context);
            coachesList.ShowDialog();
        }
        #endregion
        #region MenuFile

        private void Open_OnClick(object sender, RoutedEventArgs e)
        {
            TournamentChooser tournamentChooser = new TournamentChooser();
            tournamentChooser.Show();
            Close();
        }

        #endregion
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
        private void newEvent_Copy_Click(object sender, RoutedEventArgs e)
        {
            EventList eventList = new EventList(Context, EventsHelper.EventsList);
            eventList.CurrentTournament = TornamentPlayersHelper.CurrentTournament;
            eventList.ShowDialog();
        }
        private void eventsListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ListPage.playersListView.ItemsSource = PlayersHelper.EventSelectionChangedPlayers(eventsListBox.SelectedItem as Event);
            ListPage.tournamentPlayersListView.ItemsSource =
                TornamentPlayersHelper.EventSelectionChangedTournament(eventsListBox.SelectedItem as Event);
            DrawsPage.EventChangedDrawing();
        }
        private void spiski_Click(object sender, RoutedEventArgs e)
        {
            changerFrame.Navigate(ListPage);
        }
        private void Setki_OnClick(object sender, RoutedEventArgs e)
        {
            changerFrame.Navigate(DrawsPage);
            DrawsPage.EventChangedDrawing();
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if ((eventsListBox.SelectedItem as Event).IsDrawFormed == true)
            {
                var eventId = (eventsListBox.SelectedItem as Event).EventId;
                Context.GamesTournaments.Where(p => p.EventId == eventId).Load();
                foreach (var gamesTournament in Context.GamesTournaments.Where(p => p.EventId == eventId).ToList())
                {
                    Context.GamesTournaments.Local.Remove(gamesTournament);
                }
                (eventsListBox.SelectedItem as Event).IsDrawFormed = false;
                DrawsPage.EventChangedDrawing();
            }
            Context.SaveChanges();
        }
    }
}
