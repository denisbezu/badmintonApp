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
using Type = badmintonDataBase.Models.Type;

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
                    YearOfBirth = int.Parse(txtYearOfBirth.Text),
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
    }
}
