using System;
using System.Collections.Generic;
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

namespace BadmintonWPF.Views
{
    /// <summary>
    /// Interaction logic for PlayerAdd.xaml
    /// </summary>
    public partial class PlayerAdd : Window
    {
        private BadmintonContext context;
        public Player NewPlayer { get; set; }
        public PlayerAdd()
        {
            InitializeComponent();
            context = new BadmintonContext();
            context.Cities.Load();
            context.Grades.Load();
            context.Coaches.Load();
            context.Clubs.Load();
            context.Unions.Load();
            cmbBoxCity.ItemsSource = context.Cities.OrderBy(p => p.CityName).ToList();
            cmbBoxClub.ItemsSource = context.Clubs.OrderBy(p => p.ClubName).ToList();
            cmbBoxGrade.ItemsSource = context.Grades.OrderBy(p => p.GradeName).ToList();
            cmbBoxUnion.ItemsSource = context.Unions.OrderBy(p => p.UnionName).ToList();
            cmbBoxCoach.ItemsSource = context.Coaches.OrderBy(p => p.CoachName).ToList();
        }

        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            NewPlayer = null;
            Close();
        }

        private void BtnOk_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                NewPlayer = new Player()
                {
                    PlayerName = txtName.Text,
                    PlayerSurName = txtLastName.Text,
                    YearOfBirth = int.Parse(txtYearOfBirth.Text),
                    CityId = (cmbBoxCity.SelectionBoxItem as City).CityId,
                    GradeId = (cmbBoxGrade.SelectedItem as Grade).GradeId,
                    CoachId = (cmbBoxCoach.SelectedItem as Coach).CoachId,
                    Sex = cmbBoxSex.Text,
                    ClubId = (cmbBoxClub.SelectedItem as Club).ClubId,
                    UnionId = (cmbBoxUnion.SelectedItem as Union).UnionId
                };
                Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Неверные данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
