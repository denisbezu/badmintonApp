using System.Collections;
using System.Collections.Generic;

namespace badmintonDataBase.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? TypeId { get; set; }
        public virtual Type Type { get; set; }
        public virtual ICollection<Event> Events { get; set; }
    }
}