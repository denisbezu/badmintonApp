using System.Collections.Generic;

namespace badmintonDataBase.Models
{
    public class Union
    {
        public int UnionId { get; set; }
        public string UnionName { get; set; }
        public virtual ICollection<Player> Players { get; set; }
    }
}