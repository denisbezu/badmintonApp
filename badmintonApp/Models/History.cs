using System;
using System.ComponentModel.DataAnnotations;

namespace badmintonDataBase.Models
{
    public class History
    {
        public int HistoryId { get; set; }
        public int PlayerId { get; set; }
        public virtual Player Player { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }
        public int ParameterId { get; set; }
        public virtual Parameter Parameter { get; set; }
        public int NewValue { get; set; }
    }
}