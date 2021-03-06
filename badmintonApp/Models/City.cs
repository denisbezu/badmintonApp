﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using badmintonDataBase.DataAccess;

namespace badmintonDataBase.Models
{
    public class City
    {
        public int CityId { get; set; }
        [Required, MaxLength(30)]
        public string CityName { get; set; }
        public virtual ICollection<Judge> Judges { get; set; }
        public virtual ICollection<Club> Clubs { get; set; }
        public virtual ICollection<Coach> Coaches { get; set; }
        public virtual ICollection<Player> Players { get; set; }
        public virtual ICollection<Tournament> Tournaments { get; set; }
        public override string ToString()
        {
            return CityName;
        }
    }
}