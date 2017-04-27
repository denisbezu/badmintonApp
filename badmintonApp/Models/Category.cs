using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace badmintonDataBase.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Required, MaxLength(20)]
        public string CategoryName { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public override string ToString()
        {
            return CategoryName;
        }
    }
}