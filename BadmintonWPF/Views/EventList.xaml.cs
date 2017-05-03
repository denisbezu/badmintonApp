using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using badmintonDataBase.DataAccess;
using badmintonDataBase.Models;
using BadmintonWPF.Helpers;

namespace BadmintonWPF.Views
{
    /// <summary>
    /// Interaction logic for EventList.xaml
    /// </summary>
    public partial class EventList : Window, INotifyPropertyChanged
    {
        public BadmintonContext Context { get; set; }
        public Tournament CurrentTournament { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        private Event selectedEvent;
        public Event SelectedEvent
        {
            get { return selectedEvent; }
            set
            {
                selectedEvent = value;
                OnPropertyChanged("SelectedEvent");
            }
        }
        public BindingList<Event> EventsList { get; set; }
        public EventList(BadmintonContext context, BindingList<Event> bindingListEvents)
        {
            InitializeComponent();
            EventsList = bindingListEvents;
            Context = context;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            eventsListView.ItemsSource = EventsList;
            cmbBoxCountDraw.ItemsSource = new EventListHelper().DrawsType;
            Context.Categories.Load();
            cmbBoxCategory.ItemsSource = Context.Categories.Local.OrderBy(p => p.CategoryName).ToList();
            Context.Types.Load();
            cmbBoxType.ItemsSource = Context.Types.Local.ToList();
            eventsListView.SelectedIndex = 0;
        }
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            Context.SaveChanges();
            Close();
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (eventsListView.SelectedItem != null)
                {                    
                    Context.Events.Local.Remove((Event)eventsListView.SelectedItem);
                }
            }
            catch
            {
                MessageBox.Show("Не удалось удалить запись", "Удаление", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            EventAdd eventAdd = new EventAdd();
            eventAdd.ShowDialog();
            if (eventAdd.NewEvent != null)
            {
                eventAdd.NewEvent.TournamentId = CurrentTournament.TournamentId;
                Context.Events.Local.Add(eventAdd.NewEvent);
                Context.Entry(eventAdd.NewEvent).State = EntityState.Added;
                Context.SaveChanges();
            }
        }
    }
}
