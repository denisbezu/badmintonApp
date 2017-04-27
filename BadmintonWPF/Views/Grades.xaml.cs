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
    /// Interaction logic for Grades.xaml
    /// </summary>
    public partial class Grades : Window, INotifyPropertyChanged
    {
        private BadmintonContext context;
        public BindingList<Grade> GradesList { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        private Grade selectedGrade;
        public Grade SelectedGrade
        {
            get { return selectedGrade; }
            set
            {
                selectedGrade = value;
                OnPropertyChanged("SelectedGrade");
            }
        }
        public Grades()
        {
            InitializeComponent();
            context = new BadmintonContext();
            context.Grades.Load();
            GradesList = new BindingList<Grade>();
            GradesList = context.Grades.Local.ToBindingList();
            gradesListBox.ItemsSource = GradesList;
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtAdd.Text != "")
                {
                    context.Grades.Local.Add(new Grade() { GradeName = txtAdd.Text });
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
                if (gradesListBox.SelectedItem != null)
                {
                    context.Players.Where(v => v.GradeId == ((Grade)gradesListBox.SelectedItem).GradeId).Load();
                    context.Grades.Local.Remove((Grade)gradesListBox.SelectedItem);
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
