using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace BadmintonWPF.Views
{
    /// <summary>
    /// Interaction logic for PlayerEdit.xaml
    /// </summary>
    public partial class PlayerEdit : Window,INotifyPropertyChanged
    {
        public BadmintonContext Context { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        private Player selectedPlayer;
        public Player SelectedPlayer
        {
            get { return selectedPlayer; }
            set
            {
                selectedPlayer = value;
                OnPropertyChanged("SelectedPlayer");
            }
        }
        
        public PlayerEdit(BadmintonContext context, Player selectedPlayer)
        {
            InitializeComponent();
            Context = context;
            SelectedPlayer = selectedPlayer;
            DataContext = SelectedPlayer;
            Context.Cities.Load();
            Context.Grades.Load();
            Context.Coaches.Load();
            Context.Clubs.Load();
            Context.Unions.Load();
            cmbBoxCity.ItemsSource = Context.Cities.OrderBy(p => p.CityName).ToList();
            cmbBoxClub.ItemsSource = Context.Clubs.OrderBy(p => p.ClubName).ToList();
            cmbBoxGrade.ItemsSource = Context.Grades.OrderBy(p => p.GradeName).ToList();
            cmbBoxUnion.ItemsSource = Context.Unions.OrderBy(p => p.UnionName).ToList();
            cmbBoxCoach.ItemsSource = Context.Coaches.OrderBy(p => p.CoachName).ToList();
        }

        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            //NewPlayer = null;
            Close();
            
        }

        private void BtnOk_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                //SelectedPlayer.PlayerName = txtName.Text;
                Context.SaveChanges();
                Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Неверные данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #region AddButtons

        private void BtnAddCity_OnClick(object sender, RoutedEventArgs e)
        {
            Cities cities = new Cities(Context);
            cities.ShowDialog();
            Context.SaveChanges();
            cmbBoxCity.ItemsSource = Context.Cities.Local.OrderBy(p => p.CityName).ToList();
        }

        private void BtnAddClub_OnClick(object sender, RoutedEventArgs e)
        {
            Clubs clubs = new Clubs();
            clubs.ShowDialog();
            Context.SaveChanges();
            cmbBoxClub.ItemsSource = Context.Clubs.Local.OrderBy(p => p.ClubName).ToList();
        }

        private void BtnAddUnion_OnClick(object sender, RoutedEventArgs e)
        {
            Unions unions = new Unions();
            unions.ShowDialog();

            Context.SaveChanges();
            cmbBoxUnion.ItemsSource = Context.Unions.Local.OrderBy(p => p.UnionName).ToList();
        }

        private void BtnAddCoach_OnClick(object sender, RoutedEventArgs e)
        {
            CoachAdd coachAdd = new CoachAdd();
            coachAdd.ShowDialog();
            if (coachAdd.NewCoach != null)
                Context.Coaches.Local.Add(coachAdd.NewCoach);
            Context.SaveChanges();
            cmbBoxCoach.ItemsSource = Context.Coaches.Local.OrderBy(p => p.CoachName).ToList();
        }

        #endregion
    }
}
