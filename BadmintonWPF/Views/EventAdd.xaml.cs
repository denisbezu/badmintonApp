using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using badmintonDataBase.DataAccess;
using badmintonDataBase.Models;
using BadmintonWPF.Helpers;
using Type = badmintonDataBase.Models.Type;

namespace BadmintonWPF.Views
{
    /// <summary>
    /// Interaction logic for EventAdd.xaml
    /// </summary>
    public partial class EventAdd : Window
    {
        private BadmintonContext context;
        public Event NewEvent { get; set; }
        public EventAdd()
        {
            InitializeComponent();
            context = new BadmintonContext();
            context.Types.Load();
            context.Categories.Load();
            cmbBoxCountDraw.ItemsSource = new EventListHelper().DrawsType;
            cmbBoxCategory.ItemsSource = context.Categories.Local.OrderBy(p => p.CategoryName).ToList();
            cmbBoxType.ItemsSource = context.Types.Local.ToList();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NewEvent = new Event()
                {
                    CategoryId = (cmbBoxCategory.SelectedItem as Category).CategoryId,
                    DrawType = cmbBoxCountDraw.SelectedValue.ToString(),
                    Sort = cmbBoxSort.SelectedValue.ToString(),
                    TypeId = (cmbBoxType.SelectedItem as Type).TypeId
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
            NewEvent = null;
            Close();
        }
    }
}
