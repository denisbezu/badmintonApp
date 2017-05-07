using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace badmintonDataBase.Models
{
    public class GamesTournament
    {
        public int GamesTournamentId { get; set; }
        public int TeamsTournament1Id { get; set; }
        public virtual TeamsTournament TeamsTournament1 { get; set; }
        public int TeamsTournament2Id { get; set; }
        public virtual TeamsTournament TeamsTournament2 { get; set; }
        [Required, MaxLength(50)]
        public string Score { get; set; }
        public int? StageId { get; set; }
        public virtual Stage Stage { get; set; }

    }
}