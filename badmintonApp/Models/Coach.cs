﻿using System.Collections.Generic;

namespace badmintonDataBase.Models
{
    public class Coach
    {
        public int CoachId { get; set; }
        public string CoachName { get; set; }
        public int YearOfBirth { get; set; }
        public int CityId { get; set; }
        public virtual City City { get; set; }
        public int ClubId { get; set; }
        public virtual Club Club { get; set; }
        public virtual ICollection<Player> Players { get; set; }
    }
}