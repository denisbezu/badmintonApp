using System;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using badmintonDataBase.DataAccess;
using badmintonDataBase.Models;

namespace BadmintonWPF.Views
{

    /// <summary>
    /// Interaction logic for Clubs.xaml
    /// </summary>
    public partial class Clubs : Window, INotifyPropertyChanged
    {
        private BadmintonContext context;
        public BindingList<Club> ClubsList { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        private Club selectedClub;
        public Club SelectedClub
        {
            get { return selectedClub; }
            set
            {
                selectedClub = value;
                OnPropertyChanged("SelectedClub");
            }
        }
        public Clubs()
        {
            InitializeComponent();
            context = new BadmintonContext();
            context.Clubs.Load();
            ClubsList = new BindingList<Club>();
            ClubsList = context.Clubs.Local.ToBindingList();
            clubsListBox.ItemsSource = ClubsList;
            context.Cities.Load();
            cmbBoxAddCity.ItemsSource = context.Cities.Local.OrderBy(p => p.CityName).ToList();
            cmbBoxSelectedCity.ItemsSource = context.Cities.Local.OrderBy(p => p.CityName).ToList();
        }

        private void BtnAdd_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtAdd.Text != "")
                {
                    context.Clubs.Local.Add(new Club() { ClubName = txtAdd.Text, City = cmbBoxAddCity.SelectedValue as City});
                    txtAdd.Text = "";
                }
            }
            catch
            {
                MessageBox.Show("Не удалось добавить запись", "Добавление", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void BtnDelete_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (clubsListBox.SelectedItem != null)
                {
                    context.Coaches.Where(v => v.ClubId == ((Club)clubsListBox.SelectedItem).ClubId).Load();
                    context.Players.Where(v => v.ClubId == ((Club)clubsListBox.SelectedItem).ClubId).Load();

                    context.Clubs.Local.Remove((Club)clubsListBox.SelectedItem);
                }
            }
            catch
            {
                MessageBox.Show("Не удалось удалить запись", "Удаление", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void BtnOk_OnClick(object sender, RoutedEventArgs e)
        {
            context.SaveChanges();
            Close();
        }
        
    }
}
