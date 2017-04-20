using System.Collections.Generic;

namespace badmintonDataBase.Models
{
    public class Type
    {
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Event> Events { get; set; }
    }
}