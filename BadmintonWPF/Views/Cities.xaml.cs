using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for Cities.xaml
    /// </summary>
    public partial class Cities : Window
    {
        private BadmintonContext context;

        public Cities()
        {
            InitializeComponent();
            context = new BadmintonContext();
            context.Cities.Load();
            citiesListBox.ItemsSource = context.Cities.Local.ToBindingList();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtAdd.Text != "")
                {
                    context.Cities.Local.Add(new City() {CityName = txtAdd.Text});
                    txtAdd.Text = "";
                }
            }
            catch
            {
                MessageBox.Show("Не удалось добавить запись", "Добавление", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                City city = context.Cities.Local
                    .Single(p => p.CityName == citiesListBox.SelectedItem.ToString());
                city.CityName = txtEdit.Text;
                citiesListBox.ItemsSource = null;
                citiesListBox.Items.Clear();
                context.Cities.Load();
                citiesListBox.ItemsSource = context.Cities.Local.ToBindingList();
                if (citiesListBox.Items.Count > 0)
                    citiesListBox.SelectedIndex = 0;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Не удалось изменить запись", "Изменение", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (citiesListBox.SelectedItem != null)
                {
                    context.Cities.Local.Remove((City) citiesListBox.SelectedItem);
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

        private void citiesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (citiesListBox.SelectedItem != null)
                txtEdit.Text = ((City)citiesListBox.SelectedItem).ToString();
            else
            {
                txtEdit.Text = "";
            }
        }
    }
}
