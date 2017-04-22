using System;
using System.Collections.Generic;

namespace badmintonDataBase.Models
{
    public class Tournament
    {
        public int TournamentId { get; set; }
        public string TournamentName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public int JudgeId { get; set; }
        public virtual Judge Judge { get; set; }
        public int CityId { get; set; }
        public virtual City City { get; set; }

        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<TeamsTournament> TeamsTournaments { get; set; }
    }
}