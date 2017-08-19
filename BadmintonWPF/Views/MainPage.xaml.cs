using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using badmintonDataBase.DataAccess;
using badmintonDataBase.Models;
using BadmintonWPF.Helpers;
using Gat.Controls;

namespace BadmintonWPF.Views
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Window
    {
        #region Properties&Variables
        AboutControlView about;
        AboutControlViewModel vm;
        public ListPage ListPage { get; set; }
        public Nums Nums { get; set; }
        public DrawsPage DrawsPage { get; set; }
        public TornamentPlayersHelper TornamentPlayersHelper { get; set; }
        public PlayersHelper PlayersHelper { get; set; }
        public BadmintonContext Context { get; }
        public EventsHelper EventsHelper { get; set; }
        public Tournament CurrentTournament { get; set; }
        #endregion

        public MainPage(Tournament tournament)
        {
            WaitWindow waitWindow = new WaitWindow();
            waitWindow.Show();
            InitializeComponent();
            CurrentTournament = tournament;
            Nums = new Nums();
            Context = new BadmintonContext();
            ListPage = new ListPage(this);

            changerFrame.Navigate(ListPage);
            #region LoadContext
            Context.Cities.Load();
            Context.Grades.Load();
            Context.Clubs.Load();
            Context.Coaches.Load();
            Context.Unions.Load();
            Context.TeamsTournaments.Load();
            Context.PlayersTeams.Load();
            #endregion
            TornamentPlayersHelper = new TornamentPlayersHelper(Context, CurrentTournament);
            EventsHelper = new EventsHelper(Context, CurrentTournament);
            PlayersHelper = new PlayersHelper(Context);
            Context.Configuration.AutoDetectChangesEnabled = true;
            EventsHelper.EventsLoad();
            PlayersHelper.PlayersLoad();
            TornamentPlayersHelper.TeamTournamentsLoad();
            eventsListBox.ItemsSource = EventsHelper.EventsList;
            DrawsPage = new DrawsPage(this);
            if (eventsListBox.Items.Count > 0)
                eventsListBox.SelectedIndex = 0;

            waitWindow.Close();
        }
        #region MenuEdit
        private void City_OnClick(object sender, RoutedEventArgs e)
        {
            Cities cities = new Cities(Context);
            cities.ShowDialog();
        }
        private void Category_OnClick(object sender, RoutedEventArgs e)
        {
            Categories categories = new Categories(Context);
            categories.ShowDialog();
        }
        private void Grade_OnClick(object sender, RoutedEventArgs e)
        {
            Grades grades = new Grades(Context);
            grades.ShowDialog();
        }
        private void Union_OnClick(object sender, RoutedEventArgs e)
        {
            Unions unions = new Unions();
            unions.ShowDialog();
        }
        private void Club_OnClick(object sender, RoutedEventArgs e)
        {
            Clubs clubs = new Clubs();
            clubs.ShowDialog();
        }
        private void Judge_OnClick(object sender, RoutedEventArgs e)
        {
            JudgesList judgesList = new JudgesList();
            judgesList.ShowDialog();
        }
        private void Coach_OnClick(object sender, RoutedEventArgs e)
        {
            CoachesList coachesList = new CoachesList(Context);
            coachesList.ShowDialog();
        }
        #endregion
        #region MenuFile
        private void Open_OnClick(object sender, RoutedEventArgs e)
        {
            TournamentChooser tournamentChooser = new TournamentChooser();
            tournamentChooser.Show();
            Close();
        }
        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void NewTournament_OnClick(object sender, RoutedEventArgs e)
        {
            TournamentChooser tournamentChooser = new TournamentChooser();
            tournamentChooser.Show();
            Close();
            tournamentChooser.btnAdd_Click(sender, e);
        }
        #endregion
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void newEvent_Copy_Click(object sender, RoutedEventArgs e)
        {
            EventList eventList = new EventList(Context, EventsHelper.EventsList);
            eventList.CurrentTournament = TornamentPlayersHelper.CurrentTournament;
            eventList.ShowDialog();
            if (eventList.AddedEvents.Count > 0)
            {
                foreach (var eventListAddedEvent in eventList.AddedEvents)
                {
                    DrawsPage.DrawsFormer.TabsWorker.CanvasDictionary.Add(eventListAddedEvent, new Dictionary<string, Canvas>());
                }
            }
        }
        private void eventsListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ListPage.playersListView.ItemsSource = PlayersHelper.EventSelectionChangedPlayers(eventsListBox.SelectedItem as Event);
            ListPage.tournamentPlayersListView.ItemsSource =
                TornamentPlayersHelper.EventSelectionChangedTournament(eventsListBox.SelectedItem as Event);
            DrawsPage.EventChangedDrawing();
            ListPage.cmbBoxCategory.SelectedIndex = ListPage.cmbBoxCategory.Items.Count - 1;
        }
        private void spiski_Click(object sender, RoutedEventArgs e)
        {
            changerFrame.Navigate(ListPage);
        }
        private void Setki_OnClick(object sender, RoutedEventArgs e)
        {
            changerFrame.Navigate(DrawsPage);
            DrawsPage.EventChangedDrawing();
        }
        private void DeleteDraw_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((eventsListBox.SelectedItem as Event).IsDrawFormed == true)
                {
                    var eventId = (eventsListBox.SelectedItem as Event).EventId;
                    Context.GamesTournaments.Where(p => p.EventId == eventId).Load();
                    foreach (var gamesTournament in Context.GamesTournaments.Where(p => p.EventId == eventId).ToList())
                    {
                        Context.GamesTournaments.Local.Remove(gamesTournament);
                    }
                    (eventsListBox.SelectedItem as Event).IsDrawFormed = false;
                    DrawsPage.EventChangedDrawing();
                }
                Context.SaveChanges();
            }
            catch
            {
                MessageBox.Show("Не выбрано событие!", "Выбор события", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void aboutProgram_Click(object sender, RoutedEventArgs e)
        {
            about = new AboutControlView();
            vm = (AboutControlViewModel)about.FindResource("ViewModel");
            vm.ApplicationLogo = new BitmapImage(new System.Uri("pack://application:,,,/images/volan.png"));
            vm.Description = "Эта программа позволяет поностью спланировать турнир по бадминтону";

            vm.AdditionalNotes = "Чтобы закрыть это окно - просто переключитесь на другое окно\nАвтор: Безуглый Денис, denys.bezu@gmail.com";
            vm.Title = "Планирование турниров по бадминтону";
            vm.Window.Content = about;
            vm.Window.Show();
        }
        private void Report_OnClick(object sender, RoutedEventArgs e)
        {
            ReportChooser chooser = new ReportChooser(Context, CurrentTournament, EventsHelper);
            chooser.ShowDialog();
        }

        private void SaveCanvas_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string fileName = "";
                if (DrawsPage.TabsWorker.TabControl.SelectedIndex == 0)
                {
                    fileName = (eventsListBox.SelectedItem as Event).Tournament.TournamentName + " " +
                               (eventsListBox.SelectedItem as Event).Category.CategoryName + " " +
                               (eventsListBox.SelectedItem as Event).Sort + "_Основная сетка";
                    ToImageSource(DrawsPage.osn_canvas, fileName);

                }
                else
                {
                    var item = (TabItem)DrawsPage.TabsWorker.TabControl.SelectedItem;
                    string tabName = item.Header.ToString();
                    var canvas = DrawsPage.TabsWorker.CanvasDictionary[eventsListBox.SelectedItem as Event][tabName];
                    fileName = (eventsListBox.SelectedItem as Event).Tournament.TournamentName + " " +
                               (eventsListBox.SelectedItem as Event).Category.CategoryName + " " + (eventsListBox.SelectedItem as Event).Sort + "_" + tabName;
                    ToImageSource(canvas, fileName);
                }
                MessageBox.Show("Сохранено в файле \"" + fileName + ".png" + "\" в папке с программой!");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }


        }
        private void ToImageSource(Canvas canvas, string name)
        {
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)canvas.RenderSize.Width,
                (int)canvas.RenderSize.Height, 96d, 96d, System.Windows.Media.PixelFormats.Pbgra32);
            rtb.Render(canvas);

            var crop = new CroppedBitmap(rtb, new Int32Rect(0, 0, 1500, 2000));

            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(crop));

            using (var fs = System.IO.File.OpenWrite(name + ".png"))
            {
                pngEncoder.Save(fs);
            }
        }
    }
}
