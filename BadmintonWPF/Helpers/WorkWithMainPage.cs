using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using badmintonDataBase.DataAccess;
using badmintonDataBase.Models;

namespace BadmintonWPF.Helpers
{
    class WorkWithMainPage
    {
        public BadmintonContext Context { get; set; }
        public BindingList<Event> EventsList { get; set; }
        public BindingList<Player> PlayersList { get; set; }
        public Tournament CurrentTournament { get; set; }
        public WorkWithMainPage(BadmintonContext context, Tournament current)
        {
            Context = context;
            CurrentTournament = current;
        }
        public void PlayersLoad()
        {
            PlayersList = new BindingList<Player>();
            Context.Players.Load();
            PlayersList = Context.Players.Local.ToBindingList();
           
        }
        public void EventsLoad()
        {
            EventsList = new BindingList<Event>();
            Context.Events.Where(p => p.TournamentId == CurrentTournament.TournamentId).Load();
            EventsList = Context.Events.Local.ToBindingList();
            
        }
        public BindingList<Player> EventSelectionChanged(Event selectedEvent)
        {
            BindingList<Player> itemSource;
            if (selectedEvent.Type.TypeName.Equals("Микст"))
            {
                itemSource = new BindingList<Player>(PlayersList);
            }
            else if (selectedEvent.Sort.Equals("Женщины"))
            {
               itemSource = new BindingList<Player>(PlayersList.Where(p => p.Sex.Equals("Женский")).ToList());
            }
            else
            {
                itemSource = new BindingList<Player>(PlayersList.Where(p => p.Sex.Equals("Мужской")).ToList());
            }
            return itemSource;
        }
        public BindingList<Player> Search(Event selectedEvent, string text)
        {
            BindingList<Player> source = EventSelectionChanged(selectedEvent);
            BindingList<Player> newSource;
            newSource = new BindingList<Player>(source.Where(p => p.PlayerSurName.ToLower().Contains(text.ToLower())).ToList());
            return newSource;
        }

        public void AddNewPlayer(Player player)
        {
            if (player != null)
            {
                Context.Players.Local.Add(player);
                Context.SaveChanges();
            }
        }

        public void RefreshPlayers()
        {
            //PlayersList.Clear();
            PlayersList = null;
            PlayersLoad();
        }
    }
}
