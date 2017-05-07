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
    /// Interaction logic for TournamentAdd.xaml
    /// </summary>
    public partial class TournamentAdd : Window
    {
        private BadmintonContext context;
        public Tournament NewTournament { get; set; }
        public TournamentAdd()
        {
            InitializeComponent();
            context = new BadmintonContext();
            context.Cities.Load();
            context.Judges.Load();
            cmbBoxCities.ItemsSource = context.Cities.Local.OrderBy(p => p.CityName).ToList();
            cmbBoxJudges.ItemsSource = context.Judges.Local.OrderBy(p => p.JudgeLastName).ToList();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            NewTournament = null;
            Close();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NewTournament = new Tournament()
                {
                    TournamentName = txtName.Text,
                    CityId = (cmbBoxCities.SelectedValue as City).CityId,
                    JudgeId = (cmbBoxJudges.SelectedValue as Judge).JudgeId,
                    StartDate = DateTime.Parse(txtStartDate.Text),
                    FinishDate = DateTime.Parse(txtEndDate.Text)
                };
                Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Неверные данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAddJudge_Click(object sender, RoutedEventArgs e)
        {
            JudgesAdd judgesAdd = new JudgesAdd(context);
            judgesAdd.ShowDialog();
            if (judgesAdd.NewJudge != null)
                context.Judges.Local.Add(judgesAdd.NewJudge);
            context.SaveChanges();
            cmbBoxJudges.ItemsSource = context.Judges.Local.OrderBy(p => p.JudgeLastName).ToList();
        }

        private void BtnAddCity_OnClick(object sender, RoutedEventArgs e)
        {
            Cities cities = new Cities(context);
            cities.ShowDialog();
            cmbBoxCities.ItemsSource = context.Cities.Local.OrderBy(p => p.CityName).ToList();
        }
    }
}
