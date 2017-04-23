using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace badmintonDataBase.Models
{
    public class Judge
    {
        public int JudgeId { get; set; }
        [Required, MaxLength(50)]
        public string JudgeName { get; set; }
        [Required, MaxLength(50)]
        public string JudgeLastName { get; set; }
        public int CityId { get; set; }
        public virtual City City { get; set; }
        public int? YearOfBirth { get; set; }
        public virtual ICollection<Tournament> Tournaments { get; set; }
    }
}