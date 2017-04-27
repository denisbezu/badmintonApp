using System;
using System.Data.Entity;
using System.Windows;
using badmintonDataBase.DataAccess;
using badmintonDataBase.Models;

namespace BadmintonWPF.Views
{
    /// <summary>
    /// Interaction logic for JudgesAdd.xaml
    /// </summary>
    public partial class JudgesAdd : Window
    {
        public Judge NewJudge { get; set; }
        private BadmintonContext context;
        public JudgesAdd()
        {
            InitializeComponent();
            context = new BadmintonContext();
            context.Cities.Load();
            cmBoxCity.ItemsSource = context.Cities.Local.ToBindingList();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NewJudge = new Judge()
                {
                    JudgeName = txtName.Text,
                    JudgeLastName = txtLastName.Text,
                    CityId = (cmBoxCity.SelectedValue as City).CityId,
                    JudgeSurName = txtSurName.Text
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
            NewJudge = null;
            Close();
        }
    }
}
