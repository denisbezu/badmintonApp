using System.Collections.Generic;

namespace badmintonDataBase.Models
{
    public class Parameter
    {
        public int ParameterId { get; set; }
        public string ParameterName { get; set; }
        public virtual ICollection<History> Histories { get; set; }
    }
}