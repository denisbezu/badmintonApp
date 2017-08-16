using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using badmintonDataBase.Models;

namespace BadmintonWPF.Helpers.DrawsHelpers
{
    public class TabsWorker
    {
        public Dictionary<Event, Dictionary<string, Canvas>> CanvasDictionary { get; set; }
        public TabControl TabControl { get; set; }

        public TabsWorker(TabControl tabControl)
        {
            CanvasDictionary = new Dictionary<Event, Dictionary<string, Canvas>>();
            TabControl = tabControl;
        }

        private void Make_Tab(Event eEvent, string header)
        {
            TabItem t = new TabItem();
            t.Header = header;
            ScrollViewer sw = new ScrollViewer();
            sw.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            t.Content = sw;
            bool f = true;
            foreach (var item in CanvasDictionary)
            {
                if (item.Key == eEvent)
                    if (item.Value.ContainsKey(header))
                    {
                        f = false;
                    }
            }
            if (f)
            {
                Canvas canvas = new Canvas();
                canvas.Background = Brushes.White;
                canvas.Height = 2000;
                canvas.Width = 2000;
                sw.Content = canvas;
                CanvasDictionary[eEvent].Add(header, canvas);
                TabControl.Items.Add(t);
            }
            else
            {
                sw.Content = CanvasDictionary[eEvent][header];
                TabControl.Items.Add(t);
            }

        }
        public void TabsMaker(Event eEvent)
        {
            try
            {
                foreach (var item in CanvasDictionary)
                {
                    foreach (var pair in item.Value)
                        pair.Value.Children.Clear();
                }
                if (int.Parse(eEvent.DrawType) == 64)
                {
                    Make_Tab(eEvent, "За 33 место");
                    Make_Tab(eEvent, "За 17 место");
                    Make_Tab(eEvent, "За 9 место");
                    Make_Tab(eEvent, "За 5 место");
                }
                if (int.Parse(eEvent.DrawType) == 32)
                {
                    Make_Tab(eEvent, "За 17 место");
                    Make_Tab(eEvent, "За 9 место");
                    Make_Tab(eEvent, "За 5 место");
                }
                if (int.Parse(eEvent.DrawType) == 16)
                {
                    Make_Tab(eEvent, "За 9 место");
                    Make_Tab(eEvent, "За 5 место");
                }
                if (int.Parse(eEvent.DrawType) == 8)
                {
                    Make_Tab(eEvent, "За 5 место");
                }
            }
            catch
            {
                MessageBox.Show("Возникла ошибка, возможно не выбрано никакое событие", "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public void TabsCreatorRemover(Event eEvent)
        {
            while (TabControl.Items.Count > 1)
                TabControl.Items.RemoveAt(TabControl.Items.Count - 1);
            TabsMaker(eEvent);
        }
    }
}
