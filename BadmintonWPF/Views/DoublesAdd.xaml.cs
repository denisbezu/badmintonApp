using System;
using System.Collections.Generic;
using System.Linq;
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
using BadmintonWPF.Helpers;

namespace BadmintonWPF.Views
{
    /// <summary>
    /// Interaction logic for DoublesAdd.xaml
    /// </summary>
    public partial class DoublesAdd : Window
    {
        public PlayersHelper PlayersHelper { get; set; }
        public Event SelectedEvent { get; set; }
        public BadmintonContext Context { get; set; }
        public Player SelectedPlayer { get; set; }
        public Player CurrentPlayer { get; set; }
        public TornamentPlayersHelper TornamentPlayersHelper { get; set; }
        public DoublesAdd(BadmintonContext context, Event selectedEvent, Player player, TornamentPlayersHelper tornamentPlayersHelper)
        {
            InitializeComponent();
            Context = context;
            CurrentPlayer = player;
            TitleChanger(player, selectedEvent);
            TornamentPlayersHelper = tornamentPlayersHelper;
            PlayersHelper = new PlayersHelper(Context);
            PlayersHelper.PlayersLoad();
            SelectedEvent = selectedEvent;
            if(!SelectedEvent.Type.TypeName.Equals("Микст"))
                playersListView.ItemsSource = PlayersHelper.EventSelectionChangedPlayers(SelectedEvent).Where(p => p.PlayerSurName != player.PlayerSurName && p.PlayerName != player.PlayerName);
            else if(player.Sex.Equals("Мужской"))
                playersListView.ItemsSource = PlayersHelper.EventSelectionChangedPlayers(SelectedEvent).Where(p => p.Sex.Equals("Женский"));
            else
                playersListView.ItemsSource = PlayersHelper.EventSelectionChangedPlayers(SelectedEvent).Where(p => p.Sex.Equals("Мужской"));
        }
        public void TitleChanger(Player player, Event eEvent)
        {
            if (eEvent.Type.TypeName.Equals("Пара"))
            {
                Title = "Выберите пару для игрока " + player.PlayerSurName + " " + player.PlayerName;
            }
            else
            {
                Title = "Выберите микст для игрока " + player.PlayerSurName + ' ' + player.PlayerSurName;
            }
        }
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (playersListView.SelectedItems.Count > 0)
            {
                SelectedPlayer = new Player();
                SelectedPlayer = playersListView.SelectedItem as Player;
                if (!(!TornamentPlayersHelper.IsCorrectCategory(SelectedPlayer, SelectedEvent) ||
                      !TornamentPlayersHelper.IsNoAdded(SelectedPlayer, SelectedEvent)))
                   Close();
            }
            else
            {
                MessageBox.Show("Выберите второго игрока!", "Не выбран игрок", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            SelectedPlayer = null;
            Close();
        }
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!SelectedEvent.Type.TypeName.Equals("Микст"))
                playersListView.ItemsSource = PlayersHelper.Search(SelectedEvent, txtSearch.Text).Where(p => p.PlayerSurName != CurrentPlayer.PlayerSurName && p.PlayerName != CurrentPlayer.PlayerName);
            else if (CurrentPlayer.Sex.Equals("Мужской"))
                playersListView.ItemsSource = PlayersHelper.Search(SelectedEvent, txtSearch.Text).Where(p => p.Sex.Equals("Женский"));
            else
                playersListView.ItemsSource = PlayersHelper.Search(SelectedEvent, txtSearch.Text).Where(p => p.Sex.Equals("Мужской"));
            
        }
    }
}
