﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using badmintonDataBase.DataAccess;
using badmintonDataBase.Models;
using BadmintonWPF.Helpers;

namespace BadmintonWPF.Views
{
    /// <summary>
    /// Interaction logic for ListPage.xaml
    /// </summary>
    public partial class ListPage : Page
    {
        public MainPage MainPage { get; set; }
        public List<Category> CategoryBoxItems { get; set; }
        public ListPage(MainPage mainPage)
        {
            InitializeComponent();
            CategoryBoxItems = new List<Category>();
            MainPage = mainPage;
            MainPage.Context.Categories.Load();
            var allCategories = MainPage.Context.Categories.Local.OrderBy(p => p.CategoryName).ToList();
            foreach (var category in allCategories)
            {
                CategoryBoxItems.Add(category);
            }
            cmbBoxCategory.ItemsSource = CategoryBoxItems;

        }
        private void PlayersListView_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListView parent = (ListView)sender;
            MainPage.PlayersHelper.DragSource = parent;
            object data = MainPage.PlayersHelper.GetDataFromListView(MainPage.PlayersHelper.DragSource, e.GetPosition(parent));
            parent.SelectedItem = data;
            if (data != null)
            {
                DragDrop.DoDragDrop(parent, data, DragDropEffects.Move);
            }
        }
        private void PlayersListView_OnPreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MainPage.TornamentPlayersHelper.TournamentPlayerAdd(playersListView.SelectedItem as Player, MainPage.eventsListBox.SelectedItem as Event);
            tournamentPlayersListView.ItemsSource = null;
            MainPage.TornamentPlayersHelper.RefreshTournamentPlayers();
            tournamentPlayersListView.ItemsSource = MainPage.TornamentPlayersHelper.EventSelectionChangedTournament(MainPage.eventsListBox.SelectedItem as Event);
        }
        private void TournamentPlayersListView_OnDrop(object sender, DragEventArgs e)
        {
            MainPage.TornamentPlayersHelper.TournamentPlayerAdd(playersListView.SelectedItem as Player, MainPage.eventsListBox.SelectedItem as Event);
            tournamentPlayersListView.ItemsSource = null;
            MainPage.TornamentPlayersHelper.RefreshTournamentPlayers();
            tournamentPlayersListView.ItemsSource = MainPage.TornamentPlayersHelper.EventSelectionChangedTournament(MainPage.eventsListBox.SelectedItem as Event);
        }
        private void TxtSearch_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            playersListView.ItemsSource = MainPage.PlayersHelper.Search(MainPage.eventsListBox.SelectedItem as Event, txtSearch.Text);
        }
        #region LeftList
        private void Menu_add_OnClick(object sender, RoutedEventArgs e)
        {
            PlayerAdd playerAdd = new PlayerAdd();
            playerAdd.ShowDialog();
            if (playerAdd.NewPlayer != null)
            {
                MainPage.Context.Players.Local.Add(playerAdd.NewPlayer);
                MainPage.Context.SaveChanges();
            }
            playersListView.ItemsSource = null;
            MainPage.PlayersHelper.RefreshPlayers();
            playersListView.ItemsSource = MainPage.PlayersHelper.EventSelectionChangedPlayers(MainPage.eventsListBox.SelectedItem as Event);
        }
        private void Menu_delete_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (playersListView.SelectedItem != null)
                {
                    MainPage.PlayersHelper.DeleteLeftPlayer(playersListView.SelectedItem as Player);
                    playersListView.ItemsSource = null;
                    MainPage.PlayersHelper.RefreshPlayers();
                    playersListView.ItemsSource =
                        MainPage.PlayersHelper.EventSelectionChangedPlayers(MainPage.eventsListBox.SelectedItem as Event);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось удалить игрока!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Menu_edit_OnClick(object sender, RoutedEventArgs e)
        {
            PlayerEdit playerEdit = new PlayerEdit(MainPage.Context, playersListView.SelectedItem as Player);
            playerEdit.ShowDialog();

        }
        private void Menu_delete_right_OnClick(object sender, RoutedEventArgs e)
        {
            MainPage.TornamentPlayersHelper.DeleteRightPlayer(tournamentPlayersListView.SelectedItem as TeamsTournament);
            tournamentPlayersListView.ItemsSource = null;
            MainPage.TornamentPlayersHelper.RefreshTournamentPlayers();
            tournamentPlayersListView.ItemsSource = MainPage.TornamentPlayersHelper.EventSelectionChangedTournament(MainPage.eventsListBox.SelectedItem as Event);
        }

        #endregion
        private void BtnSeed_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Seed seed = new Seed(MainPage.Context, MainPage.eventsListBox.SelectedItem as Event);
                seed.ShowDialog();
                tournamentPlayersListView.ItemsSource = null;
                MainPage.TornamentPlayersHelper.RefreshTournamentPlayers();
                tournamentPlayersListView.ItemsSource =
                    MainPage.TornamentPlayersHelper.EventSelectionChangedTournament(
                        MainPage.eventsListBox.SelectedItem as Event);
            }
            catch
            {
                MessageBox.Show("Возникла ошибка, возможно не выбрано событие", "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
        private List<int> SelectedNum(Event eEvent)
        {
            switch (int.Parse(eEvent.DrawType))
            {
                case 64: return MainPage.Nums.Nums64;
                case 32: return MainPage.Nums.Nums32;
                case 16: return MainPage.Nums.Nums16;
                case 8: return MainPage.Nums.Nums8;
                default: return new List<int>();
            }
        }
        private void DrawsForm_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedEvent = MainPage.Context.Events.Local
                    .Where(p => p.EventId == (MainPage.eventsListBox.SelectedItem as Event).EventId).FirstOrDefault();
                if (selectedEvent.IsDrawFormed == false)
                {
                    MainPage.DrawsPage.DrawsFormer.FirstRoundGamesFormer(
                        SelectedNum(MainPage.eventsListBox.SelectedItem as Event));
                    //if (selectedEvent.Type.TypeName.Equals("Одиночка"))
                    //{
                    int n = int.Parse(selectedEvent.DrawType) / 2, i = 0;
                    while (n > 2)
                    {
                        MainPage.DrawsPage.DrawsFormer.GamesForLoosersFormer(selectedEvent, n,
                            MainPage.DrawsPage.DrawsFormer.ForPlaceCalculate(MainPage.DrawsPage.DrawsFormer.TabsWorker
                                .CanvasDictionary[selectedEvent].ElementAt(i).Key));
                        n /= 2;
                        i++;
                    }
                    //}
                }
                if (!selectedEvent.IsDrawFormed)
                {
                    MessageBox.Show(
                        "Сетка для события " + selectedEvent.Sort + " " + selectedEvent.Category.CategoryName + "[" +
                        selectedEvent.DrawType + "] (" + selectedEvent.Type.TypeName + ") сформирована",
                        "Создание сетки",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    selectedEvent.IsDrawFormed = true;
                }
                else
                {
                    MessageBox.Show("Сетка уже была сформирована раньше!", "Создание сетки", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Возникла ошибка, возможно не выбрано событие", "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            MainPage.Context.SaveChanges();
        }

        private void cmbBoxCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            playersListView.ItemsSource = MainPage.PlayersHelper.ComboBoxChangedValue(MainPage.eventsListBox.SelectedItem as Event,
                cmbBoxCategory.SelectedValue as Category);
        }
    }
}
