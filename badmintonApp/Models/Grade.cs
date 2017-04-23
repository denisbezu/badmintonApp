using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace badmintonDataBase.Models
{
    public class Grade
    {
        public int GradeId { get; set; }
        [Required, MaxLength(20)]
        public string GradeName { get; set; }
        public virtual ICollection<Player> Players { get; set; }
    }
}