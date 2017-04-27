using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace badmintonDataBase.Models
{
    public class GamesTournament
    {
        public int GamesTournamentId { get; set; }
        public int PlayersTeam1Id { get; set; }
        public virtual PlayersTeam PlayersTeam1 { get; set; }
        public int PlayersTeam2Id { get; set; }
        public virtual PlayersTeam PlayersTeam2 { get; set; }
        [Required, MaxLength(50)]
        public string Score { get; set; }
        public int? StageId { get; set; }
        public virtual Stage Stage { get; set; }

    }
}