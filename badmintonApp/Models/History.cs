using System;

namespace badmintonDataBase.Models
{
    public class History
    {
        public int HistoryId { get; set; }
        public int? PlayerId { get; set; }
        public virtual Player Player { get; set; }
        public DateTime Date { get; set; }
        public int? ParameterId { get; set; }
        public virtual Parameter Parameter { get; set; }
        public int NewValue { get; set; }
    }
}