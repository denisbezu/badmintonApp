using System.Collections.Generic;

namespace badmintonDataBase.Models
{
    public class Grade
    {
        public int GradeId { get; set; }
        public string GradeName { get; set; }
        public virtual ICollection<Player> Players { get; set; }
}
}