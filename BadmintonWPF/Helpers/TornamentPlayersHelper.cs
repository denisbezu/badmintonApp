using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using badmintonDataBase.DataAccess;
using badmintonDataBase.Models;
using BadmintonWPF.Views;

namespace BadmintonWPF.Helpers
{
    public class TornamentPlayersHelper
    {
        public BadmintonContext Context { get; set; }
        public BindingList<TeamsTournament> TeamsTournamentList { get; set; }
        public Tournament CurrentTournament { get; set; }
        public TornamentPlayersHelper(BadmintonContext context, Tournament current)
        {
            Context = context;
            CurrentTournament = current;
        }
        public void TeamTournamentsLoad()
        {
            Context.TeamsTournaments.Where(p => p.Event.TournamentId == CurrentTournament.TournamentId).Load();
            TeamsTournamentList = new BindingList<TeamsTournament>(Context.TeamsTournaments.Local.OrderBy(p => p.SeedingNumber).ToList());            
        }
        public BindingList<TeamsTournament> EventSelectionChangedTournament(Event selectedEvent)
        {
            BindingList<TeamsTournament> itemSource;
            itemSource = new BindingList<TeamsTournament>(TeamsTournamentList.Where(p => p.EventId == selectedEvent.EventId).ToList());
            return itemSource;
        }
        public void RefreshTournamentPlayers()
        {
            TeamsTournamentList = null;
            TeamTournamentsLoad();
        }
        public TeamsTournament TournamentTeamAdd(Event eEvent)
        {
            TeamsTournament teamsTournament = new TeamsTournament()
            {
                EventId = eEvent.EventId,
                SeedingNumber = 0
            };
            Context.TeamsTournaments.Local.Add(teamsTournament);
            return teamsTournament;
        }
        public void TournamentSoloPlayerAdd(Player player, Event eEvent)
        {
            bool flag = !(!IsMaxPlayersInList(eEvent) || !IsCorrectCategory(player, eEvent) || !IsNoAdded(player, eEvent));
            if (flag)
            {
                var currentTournament = TournamentTeamAdd(eEvent);
                PlayersTeam playersTeam = new PlayersTeam()
                {
                    TeamsTournamentId = currentTournament.TeamsTournamentId,
                    PlayerId = player.PlayerId,
                };
                Context.TeamsTournaments.Local
                    .FirstOrDefault(p => p.TeamsTournamentId == currentTournament.TeamsTournamentId)
                    .TeamName = player.PlayerSurName + " " + player.PlayerName;
                Context.PlayersTeams.Local.Add(playersTeam);
                Context.SaveChanges();
            }
        }
        public void TournamentDoublesPlayerAdd(Player player, Event eEvent)
        {
            bool flag = !(!IsMaxPlayersInList(eEvent) || !IsCorrectCategory(player, eEvent) || !IsNoAdded(player, eEvent));
            if (flag)
            {
                DoublesAdd doublesAdd = new DoublesAdd(Context, eEvent, player, this);
                doublesAdd.ShowDialog();
                if (doublesAdd.SelectedPlayer == null) return;

                var currentTournament = TournamentTeamAdd(eEvent);
                PlayersTeam playersTeam = new PlayersTeam()
                {
                    TeamsTournamentId = currentTournament.TeamsTournamentId,
                    PlayerId = player.PlayerId,
                };
                PlayersTeam playersTeam2 = new PlayersTeam()
                {
                    TeamsTournamentId = currentTournament.TeamsTournamentId,
                    PlayerId = doublesAdd.SelectedPlayer.PlayerId,
                };
                Context.TeamsTournaments.Local
                    .FirstOrDefault(p => p.TeamsTournamentId == currentTournament.TeamsTournamentId)
                    .TeamName = player.PlayerSurName + "/" + doublesAdd.SelectedPlayer.PlayerSurName;
                Context.PlayersTeams.Local.Add(playersTeam);
                Context.PlayersTeams.Local.Add(playersTeam2);
                Context.SaveChanges();

            }
        }
        public bool IsCorrectCategory(Player player, Event eEvent)
        {
            if (eEvent.Category.CategoryName == "Взрослые") return true;
            if (player.YearOfBirth >= int.Parse(eEvent.Category.CategoryName)) return true;
            MessageBox.Show(
                "Игрок \"" + player.PlayerName + " " + player.PlayerSurName +
                "\" не может быть добавлен, не подходит по категории!",
                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
        public bool IsNoAdded(Player player, Event eEvent)
        {
            if (Context.PlayersTeams.Count(p => p.PlayerId == player.PlayerId &&
                                                p.TeamsTournament.EventId == eEvent.EventId) <= 0) return true;
            MessageBox.Show("Игрок \"" + player.PlayerName + " " + player.PlayerSurName + "\" уже добавлен!",
                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
        public bool IsMaxPlayersInList(Event eEvent)
        {
            int countPlayersInList = Context.TeamsTournaments.Local.Count(p => p.EventId == eEvent.EventId);
            if (countPlayersInList + 1 > int.Parse(eEvent.DrawType))
            {
                MessageBox.Show(
                    "Добавлено уже максимальное количество игроков!",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
        public void TournamentPlayerAdd(Player player, Event eEvent)
        {
            if (eEvent.Type.TypeName.Equals("Одиночка"))
                TournamentSoloPlayerAdd(player, eEvent);
            else 
                TournamentDoublesPlayerAdd(player, eEvent);
        }
        public void DeleteRightPlayer(TeamsTournament player)
        {
            try
            {
                if (player == null) return;
                var result = MessageBox.Show(
                    "Вы действительно хотите удалить \"" + player.TeamName +
                    "\"", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result != MessageBoxResult.Yes) return;
                var playersTeamsToDelete = Context.PlayersTeams
                    .Where(p => p.TeamsTournamentId == player.TeamsTournamentId)
                    .ToList();
                foreach (var playersTeam in playersTeamsToDelete)
                {
                    Context.PlayersTeams.Local.Remove(playersTeam);
                }
                Context.TeamsTournaments.Local.Remove(player);
                Context.SaveChanges();
            }
            catch
            {
                MessageBox.Show("Не удалось удалить запись", "Удаление", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
