﻿namespace badmintonDataBase.Models
{
    public class Event
    {
        public int EventId { get; set; }
        public int TypeId { get; set; }
        public virtual Type Type { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public int TournamentId { get; set; }
        public virtual Tournament Tournament { get; set; }
    }
}