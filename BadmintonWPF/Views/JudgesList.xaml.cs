using System.Collections.Generic;
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
    /// Interaction logic for JudgesList.xaml
    /// </summary>
    public partial class JudgesList : Window, INotifyPropertyChanged
    {
        private Judge selectedJudge;
        public Judge SelectedJudge
        {
            get { return selectedJudge; }
            set
            {
                selectedJudge = value;
                OnPropertyChanged("SelectedJudge");
            }
        }
        BadmintonContext context = new BadmintonContext();

        public BindingList<Judge> Judges { get; set; }
        
        public JudgesList()
        {
            InitializeComponent();
            context.Judges.Load();
            Judges = new BindingList<Judge>();
            Judges = context.Judges.Local.ToBindingList();
            judgesListBox.ItemsSource = Judges;
            context.Cities.Load();
            cmbBoxCities.ItemsSource = context.Cities.Local.OrderBy(p => p.CityName);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            JudgesAdd judgesAdd = new JudgesAdd(context);
            judgesAdd.ShowDialog();
            if (judgesAdd.NewJudge != null)
                context.Judges.Local.Add(judgesAdd.NewJudge);
            context.SaveChanges();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            context.SaveChanges();
            Close();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (judgesListBox.SelectedItem != null)
                {
                    context.Tournaments.Where(v => v.JudgeId == ((Tournament)judgesListBox.SelectedItem).TournamentId).Load();
                    context.Judges.Local.Remove((Judge)judgesListBox.SelectedItem);
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
    }
}
