using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using badmintonDataBase.DataAccess;
using badmintonDataBase.Models;

namespace BadmintonWPF.Helpers
{
    public class PlayersHelper
    {
        public ListView DragSource { get; set; }
        public BindingList<Player> PlayersList { get; set; }
        public BadmintonContext Context { get; set; }
        public PlayersHelper(BadmintonContext context)
        {
            Context = context;
        }
        public void PlayersLoad()
        {
            Context.Players.Load();
            PlayersList = new BindingList<Player>(Context.Players.Local.OrderBy(p => p.YearOfBirth).ThenBy(p => p.PlayerSurName).ToList());

        }
        public BindingList<Player> EventSelectionChangedPlayers(Event selectedEvent)
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
            BindingList<Player> source = EventSelectionChangedPlayers(selectedEvent);
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
            PlayersList = null;
            PlayersLoad();
        }
        public void DeleteLeftPlayer(Player player)
        {
            try
            {
                if (player == null) return;
                var result = MessageBox.Show(
                    "Вы действительно хотите удалить игрока \"" + player.PlayerSurName + " " + player.PlayerName +
                    "\"", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result != MessageBoxResult.Yes) return;
                Context.Players.Local.Remove(player);
                Context.SaveChanges();
            }
            catch
            {
                MessageBox.Show("Не удалось удалить запись", "Удаление", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public object GetDataFromListView(ListView source, Point point)
        {
            UIElement element = source.InputHitTest(point) as UIElement;
            if (element != null)
            {
                object data = DependencyProperty.UnsetValue;
                while (data == DependencyProperty.UnsetValue)
                {
                    data = source.ItemContainerGenerator.ItemFromContainer(element);
                    if (data == DependencyProperty.UnsetValue)
                    {
                        element = VisualTreeHelper.GetParent(element) as UIElement;
                    }
                    if (element == source)
                    {
                        return null;
                    }
                }
                if (data != DependencyProperty.UnsetValue)
                {
                    return data;
                }
            }
            return null;
        }

        public BindingList<Player> ComboBoxChangedValue(Event eEvent, Category category)
        {
            BindingList<Player> itemSource = null;

            if (category.CategoryName.Equals("Взрослые") && eEvent.Type.TypeName.Equals("Микст"))
            {
                itemSource = new BindingList<Player>(PlayersList);
            }
            else if(eEvent.Type.TypeName.Equals("Микст") && !category.CategoryName.Equals("Взрослые"))
            {
                itemSource = new BindingList<Player>(PlayersList.Where(p => p.YearOfBirth == int.Parse(category.CategoryName)).ToList());
            }
            else if (!eEvent.Type.TypeName.Equals("Микст") && !category.CategoryName.Equals("Взрослые") && eEvent.Sort.Equals("Юноши"))
            {
                itemSource = new BindingList<Player>(PlayersList.Where(p => p.YearOfBirth == int.Parse(category.CategoryName) && p.Sex.Equals("Мужской")).ToList());
            }
            else if (!eEvent.Type.TypeName.Equals("Микст") && category.CategoryName.Equals("Взрослые") && eEvent.Sort.Equals("Юноши"))
            {
                itemSource = new BindingList<Player>(PlayersList.Where(p => p.Sex.Equals("Мужской")).ToList());
            }
            else if (!eEvent.Type.TypeName.Equals("Микст") && !category.CategoryName.Equals("Взрослые") && eEvent.Sort.Equals("Женщины"))
            {
                itemSource = new BindingList<Player>(PlayersList.Where(p => p.YearOfBirth == int.Parse(category.CategoryName) && p.Sex.Equals("Женский")).ToList());
            }
            else if (!eEvent.Type.TypeName.Equals("Микст") && category.CategoryName.Equals("Взрослые") && eEvent.Sort.Equals("Женщины"))
            {
                itemSource = new BindingList<Player>(PlayersList.Where(p => p.Sex.Equals("Женский")).ToList());
            }



            return itemSource;
        }
    }
}
