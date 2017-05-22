using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace badmintonDataBase.Models
{
    public class Tournament
    {
        public int TournamentId { get; set; }
        [Required]
        public string TournamentName { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime StartDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FinishDate { get; set; }
        public int? JudgeId { get; set; }
        public virtual Judge Judge { get; set; }
        public int? CityId { get; set; }
        public virtual City City { get; set; }
        public virtual ICollection<Event> Events { get; set; }

        [NotMapped]
        public string CityName
        {
            get { return City.CityName; }
        }

        public override string ToString()
        {
            return TournamentName;
        }
    }
}