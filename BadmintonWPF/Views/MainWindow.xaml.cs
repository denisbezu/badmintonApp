using System.Windows;

namespace BadmintonWPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Cities cities = new Cities();
            cities.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Grades grades = new Grades();
            grades.ShowDialog();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Categories categories = new Categories();
            categories.ShowDialog();
        }

        private void Union_OnClick(object sender, RoutedEventArgs e)
        {
            Unions unions = new Unions();
            unions.ShowDialog();
        }

        private void Clubs_OnClick(object sender, RoutedEventArgs e)
        {
            Clubs clubs = new Clubs();
            clubs.ShowDialog();
        }

        private void JudgesAdd_OnClick(object sender, RoutedEventArgs e)
        {
            JudgesAdd judgesAdd = new JudgesAdd();
            judgesAdd.ShowDialog();
        }

        private void JudgesList_Click(object sender, RoutedEventArgs e)
        {
            JudgesList judgesList = new JudgesList();
            judgesList.ShowDialog();
        }

        private void TournamentChooser_OnClick(object sender, RoutedEventArgs e)
        {
            TournamentChooser tournamentChooser = new TournamentChooser();
            tournamentChooser.ShowDialog();
        }

        private void Main_page_OnClick(object sender, RoutedEventArgs e)
        {
            MainPage mainPage = new MainPage();
            mainPage.ShowDialog();
        }
    }
}
