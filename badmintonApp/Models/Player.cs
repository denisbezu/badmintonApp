using System.Collections.Generic;

namespace badmintonDataBase.Models
{
    public class Player
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public string PlayerSurName { get; set; }
        public int YearOfBirth { get; set; }
        public int GradeId { get; set; }
        public virtual Grade Grade { get; set; }
        public int CityId { get; set; }
        public virtual City City { get; set; }
        public int ClubId { get; set; }
        public virtual Club Club { get; set; }
        public int UnionId { get; set; }
        public virtual Union Union { get; set; }
        public int CoachId { get; set; }
        public virtual Coach Coach { get; set; }
        public virtual ICollection<History> Histories { get; set; }
        public virtual ICollection<PlayersTeam> PlayersTeams { get; set; }

    }
}