using System.Collections.Generic;

namespace badmintonDataBase.Models
{
    public class Stage
    {
        public int StageId { get; set; }
        public string StageName { get; set; }
        public virtual ICollection<GamesTournament> GameSingles { get; set; }
    }
}