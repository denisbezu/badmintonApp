using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
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
        private WorkWithMainPage workMain;
        private BadmintonContext context;
        public Tournament CurrentTournament { get; set; }
        public MainPage()
        {
            InitializeComponent();
        }
        #region MenuEdit
        private void City_OnClick(object sender, RoutedEventArgs e)
        {
            Cities cities = new Cities(context);
            cities.ShowDialog();
        }
        private void Category_OnClick(object sender, RoutedEventArgs e)
        {
            Categories categories = new Categories(context);
            categories.ShowDialog();
        }
        private void Grade_OnClick(object sender, RoutedEventArgs e)
        {
            Grades grades = new Grades(context);
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
        #endregion
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            context = new BadmintonContext();
            #region LoadContext
            context.Cities.Load();
            context.Grades.Load();
            context.Clubs.Load();
            context.Coaches.Load();
            context.Unions.Load();
            #endregion
            workMain = new WorkWithMainPage(context, CurrentTournament);
            context.Configuration.AutoDetectChangesEnabled = true;
            workMain.EventsLoad();
            workMain.PlayersLoad();
            eventsListBox.ItemsSource = workMain.EventsList;
            if (eventsListBox.Items.Count > 0)
                eventsListBox.SelectedIndex = 0;
        }
        private void newEvent_Copy_Click(object sender, RoutedEventArgs e)
        {
            EventList eventList = new EventList(context, workMain.EventsList);
            eventList.CurrentTournament = workMain.CurrentTournament;
            eventList.ShowDialog();
        }
        private void Coach_OnClick(object sender, RoutedEventArgs e)
        {
            CoachesList coachesList = new CoachesList(context);
            coachesList.ShowDialog();
        }
        private void Open_OnClick(object sender, RoutedEventArgs e)
        {
            TournamentChooser tournamentChooser = new TournamentChooser();
            tournamentChooser.Show();
            Close();
        }

        private void eventsListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            playersListView.ItemsSource = workMain.EventSelectionChanged(eventsListBox.SelectedItem as Event);
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            playersListView.ItemsSource = workMain.Search(eventsListBox.SelectedItem as Event, txtSearch.Text);
        }

        private void Menu_add_OnClick(object sender, RoutedEventArgs e)
        {
            PlayerAdd playerAdd = new PlayerAdd();
            playerAdd.ShowDialog();
            if (playerAdd.NewPlayer != null)
            {
                context.Players.Local.Add(playerAdd.NewPlayer);
                context.SaveChanges();
            }
            playersListView.ItemsSource = null;
            workMain.RefreshPlayers();
            playersListView.ItemsSource = workMain.EventSelectionChanged(eventsListBox.SelectedItem as Event);
        }
    }
}
