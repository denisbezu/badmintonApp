using System;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Windows;
using badmintonDataBase.DataAccess;
using badmintonDataBase.Models;
using BadmintonWPF.Helpers;

namespace BadmintonWPF.Views
{
    /// <summary>
    /// Interaction logic for TournamentChooser.xaml
    /// </summary>
    public partial class TournamentChooser : Window, INotifyPropertyChanged
    {
        private WaitWindow waitWindow;
        private BadmintonContext context;
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        private Tournament selectedTournament;
        public Tournament SelectedTournament
        {
            get { return selectedTournament; }
            set
            {
                selectedTournament = value;
                OnPropertyChanged("SelectedTournament");
            }
        }
        public BindingList<Tournament> TournamentsList { get; set; }
        public TournamentChooser()
        {
            waitWindow = new WaitWindow();
            waitWindow.Show();
            InitializeComponent();
            LoadWindow();
        }
        private void LoadWindow()
        {
            try
            {
                context = new BadmintonContext();
                //if (!TestConnection())
                //    throw new Exception();
                context.Tournaments.Load();
                TournamentsList = new BindingList<Tournament>();
                TournamentsList = context.Tournaments.Local.ToBindingList();
                tournamentsListView.ItemsSource = TournamentsList;
                context.Cities.Load();
                context.Judges.Load();
                cmbBoxCities.ItemsSource = context.Cities.Local.OrderBy(p => p.CityName).ToList();
                cmbBoxJudges.ItemsSource = context.Judges.Local.OrderBy(p => p.JudgeLastName).ToList();

            }
            catch (Exception)
            {
                waitWindow.Close();
                waitWindow = null;
                MessageBox.Show("Не удалось подключитсья к базе данных. Проверьте интернет соединение", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
            if (waitWindow != null)
                waitWindow.Close();
        }
        //private bool TestConnection()
        //{
        //    IPStatus status = IPStatus.Unknown;
        //    try
        //    {
        //        status = new Ping().Send("google.com").Status;
        //    }
        //    catch { }

        //    return status == IPStatus.Success;
        //}

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tournamentsListView.SelectedItem != null)
                {
                    context.Events.Where(v => v.TournamentId == ((Tournament)tournamentsListView.SelectedItem).TournamentId).Load();
                    context.Tournaments.Local.Remove((Tournament)tournamentsListView.SelectedItem);
                }
            }
            catch
            {
                MessageBox.Show("Не удалось удалить запись", "Удаление", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            tournamentsListView.SelectedIndex = 0;
        }
        public void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            TournamentAdd tournamentAdd = new TournamentAdd();
            tournamentAdd.ShowDialog();
            if(tournamentAdd.NewTournament!=null)
                context.Tournaments.Local.Add(tournamentAdd.NewTournament);
            context.SaveChanges();
            context.Judges.Load();
            context.Cities.Load();
            cmbBoxJudges.ItemsSource = context.Judges.Local.OrderBy(p => p.JudgeLastName).ToList();
            cmbBoxCities.ItemsSource = context.Cities.Local.OrderBy(p => p.CityName).ToList();
        }
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            context.SaveChanges();
        }

        private void tournamentsListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MainPage mainPage = new MainPage(tournamentsListView.SelectedItem as Tournament);
            
            mainPage.Show();
            mainPage.Title +="\"" +  (tournamentsListView.SelectedItem as Tournament).TournamentName + "\"";
            this.Close();
            
        }
    }
}
