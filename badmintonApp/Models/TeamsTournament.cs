using System.Collections.Generic;

namespace badmintonDataBase.Models
{
    public class TeamsTournament
    {
        public int TeamsTournamentId { get; set; }
        public int EventId { get; set; }
        public virtual Event Event { get; set; }
        public virtual ICollection<PlayersTeam> PlayersTeams { get; set; }
    }
}