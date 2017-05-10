using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using badmintonDataBase.Enums;

namespace badmintonDataBase.Models
{
    public class Event
    {
        public int EventId { get; set; }
        [Required, MaxLength(30)]
        public string DrawType { get; set; }
        public int? TypeId { get; set; }
        [MaxLength(20)]
        public string Sort { get; set; }
        public virtual Type Type { get; set; }
        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public int TournamentId { get; set; }
        public virtual Tournament Tournament { get; set; }
        public bool IsDrawFormed { get; set; }
        public virtual ICollection<TeamsTournament> TeamsTournaments { get; set; }
        public virtual ICollection<GamesTournament> GamesTournaments { get; set; }

    }
}