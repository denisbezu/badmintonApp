using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
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
    /// Interaction logic for CoachesList.xaml
    /// </summary>
    public partial class CoachesList : Window, INotifyPropertyChanged
    {
        private Coach selectedCoach;
        public Coach SelectedCoach
        {
            get { return selectedCoach; }
            set
            {
                selectedCoach = value;
                OnPropertyChanged("SelectedCoach");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public BadmintonContext Context { get; set; }
        public BindingList<Coach> CoachList { get; set; }
        public CoachesList(BadmintonContext context)
        {
            InitializeComponent();
            Context = context;
            Context.Coaches.Load();
            CoachList = new BindingList<Coach>();
            CoachList = Context.Coaches.Local.ToBindingList();
            coachesListView.ItemsSource = CoachList;
            Context.Cities.Load();
            cmbBoxCity.ItemsSource = Context.Cities.Local.OrderBy(p => p.CityName).ToList();
            Context.Clubs.Load();
            cmbBoxClub.ItemsSource = Context.Clubs.Local.OrderBy(p => p.ClubName).ToList();
        }
        private void CoachesList_OnLoaded(object sender, RoutedEventArgs e)
        {
            coachesListView.SelectedIndex = 0;
        }
        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void BtnDelete_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (coachesListView.SelectedItem != null)
                {
                    Context.Players.Where(v => v.CoachId == ((Coach)coachesListView.SelectedItem).CoachId).Load();
                    Context.Coaches.Local.Remove((Coach)coachesListView.SelectedItem);
                }
            }
            catch
            {
                MessageBox.Show("Не удалось удалить запись", "Удаление", MessageBoxButton.OK, MessageBoxImage.Error);
            } 
        }
        private void BtnAdd_OnClick(object sender, RoutedEventArgs e)
        {
            CoachAdd coachAdd = new CoachAdd();
            coachAdd.ShowDialog();
            if (coachAdd.NewCoach != null)
            {
                Context.Coaches.Local.Add(coachAdd.NewCoach);
                Context.SaveChanges();
            }
        }
        private void BtnOk_OnClick(object sender, RoutedEventArgs e)
        {
            Context.SaveChanges();
            Close();
        }
    }
}
