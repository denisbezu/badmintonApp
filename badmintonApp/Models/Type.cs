using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace badmintonDataBase.Models
{
    public class Type
    {
        public int TypeId { get; set; }
        [Required, MaxLength(30)]
        public string TypeName { get; set; }
        public virtual ICollection<Event> Events { get; set; }
    }
}