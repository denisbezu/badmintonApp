using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using badmintonDataBase.Models;
using BadmintonWPF.Helpers;
using BadmintonWPF.Helpers.DrawsHelpers;

namespace BadmintonWPF.Views
{
    /// <summary>
    /// Interaction logic for DrawsPage.xaml
    /// </summary>
    public partial class DrawsPage : Page
    {
        public DrawsFormer DrawsFormer { get; set; }
        public MainPage MainPage { get; set; }
        public TabsWorker TabsWorker { get; set; }
        public DrawsPage(MainPage mainPage)
        {
            InitializeComponent();
            MainPage = mainPage;
            TabsWorker = new TabsWorker(tab_setki);
            DrawsFormer = new DrawsFormer(mainPage.Context, TabsWorker);
            // при добавление в событие нужно еще добавлять сюда

            foreach (var eevent in MainPage.eventsListBox.Items)
            {
                //if ((eevent as Event).Type.TypeName.Equals("Одиночка"))
                    DrawsFormer.TabsWorker.CanvasDictionary.Add(eevent as Event, new Dictionary<string, Canvas>());
            }

        }
        public void EventChangedDrawing()
        {
            DrawsFormer.SelectedEvent = MainPage.eventsListBox.SelectedItem as Event;
            DrawsFormer.SelectedCanvas = MainPage.DrawsPage.osn_canvas;
            DrawsFormer.EventChanged(osn_canvas);
        }
    }
}
