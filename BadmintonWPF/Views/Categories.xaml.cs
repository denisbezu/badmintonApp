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
    /// Interaction logic for Categories.xaml
    /// </summary>
    public partial class Categories : Window, INotifyPropertyChanged
    {
        public BadmintonContext Context { get; set; }
        public BindingList<Category> CategoriesList { get; set; }
        public Categories(BadmintonContext context)
        {
            InitializeComponent();
            Context = context;
            Context.Categories.Load();
            CategoriesList = new BindingList<Category>();
            CategoriesList = Context.Categories.Local.ToBindingList();
            categoriesListBox.ItemsSource = CategoriesList;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        private Category selectedCategory;
        public Category SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                selectedCategory = value;
                OnPropertyChanged("SelectedCategory");
            }
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtAdd.Text != "")
                {
                    Context.Categories.Local.Add(new Category() { CategoryName = txtAdd.Text });
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
                if (categoriesListBox.SelectedItem != null)
                {
                    Context.Events.Where(v => v.CategoryId == ((Category)categoriesListBox.SelectedItem).CategoryId).Load();
                    Context.Categories.Local.Remove((Category)categoriesListBox.SelectedItem);
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
            Context.SaveChanges();
            Close();
        }
    }
}
