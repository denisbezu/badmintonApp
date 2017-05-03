using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace badmintonDataBase.Models
{
    public class Club
    {
        public int ClubId { get; set; }
        [Required, MaxLength(30)]
        public string ClubName { get; set; }
        public int? CityId { get; set; }
        public virtual City City { get; set; }
        public virtual ICollection<Coach> Coaches { get; set; }
        public virtual ICollection<Player> Players { get; set; }
        public override string ToString()
        {
            return ClubName;
        }
    }
}