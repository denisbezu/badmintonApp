using System.Collections.Generic;

namespace badmintonDataBase.Models
{
    public class TeamsTournament
    {
        public int TeamsTournamentId { get; set; }
        public int TournamentId { get; set; }
        public virtual Tournament Tournament { get; set; }
        public int TypeId { get; set; }
        public virtual Type Type { get; set; }
        public virtual ICollection<PlayersTeam> PlayersTeams { get; set; }
    }
}