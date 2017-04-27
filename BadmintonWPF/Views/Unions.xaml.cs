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
    /// Interaction logic for Unions.xaml
    /// </summary>
    public partial class Unions : Window, INotifyPropertyChanged
    {
        private BadmintonContext context;
        public BindingList<Union> UnionsList { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        private Union selectedUnion;
        public Union SelectedUnion
        {
            get { return selectedUnion; }
            set
            {
                selectedUnion = value;
                OnPropertyChanged("SelectedUnion");
            }
        }
        public Unions()
        {
            InitializeComponent();
            context = new BadmintonContext();
            context.Unions.Load();
            UnionsList = new BindingList<Union>();
            UnionsList = context.Unions.Local.ToBindingList();
            unionsListBox.ItemsSource = UnionsList;
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtAdd.Text != "")
                {
                    context.Unions.Local.Add(new Union() { UnionName = txtAdd.Text });
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
                if (unionsListBox.SelectedItem != null)
                {
                    context.Players.Where(v => v.PlayerId == ((Union)unionsListBox.SelectedItem).UnionId).Load();
                    context.Unions.Local.Remove((Union)unionsListBox.SelectedItem);
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
