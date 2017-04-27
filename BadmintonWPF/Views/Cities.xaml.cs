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
        private BadmintonContext context;
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
        public Cities()
        {
            InitializeComponent();
            context = new BadmintonContext();
            context.Cities.Load();
            CitiesList = new BindingList<City>();
            CitiesList = context.Cities.Local.ToBindingList();
            citiesListBox.ItemsSource = CitiesList;
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtAdd.Text != "")
                {
                    context.Cities.Local.Add(new City() {CityName = txtAdd.Text});
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
                    context.Coaches.Where(v => v.CityId  == ((City)citiesListBox.SelectedItem).CityId).Load();
                    context.Clubs.Where(v => v.CityId == ((City)citiesListBox.SelectedItem).CityId).Load();
                    context.Players.Where(v => v.CityId == ((City)citiesListBox.SelectedItem).CityId).Load();
                    context.Judges.Where(v => v.CityId == ((City)citiesListBox.SelectedItem).CityId).Load();
                    context.Tournaments.Where(v => v.CityId == ((City)citiesListBox.SelectedItem).CityId).Load();
                    context.Cities.Local.Remove((City) citiesListBox.SelectedItem);
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
            context.SaveChanges();
            Close();
        }
    }
}
