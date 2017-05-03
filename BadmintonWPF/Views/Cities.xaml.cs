using System;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using badmintonDataBase.DataAccess;
using badmintonDataBase.Models;

namespace BadmintonWPF.Views
{
    /// <summary>
    /// Interaction logic for Cities.xaml
    /// </summary>
    public partial class Cities : Window, INotifyPropertyChanged
    {
        public BadmintonContext Context { get; set; }
        public BindingList<City> CitiesList { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        private City selectedCity;
        public City SelectedCity
        {
            get { return selectedCity; }
            set
            {
                selectedCity = value;
                OnPropertyChanged("SelectedCity");
            }
        }
        public Cities(BadmintonContext context)
        {
            InitializeComponent();
            Context = context;
            Context.Cities.Load();
            CitiesList = new BindingList<City>();
            CitiesList = Context.Cities.Local.ToBindingList();
            citiesListBox.ItemsSource = CitiesList;
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtAdd.Text != "")
                {
                    Context.Cities.Local.Add(new City() {CityName = txtAdd.Text});
                    txtAdd.Text = "";
                }
            }
            catch
            {
                MessageBox.Show("Не удалось добавить запись", "Добавление", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (citiesListBox.SelectedItem != null)
                {
                    Context.Coaches.Where(v => v.CityId  == ((City)citiesListBox.SelectedItem).CityId).Load();
                    Context.Clubs.Where(v => v.CityId == ((City)citiesListBox.SelectedItem).CityId).Load();
                    Context.Players.Where(v => v.CityId == ((City)citiesListBox.SelectedItem).CityId).Load();
                    Context.Judges.Where(v => v.CityId == ((City)citiesListBox.SelectedItem).CityId).Load();
                    Context.Tournaments.Where(v => v.CityId == ((City)citiesListBox.SelectedItem).CityId).Load();
                    Context.Cities.Local.Remove((City) citiesListBox.SelectedItem);
                }
            }
            catch
            {
                MessageBox.Show("Не удалось удалить запись", "Удаление", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            Context.SaveChanges();
            Close();
        }
    }
}
