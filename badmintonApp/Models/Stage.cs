using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace badmintonDataBase.Models
{
    public class Stage
    {
        public int StageId { get; set; }
        [Required, MaxLength(20)]
        public string StageName { get; set; }
        public virtual ICollection<GamesTournament> GamesTournaments { get; set; }
    }
}