using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace badmintonDataBase.Models
{
    public class Union
    {
        public int UnionId { get; set; }
        [Required, MaxLength(25)]
        public string UnionName { get; set; }
        public virtual ICollection<Player> Players { get; set; }
    }
}