using System.Collections.Generic;

namespace badmintonDataBase.Models
{
    public class PlayersTeam
    {
        public int PlayersTeamId { get; set; }
        public int TeamsTournamentId { get; set; }
        public virtual TeamsTournament TeamsTournament { get; set; }
        public int PlayerId { get; set; }
        public virtual Player Player { get; set; }
        public virtual ICollection<GamesTournament> FirstPlayer { get; set; }
        public virtual ICollection<GamesTournament> SecondPlayer { get; set; }
    }
}