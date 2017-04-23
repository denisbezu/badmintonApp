using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace badmintonDataBase.Models
{
    public class Parameter
    {
        public int ParameterId { get; set; }
        [Required, MaxLength(25)]
        public string ParameterName { get; set; }
        public virtual ICollection<History> Histories { get; set; }
    }
}