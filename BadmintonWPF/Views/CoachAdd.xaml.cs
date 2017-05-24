using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using badmintonDataBase.DataAccess;
using badmintonDataBase.Models;

namespace BadmintonWPF.Views
{
    /// <summary>
    /// Interaction logic for CoachAdd.xaml
    /// </summary>
    public partial class CoachAdd : Window
    {
        private BadmintonContext context;
        public Coach NewCoach { get; set; }
        public CoachAdd()
        {
            InitializeComponent();
            context = new BadmintonContext();
            context.Cities.Load();
            cmbBoxCity.ItemsSource = context.Cities.Local.OrderBy(p => p.CityName).ToList();
            context.Clubs.Load();
            cmbBoxClub.ItemsSource = context.Clubs.Local.OrderBy(p => p.ClubName).ToList();
        }
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NewCoach = new Coach()
                {
                    CoachName = txtCoachName.Text,
                    YearOfBirth = int.Parse(txtYearOfBirth.Value.ToString()),
                    CityId = (cmbBoxCity.SelectedItem as City).CityId,
                    ClubId = (cmbBoxClub.SelectedItem as Club).ClubId
                };
                Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Неверные данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            NewCoach = null;
            Close();
        }

        private void BtnAddCity_OnClick(object sender, RoutedEventArgs e)
        {
            Cities cities = new Cities(context);
            cities.ShowDialog();
           
            context.SaveChanges();
            cmbBoxCity.ItemsSource = context.Cities.Local.OrderBy(p => p.CityName).ToList();
        }

        private void BtnAddClub_OnClick(object sender, RoutedEventArgs e)
        {
            Clubs clubs = new Clubs();
            clubs.ShowDialog();
        
            context.SaveChanges();
            cmbBoxClub.ItemsSource = context.Clubs.Local.OrderBy(p => p.ClubName).ToList();
        }
    }
}
