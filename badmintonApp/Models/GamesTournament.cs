using System.ComponentModel.DataAnnotations.Schema;

namespace badmintonDataBase.Models
{
    public class GamesTournament
    {
        public int GameSingleId { get; set; }
        public int PlayersTeam1Id { get; set; }
        [ForeignKey("PlayersTeam1Id")]
        public virtual PlayersTeam PlayersTeam1 { get; set; }
        public int PlayersTeam2Id { get; set; }
        [ForeignKey("PlayersTeam2Id")]
        public virtual PlayersTeam PlayersTeam2 { get; set; }
        public string Scrore { get; set; }
        public int StageId { get; set; }
        public virtual Stage Stage { get; set; }

    }
}