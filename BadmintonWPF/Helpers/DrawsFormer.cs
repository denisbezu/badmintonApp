using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using badmintonDataBase.DataAccess;
using badmintonDataBase.Models;
using BadmintonWPF.Views;

namespace BadmintonWPF.Helpers
{
    public class DrawsFormer
    {
        //вместо string возможно event
        public Dictionary<string, Dictionary<string, Canvas>> CanvasDictionary { get; set; }
        public Event SelectedEvent { get; set; }
        public Canvas SelectedCanvas { get; set; }
        public Dictionary<Event, List<Rectangle>> DictRectangles { get; set; }
        public List<string> Circles { get; set; }
        public BadmintonContext Context { get; set; }
        public DrawsFormer(BadmintonContext context)
        {
            SelectedEvent = new Event();
            CanvasDictionary = new Dictionary<string, Dictionary<string, Canvas>>();
            Circles = new List<string>();
            DictRectangles = new Dictionary<Event, List<Rectangle>>();
            Context = context;
        }
        private void CirclesDrawing(int numberPers, Canvas canvas)
        {
            Circles.Add("Первый круг");
            Circles.Add("Второй круг");
            Circles.Add("Третий круг");
            Circles.Add("Четвертьфинал");
            Circles.Add("Полуфинал");
            Circles.Add("Финал");
            Circles.Add("Победитель");
            int left = 20, top = 0;
            if (numberPers == 64)
            {
                foreach (var i in Circles)
                {
                    Label l = new Label();
                    l.Content = i;
                    l.FontWeight = FontWeights.Bold;
                    Canvas.SetTop(l, top);
                    Canvas.SetLeft(l, left);
                    canvas.Children.Add(l);
                    left += 120;
                    if (i == "Победитель")
                        break;
                }
            }
            else if (numberPers == 32)
            {
                foreach (var i in Circles)
                {
                    if (i.Contains("Третий"))
                        continue;
                    Label l = new Label();
                    l.FontWeight = FontWeights.Bold;
                    l.Content = i;
                    Canvas.SetTop(l, top);
                    Canvas.SetLeft(l, left);
                    canvas.Children.Add(l);
                    left += 120;
                    if (i == "Победитель")
                        break;
                }
            }
            else if (numberPers == 16)
            {
                foreach (var i in Circles)
                {
                    if (i.Contains("Третий") || i.Contains("Второй"))
                        continue;
                    Label l = new Label();
                    l.FontWeight = FontWeights.Bold;
                    l.Content = i;
                    Canvas.SetTop(l, top);
                    Canvas.SetLeft(l, left);
                    canvas.Children.Add(l);
                    left += 120;
                    if (i == "Победитель")
                        break;
                }
            }
            else if (numberPers == 8)
            {
                for (int i = 3; i < Circles.Count; i++)
                {
                    Label l = new Label();
                    l.Content = Circles[i];
                    l.FontWeight = FontWeights.Bold;
                    Canvas.SetTop(l, top);
                    Canvas.SetLeft(l, left);
                    canvas.Children.Add(l);
                    left += 120;
                    if (Circles[i] == "Победитель")
                        break;
                }
            }
            else if (numberPers == 4)
            {
                for (int i = 4; i < Circles.Count; i++)
                {
                    Label l = new Label();
                    l.Content = Circles[i];
                    l.FontWeight = FontWeights.Bold;
                    Canvas.SetTop(l, top);
                    Canvas.SetLeft(l, left);
                    canvas.Children.Add(l);
                    left += 120;
                    if (Circles[i] == "Победитель")
                        break;
                }
            }
        }
        public void CanvasDrawing(Event eEvent, Canvas canvas)
        {
            CirclesDrawing(int.Parse(eEvent.DrawType), canvas);
            LinesDrawing(int.Parse(eEvent.DrawType), canvas);

            if (eEvent.IsDrawFormed == true)
            {
                //FirstStageLabelsDrawing(eEvent, canvas);
                AllRoundsLabelsDrawing(eEvent, canvas);
                FirstStageLabelsDrawing(eEvent, canvas);
                RectanglesDrawing(eEvent, canvas);
            }
        }
        public void EventChanged(Event eEvent, Canvas canvas)
        {
            canvas.Children.Clear();
            #region Commented

            //while (tab_setki.Items.Count > 1)
            //    tab_setki.Items.RemoveAt(tab_setki.Items.Count - 1);

            //foreach (var item in canvases)
            //{
            //    foreach (var pair in item.Value)
            //        pair.Value.Children.Clear();
            //}

            //if (!spiski_categories.SelectedItem.ToString().Contains(" П") && !spiski_categories.SelectedItem.ToString().Contains(" М"))
            //{
            //    foreach (var item in Settings.events)
            //    {
            //        if (item.Name == spiski_categories.SelectedItem.ToString() && item.Type == "Основная" && item.Provoditsya == "проводится" /*&& Dict_former_setki.Dic[spiski_categories.SelectedItem.ToString()] == true*/)
            //        {
            //            if (item.Size_osn == "32")
            //            {
            //                Make_Tab("За 17 место");
            //                Make_Tab("За 9 место");
            //                Make_Tab("За 5 место");
            //            }
            //            if (item.Size_osn == "16")
            //            {
            //                Make_Tab("За 9 место");
            //                Make_Tab("За 5 место");
            //            }
            //            if (item.Size_osn == "8")
            //            {
            //                Make_Tab("За 5 место");
            //            }
            //        }
            //    }
            //}

            #endregion
            CanvasDrawing(eEvent, canvas);
        }
        private void LinesDrawing(int numberPers, Canvas canvas)
        {
            int x1, x2, y1, y2, y_mnozh = 1;
            int x1_nach = 20, x2_nach = 140, y1_nach = 50, y2_nach = 50;
            int x_hor_nach = 140, y1_hor_nach = 50, y2_hor_nach = 80, x_hor, y1_hor, y2_hor;
            while (numberPers > 0)
            {
                x1 = x1_nach;
                x2 = x2_nach;
                y1 = y1_nach;
                y2 = y2_nach;
                x_hor = x_hor_nach;
                y1_hor = y1_hor_nach;
                y2_hor = y2_hor_nach;
                for (int i = 0; i < numberPers; i++)
                {
                    Line l1 = new Line();
                    l1.Stroke = Brushes.Black;
                    l1.StrokeThickness = 1;
                    l1.X1 = x1;
                    l1.X2 = x2;
                    l1.Y1 = y1;
                    l1.Y2 = y2;
                    canvas.Children.Add(l1);
                    y1 += 30 * y_mnozh;
                    y2 += 30 * y_mnozh;
                }
                for (int i = 0; i < numberPers / 2; i++)
                {
                    Line l1 = new Line();
                    l1.Stroke = Brushes.Black;
                    l1.StrokeThickness = 1;
                    l1.X1 = x_hor;
                    l1.X2 = x_hor;
                    l1.Y1 = y1_hor;
                    l1.Y2 = y2_hor;
                    canvas.Children.Add(l1);
                    y1_hor += 60 * y_mnozh;
                    y2_hor += 60 * y_mnozh;
                }
                x1_nach += 120;
                x2_nach += 120;
                y1_nach += 15 * y_mnozh;
                y2_nach += 15 * y_mnozh;
                x_hor_nach += 120;
                y1_hor_nach += 15 * y_mnozh;
                y2_hor_nach += 45 * y_mnozh;
                y_mnozh *= 2;
                numberPers /= 2;
            }
        }
        private void RectanglesDrawing(Event eEvent, Canvas canvas)
        {
            int numberPers = int.Parse(eEvent.DrawType);
            int left = 140, top_nach = 50, top, mnozh_top = 1;
            numberPers /= 2;
            if (!DictRectangles.ContainsKey(eEvent))
                DictRectangles.Add(eEvent, new List<Rectangle>());
            else
                DictRectangles[eEvent].Clear();
            while (numberPers > 0)
            {
                top = top_nach;
                for (int i = 0; i < numberPers; i++)
                {
                    DictRectangles[eEvent].Add(new Rectangle());
                    DictRectangles[eEvent].Last().Height = 30;
                    DictRectangles[eEvent].Last().Width = 120;
                    DictRectangles[eEvent].Last().Fill = Brushes.Transparent;
                    Canvas.SetTop(DictRectangles[eEvent].Last(), top);
                    Canvas.SetLeft(DictRectangles[eEvent].Last(), left);
                    canvas.Children.Add(DictRectangles[eEvent].Last());
                    DictRectangles[eEvent].Last().MouseEnter += (sender, args) => (sender as Rectangle).Fill =
                        new SolidColorBrush(Color.FromArgb(45, 15, 255, 5));
                    DictRectangles[eEvent].Last().MouseLeave +=
                        (sender, args) => (sender as Rectangle).Fill = Brushes.Transparent;
                    DictRectangles[eEvent].Last().PreviewMouseDown += OnPreviewMouseDown;
                    top += 60 * mnozh_top;
                }
                left += 120;
                top_nach += 30 * mnozh_top;
                mnozh_top *= 2;
                numberPers /= 2;
            }
        }
        private void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Context.GamesTournaments.Where(p => p.EventId == SelectedEvent.EventId).Load();
            if (e.ClickCount != 2) return;

            //matchInfo.ShowDialog();
            int kolVoLudey = int.Parse(SelectedEvent.DrawType);
            for (int i = 0; i < kolVoLudey - 1; i++)
            {
                if ((sender as Rectangle) == DictRectangles[SelectedEvent][i])
                {
                    int placeInDraw = CalculatePlaceInDraw(i, kolVoLudey);
                    int round = CalculateRoundForRectangle(i, kolVoLudey);
                    int whichPlayer;
                    whichPlayer = i % 2 == 0 ? 1 : 2;
                    int precedentNumber = CalculatePrecedentGame(i, placeInDraw);
                    try
                    {
                        var team1 = Context.GamesTournaments.Local
                            .FirstOrDefault(p => p.ForPlace == 1 && p.PlaceInDraw == precedentNumber &&
                                                 p.StageId == round - 1 && p.EventId == SelectedEvent.EventId).TeamsTournament1;
                        var team2 = Context.GamesTournaments.Local
                            .FirstOrDefault(p => p.ForPlace == 1 && p.PlaceInDraw == precedentNumber &&
                                                 p.StageId == round - 1 && p.EventId == SelectedEvent.EventId).TeamsTournament2;
                        if (team1 != null && team2 != null)
                        {
                            MatchInfo matchInfo = new MatchInfo();
                            matchInfo.igrok1.Content = team1.TeamName;
                            matchInfo.igrok2.Content = team2.TeamName;
                            matchInfo.radioButton1.Content = team1.TeamName;
                            matchInfo.radioButton2.Content = team2.TeamName;
                            matchInfo.ShowDialog();
                            if (matchInfo.WinnerChanged)
                            {
                                int left = 20, top_nach = 30, top, y_mnozh = 1;
                                for (int y = 0; y < round - CalculateFirstStageId(SelectedEvent); y++)
                                {
                                    left += 120;
                                    top_nach += 15 * y_mnozh;
                                    y_mnozh *= 2;
                                }
                                top = top_nach;
                                int goUntil;
                                if (whichPlayer == 1)
                                    goUntil = placeInDraw * 2 - 1;
                                else
                                    goUntil = placeInDraw * 2;
                                for (int j = 0; j < goUntil - 1; j++)
                                {
                                    top += 30 * y_mnozh;
                                }
                                if (matchInfo.radioButton1.IsChecked == true)
                                {
                                    Context.GamesTournaments.Load();
                                    OneLabelDrawing(SelectedCanvas, top, team1.TeamName, left);
                                    if (whichPlayer == 1)
                                    {
                                        Context.GamesTournaments.Local
                                            .FirstOrDefault(p => p.ForPlace == 1 && p.PlaceInDraw == placeInDraw &&
                                                                 p.StageId == round && p.EventId ==
                                                                 SelectedEvent.EventId)
                                            .TeamsTournament1Id = team1.TeamsTournamentId;
                                    }
                                    else
                                    {
                                        Context.GamesTournaments.Local
                                            .FirstOrDefault(p => p.ForPlace == 1 && p.PlaceInDraw == placeInDraw &&
                                                                 p.StageId == round && p.EventId == SelectedEvent.EventId).TeamsTournament2Id = team1.TeamsTournamentId;
                                    }
                                }
                                else
                                {
                                    OneLabelDrawing(SelectedCanvas, top, team2.TeamName, left);
                                    if (whichPlayer == 1)
                                        Context.GamesTournaments.Local
                                            .FirstOrDefault(p => p.ForPlace == 1 && p.PlaceInDraw == placeInDraw &&
                                                                 p.StageId == round && p.EventId == SelectedEvent.EventId).TeamsTournament1Id = team2.TeamsTournamentId;
                                    else
                                    {
                                        Context.GamesTournaments.Local
                                            .FirstOrDefault(p => p.ForPlace == 1 && p.PlaceInDraw == placeInDraw &&
                                                                 p.StageId == round && p.EventId == SelectedEvent.EventId).TeamsTournament2Id = team2.TeamsTournamentId;
                                    }
                                }

                                OneLabelDrawing(SelectedCanvas, top, matchInfo.Winner, left);
                                matchInfo.WinnerChanged = false;
                            }
                        }
                        else
                            throw new Exception();
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("Невозможно добавить результат", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    Context.SaveChanges();

                }

            }
        }
        public void FirstRoundGamesFormer(Event eEvent, List<int> numsForDraw)
        {
            Context.GamesTournaments.Load(); // нужно будет сделать Where
            Context.TeamsTournaments.Load();
            for (int i = 1; i <= numsForDraw.Count; i += 2)
            {
                GamesTournament gamesTournament = new GamesTournament();
                gamesTournament.ForPlace = 1;
                gamesTournament.PlaceInDraw = (i / 2) + 1;
                TeamsTournament teamsTournament1 = Context.TeamsTournaments.Local.FirstOrDefault
                    (p => p.EventId == eEvent.EventId && p.SeedingNumber == numsForDraw[i - 1]);
                if (teamsTournament1 != null)
                    gamesTournament.TeamsTournament1Id = teamsTournament1.TeamsTournamentId;
                TeamsTournament teamsTournament2 = Context.TeamsTournaments.Local.FirstOrDefault
                    (p => p.EventId == eEvent.EventId && p.SeedingNumber == numsForDraw[i]);
                if (teamsTournament2 != null)
                    gamesTournament.TeamsTournament2Id = teamsTournament2.TeamsTournamentId;
                gamesTournament.StageId = CalculateFirstStageId(eEvent);
                gamesTournament.EventId = eEvent.EventId;
                Context.GamesTournaments.Local.Add(gamesTournament);
            }
            int numberPlace = CalculateFirstStageId(eEvent) + 1, draw = numsForDraw.Count / 2;
            while (numberPlace <= 7)
            {
                for (int i = 1; i <= draw; i += 2)
                {
                    GamesTournament gamesTournament = new GamesTournament();
                    gamesTournament.ForPlace = 1;
                    gamesTournament.PlaceInDraw = (i / 2) + 1;
                    TeamsTournament teamsTournament1 = null;
                    TeamsTournament teamsTournament2 = null;
                    gamesTournament.StageId = numberPlace;
                    gamesTournament.EventId = eEvent.EventId;
                    Context.GamesTournaments.Local.Add(gamesTournament);
                }
                numberPlace++;
                draw /= 2;
            }

            Context.SaveChanges();
        }
        //добавить счет для 2 круга
        public void FirstStageLabelsDrawing(Event eEvent, Canvas canvas)
        {
            var firstStage = CalculateFirstStageId(eEvent);
            Context.GamesTournaments.Where(p => p.EventId == eEvent.EventId && p.StageId == firstStage).Load();
            var gamesToDraw = Context.GamesTournaments.Local.Where(p => p.EventId == eEvent.EventId && p.StageId == firstStage)
                .ToList();
            var gamesToDrawSecondX = Context.GamesTournaments.Local.Where(p => p.EventId == eEvent.EventId && p.StageId == firstStage + 1)
                .ToList();
            int y = 30;
            foreach (var gamesTournament in gamesToDraw)
            {
                if (gamesTournament.TeamsTournament1Id != null)
                {
                    OneLabelDrawing(canvas, y, gamesTournament.TeamsTournament1.TeamName, 20);
                    y += 30;
                }
                else
                {
                    GamesTournamentXCreating(gamesTournament, 1);
                    OneLabelDrawing(canvas, y, "X", 20);
                    y += 30;
                }
                if (gamesTournament.TeamsTournament2Id != null)
                {
                    OneLabelDrawing(canvas, y, gamesTournament.TeamsTournament2.TeamName, 20);
                    y += 30;
                }
                else
                {
                    GamesTournamentXCreating(gamesTournament, 2);
                    OneLabelDrawing(canvas, y, "X", 20);
                    y += 30;
                }

            }
            int y2 = 45, scoreY = 90;
            foreach (var gamesTournament in gamesToDrawSecondX)
            {
                if (gamesTournament.TeamsTournament1Id != null)
                {
                    OneLabelDrawing(canvas, y2, gamesTournament.TeamsTournament1.TeamName, 140);
                    OneLabelDrawing(canvas, scoreY, gamesTournament.Score, 310);
                    y2 += 60;
                    scoreY += 120;
                }
                else
                {
                    y2 += 60;
                }

                if (gamesTournament.TeamsTournament2Id != null)
                {
                    OneLabelDrawing(canvas, y2, gamesTournament.TeamsTournament2.TeamName, 140);
                    OneLabelDrawing(canvas, scoreY, gamesTournament.Score, 310);
                    y2 += 60;
                    scoreY += 120;
                }
                else
                {
                    y2 += 60;
                }
            }
        }
        public int CalculateFirstStageId(Event eEvent)
        {
            switch (int.Parse(eEvent.DrawType))
            {
                case 64:
                    return 2;
                case 32:
                    return 3;
                case 16:
                    return 4;
                case 8:
                    return 5;

                default: return 1;
            }
        }
        public void OneLabelDrawing(Canvas canvas, int y, string content, int x)
        {
            Label lab = new Label();
            lab.FontSize = 12;
            lab.Content = content;
            Canvas.SetLeft(lab, x);
            Canvas.SetTop(lab, y);
            canvas.Children.Add(lab);
        }
        public void GamesTournamentXCreating(GamesTournament gamesTournament, int notXPlayerIs)
        {
            Context.TeamsTournaments.Where(p => p.EventId == gamesTournament.EventId).Load();
            Context.GamesTournaments.Where(p => p.EventId == gamesTournament.EventId).Load();
            int placeInDraw;
            if (gamesTournament.PlaceInDraw % 2 == 0)
                placeInDraw = gamesTournament.PlaceInDraw / 2;
            else
                placeInDraw = gamesTournament.PlaceInDraw / 2 + 1;
            var whichPlayer = gamesTournament.PlaceInDraw % 2 == 0 ? 2 : 1;
            GamesTournament gamesTournamentSelected =
                Context.GamesTournaments.Local.FirstOrDefault(p => p.EventId == gamesTournament.EventId &&
                                                                   p.PlaceInDraw == placeInDraw && p.StageId ==
                                                                  (gamesTournament.StageId + 1) && p.ForPlace == gamesTournament.ForPlace);
            if (whichPlayer == 1 && notXPlayerIs == 2)
            {
                Context.GamesTournaments.Local.FirstOrDefault(p => p.GamesTournamentId == gamesTournamentSelected.GamesTournamentId).TeamsTournament1Id = gamesTournament.TeamsTournament1Id;
            }
            else if (whichPlayer == 1 && notXPlayerIs == 1)
            {
                Context.GamesTournaments.Local.FirstOrDefault(p => p.GamesTournamentId == gamesTournamentSelected.GamesTournamentId).TeamsTournament1Id = gamesTournament.TeamsTournament2Id;
            }
            else if (whichPlayer == 2 && notXPlayerIs == 2)
            {
                Context.GamesTournaments.Local.FirstOrDefault(p => p.GamesTournamentId == gamesTournamentSelected.GamesTournamentId).TeamsTournament2Id = gamesTournament.TeamsTournament1Id;
            }
            else
            {
                Context.GamesTournaments.Local.FirstOrDefault(p => p.GamesTournamentId == gamesTournamentSelected.GamesTournamentId).TeamsTournament2Id = gamesTournament.TeamsTournament2Id;
            }
            Context.SaveChanges();
        }
        //переделать счет
        public void AllRoundsLabelsDrawing(Event eEvent, Canvas canvas)
        {
            Context.GamesTournaments.Where(p => p.EventId == eEvent.EventId).Load();
            int kolVoLudey = int.Parse(eEvent.DrawType);
            int left = 260, topNach = 75, top, yMnozh = 4, krug = CalculateFirstStageId(eEvent) + 2;
            kolVoLudey /= 4;
            while (krug < 8)
            {
                top = topNach;
                for (int j = 1; j <= kolVoLudey; j += 2)
                {
                    int placeInDraw;
                    if (j % 2 == 0)
                        placeInDraw = j / 2;
                    else
                        placeInDraw = j / 2 + 1;
                    if (Context.GamesTournaments.Local
                            .FirstOrDefault(p => p.StageId == krug && p.PlaceInDraw == placeInDraw && p.EventId == eEvent.EventId)
                            .TeamsTournament1Id != null)
                    {
                        OneLabelDrawing(canvas, top,
                            Context.GamesTournaments.Local
                                .FirstOrDefault(p => p.StageId == krug && p.PlaceInDraw == placeInDraw && p.EventId == eEvent.EventId).TeamsTournament1
                                .TeamName, left);
                        OneLabelDrawing(canvas, top + (30 * yMnozh / 2) + 15,
                            Context.GamesTournaments.Local
                                .FirstOrDefault(p => p.StageId == krug && p.PlaceInDraw == placeInDraw && p.EventId == eEvent.EventId).Score,
                            left + 170);
                    }
                    top += 30 * yMnozh;
                    if (Context.GamesTournaments.Local
                            .FirstOrDefault(p => p.StageId == krug && p.PlaceInDraw == placeInDraw && p.EventId == eEvent.EventId)
                            .TeamsTournament2Id != null)
                    {
                        OneLabelDrawing(canvas, top,
                            Context.GamesTournaments.Local
                                .FirstOrDefault(p => p.StageId == krug && p.PlaceInDraw == placeInDraw && p.EventId == eEvent.EventId).TeamsTournament2
                                .TeamName, left);
                    }
                    top += 30 * yMnozh;
                }
                left += 120;
                topNach += 15 * yMnozh;
                yMnozh *= 2;
                kolVoLudey /= 2;
                krug++;
            }
        }
        private int CalculateRoundForRectangle(int numberOfRectangle, int numberOfPlayers)
        {
            int startNumber = 0, round = 0;
            switch (numberOfPlayers)
            {
                case 64:
                    round = 3;
                    break;
                case 32:
                    round = 4;
                    break;
                case 16:
                    round = 5;
                    break;
                case 8:
                    round = 6;
                    break;
            }
            while (startNumber < numberOfRectangle)
            {
                numberOfPlayers /= 2;
                startNumber += numberOfPlayers;
                if (startNumber <= numberOfRectangle)
                {
                    round++;
                }
                else
                {
                    break;
                }

            }
            return round;
        }

        private int CalculatePlaceInDraw(int numberOfRectangle, int numberOfPlayers)
        {
            int startNumber = 0;
            while (startNumber < numberOfRectangle)
            {
                numberOfPlayers /= 2;
                if ((startNumber + numberOfPlayers) > numberOfRectangle)
                {
                    break;
                }
                startNumber += numberOfPlayers;
            }
            return (numberOfRectangle - startNumber) / 2 + 1;
        }

        private int CalculatePrecedentGame(int numberOfRectangle, int numberOfPlace)
        {
            if (numberOfRectangle % 2 == 0)
                return numberOfPlace * 2 - 1;
            return numberOfPlace * 2;
        }

        private string WinnerHelper(int winnerPlayer, string winnerScore)
        {
            return "";
        }
    }
}
