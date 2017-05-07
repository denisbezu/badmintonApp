using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using badmintonDataBase.DataAccess;
using badmintonDataBase.Models;

namespace BadmintonWPF.Helpers
{
    public class EventsHelper
    {
        public BindingList<Event> EventsList { get; set; }
        public Tournament CurrentTournament { get; set; }
        public BadmintonContext Context { get; set; }

        public EventsHelper(BadmintonContext context, Tournament tournament)
        {
            Context = context;
            CurrentTournament = tournament;
        }
        public void EventsLoad()
        {
            EventsList = new BindingList<Event>();
            Context.Events.Where(p => p.TournamentId == CurrentTournament.TournamentId).Load();
            EventsList = Context.Events.Local.ToBindingList();
        }

    }
}
