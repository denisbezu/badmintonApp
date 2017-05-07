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
        private ListPage _listPage;
        private DrawsPage _drawsPage;
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
            Context = new BadmintonContext();
            _listPage = new ListPage(this);
            _drawsPage = new DrawsPage();
            changerFrame.Navigate(_listPage);
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
        //
        private void eventsListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _listPage.playersListView.ItemsSource = PlayersHelper.EventSelectionChangedPlayers(eventsListBox.SelectedItem as Event);
            _listPage.tournamentPlayersListView.ItemsSource =
                TornamentPlayersHelper.EventSelectionChangedTournament(eventsListBox.SelectedItem as Event);
        }

        private void spiski_Click(object sender, RoutedEventArgs e)
        {
            changerFrame.Navigate(_listPage);
        }

        private void Setki_OnClick(object sender, RoutedEventArgs e)
        {
            changerFrame.Navigate(_drawsPage);
        }
    }
}
