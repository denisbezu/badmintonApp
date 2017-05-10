using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO.Packaging;
using System.Linq;
using System.Security.AccessControl;
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
        #region Properties
        private Dictionary<Event, Dictionary<string, List<Rectangle>>> RectanglesTab { get; set; }
        public Dictionary<Event, Dictionary<string, Canvas>> CanvasDictionary { get; set; }
        public Event SelectedEvent { get; set; }
        public TabControl TabControl { get; set; }
        public Canvas SelectedCanvas { get; set; }
        public Dictionary<Event, List<Rectangle>> DictRectangles { get; set; }
        public List<string> Circles { get; set; }
        public BadmintonContext Context { get; set; }
        #endregion

        public DrawsFormer(BadmintonContext context)
        {
            SelectedEvent = new Event();
            CanvasDictionary = new Dictionary<Event, Dictionary<string, Canvas>>();
            Circles = new List<string>();
            DictRectangles = new Dictionary<Event, List<Rectangle>>();
            RectanglesTab = new Dictionary<Event, Dictionary<string, List<Rectangle>>>();
            Context = context;
            Context.GamesTournaments.Load(); //сделать отбор по выбранному турниру
            Context.TeamsTournaments.Load();
        }
        #region DrawingOnCanvas
        private void CanvasDrawing(Event eEvent, Canvas canvas)
        {
            RoundsDrawing(int.Parse(eEvent.DrawType), canvas);
            LinesDrawing(int.Parse(eEvent.DrawType), canvas);
            if (eEvent.IsDrawFormed == true)
            {
                if (eEvent.Type.TypeName.Equals("Одиночка"))
                {
                    int n = int.Parse(eEvent.DrawType) / 2, i = 0;
                    while (n > 2)
                    {
                        LinesDrawing(n, CanvasDictionary[eEvent].ElementAt(i).Value);
                        AllRoundGamesForLoosersLabelsDrawing(eEvent, CanvasDictionary[eEvent].ElementAt(i).Key, n);
                        Rectangles_drawing_tabs(eEvent, CanvasDictionary[eEvent].ElementAt(i).Value, CanvasDictionary[eEvent].ElementAt(i).Key, n / 2);
                        n /= 2;
                        i++;
                    }
                }
                AllRoundsLabelsDrawing(eEvent, canvas);
                FirstStageLabelsDrawing(eEvent, canvas);
                RectanglesDrawing(eEvent, canvas);
            }
        }
        private void RoundsDrawing(int numberPers, Canvas canvas)
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
        private void Rectangles_drawing_tabs(Event eEvent, Canvas canvas, string header, int numberOfPerson)
        {
            int left = 140, topNach = 50, top, mnozhTop = 1;//инициализация переменных
            int rectanglesForRound = numberOfPerson;//инициализация переменных
            if (!RectanglesTab.ContainsKey(eEvent))
            {
                Dictionary<string, List<Rectangle>> tempD = new Dictionary<string, List<Rectangle>>();
                tempD.Add(header, new List<Rectangle>());
                RectanglesTab.Add(eEvent, tempD); //добавляем пустой новый набор прямогольников для события для категории утешительного турнира
            }
            else
            {
                if (!RectanglesTab[eEvent].ContainsKey(header))
                {
                    RectanglesTab[eEvent].Add(header, new List<Rectangle>());//добавляем новый утеш турнир, если для события еще не был он создан
                }
                else
                    RectanglesTab[eEvent][header].Clear();
            }
            while (rectanglesForRound > 0)
            {
                top = topNach;
                for (int i = 0; i < rectanglesForRound; i++)//создание и позиционирование нового прямоугольника для утешительного турнира
                {
                    RectanglesTab[eEvent][header].Add(new Rectangle());
                    RectanglesTab[eEvent][header].Last().Height = 30;
                    RectanglesTab[eEvent][header].Last().Width = 120;
                    RectanglesTab[eEvent][header].Last().Fill = Brushes.Transparent;
                    Canvas.SetTop(RectanglesTab[eEvent][header].Last(), top);
                    Canvas.SetLeft(RectanglesTab[eEvent][header].Last(), left);
                    canvas.Children.Add(RectanglesTab[eEvent][header].Last());
                    RectanglesTab[eEvent][header].Last().MouseEnter += (sender, args) => (sender as Rectangle).Fill =
                        new SolidColorBrush(Color.FromArgb(45, 15, 255, 5));
                    RectanglesTab[eEvent][header].Last().MouseLeave += (sender, args) => (sender as Rectangle).Fill = Brushes.Transparent;
                    RectanglesTab[eEvent][header].Last().PreviewMouseDown += OnPreviewMouseDownTabRectangles;
                    top += 60 * mnozhTop;
                }
                left += 120;
                topNach += 30 * mnozhTop;
                mnozhTop *= 2;
                rectanglesForRound /= 2;
            }
        }
        private void AllRoundsLabelsDrawing(Event eEvent, Canvas canvas)
        {
            int kolVoLudey = int.Parse(eEvent.DrawType);
            int left = 260, topNach = 75, top, yMnozh = 4, krug = CalculateFirstStageId(eEvent) + 2;
            kolVoLudey /= 4;
            while (krug < 8)
            {
                top = topNach;
                for (int j = 1; j <= kolVoLudey; j += 2)
                {
                    int placeInDraw = CalculatePlaceInDraw(j);
                    var selectedGame = Context.GamesTournaments.Local
                        .FirstOrDefault(p => p.StageId == krug && p.PlaceInDraw == placeInDraw &&
                                             p.EventId == eEvent.EventId && p.ForPlace == 1);
                    if (selectedGame.TeamsTournament1Id != null)
                    {
                        OneLabelDrawing(canvas, top, selectedGame.TeamsTournament1.TeamName, left); // добавдение в сетку 1 команды
                        OneLabelDrawing(canvas, top + (30 * yMnozh / 2) + 15, ScoreConverter(selectedGame.Score), left + 170);
                    }
                    top += 30 * yMnozh;
                    if (selectedGame.TeamsTournament2Id != null)
                        OneLabelDrawing(canvas, top, selectedGame.TeamsTournament2.TeamName, left); // добавление на сетку 2 команды
                    
                    top += 30 * yMnozh;
                }
                left += 120;
                topNach += 15 * yMnozh;
                yMnozh *= 2;
                kolVoLudey /= 2;
                krug++;
            }
            var winner = Context.GamesTournaments.Local
                .FirstOrDefault(p => p.StageId == 8 && p.PlaceInDraw == 1 && p.EventId == eEvent.EventId &&
                                     p.ForPlace == 1);
            if (winner.TeamsTournament1Id != null)
                OneLabelDrawing(canvas, topNach, winner.TeamsTournament1.TeamName, left);

        }
        private void OneLabelDrawing(Canvas canvas, int y, string content, int x)
        {
            Label lab = new Label();
            lab.FontSize = 12;
            lab.Content = content;
            Canvas.SetLeft(lab, x);
            Canvas.SetTop(lab, y);
            canvas.Children.Add(lab);
        }
        #endregion

        #region EventsHandler and EventChanger
        public void EventChanged(Event eEvent, Canvas canvas)
        {
            canvas.Children.Clear();
            while (TabControl.Items.Count > 1)
                TabControl.Items.RemoveAt(TabControl.Items.Count - 1);
            TabsMaker(eEvent);
            CanvasDrawing(eEvent, canvas);
        }
        private void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Context.GamesTournaments.Where(p => p.EventId == SelectedEvent.EventId).Load();
            if (e.ClickCount != 2) return;
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
                                int left = 20, topNach = 30, top, yMnozh = 1;
                                for (int y = 0; y < round - CalculateFirstStageId(SelectedEvent); y++)
                                {
                                    left += 120;
                                    topNach += 15 * yMnozh;
                                    yMnozh *= 2;
                                }
                                top = topNach;
                                int goUntil;
                                int winnerButton;
                                if (whichPlayer == 1)
                                    goUntil = placeInDraw * 2 - 1;
                                else
                                    goUntil = placeInDraw * 2;
                                for (int j = 0; j < goUntil - 1; j++)
                                {
                                    top += 30 * yMnozh;
                                }
                                if (matchInfo.radioButton1.IsChecked == true)
                                {
                                    winnerButton = 1;
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
                                    winnerButton = 2;
                                    OneLabelDrawing(SelectedCanvas, top, team2.TeamName, left);
                                    if (whichPlayer == 1)
                                    {
                                        Context.GamesTournaments.Local
                                            .FirstOrDefault(p => p.ForPlace == 1 && p.PlaceInDraw == placeInDraw &&
                                                                 p.StageId == round && p.EventId == SelectedEvent.EventId).TeamsTournament1Id = team2.TeamsTournamentId;
                                    }
                                    else
                                    {
                                        Context.GamesTournaments.Local
                                            .FirstOrDefault(p => p.ForPlace == 1 && p.PlaceInDraw == placeInDraw &&
                                                                 p.StageId == round && p.EventId == SelectedEvent.EventId).TeamsTournament2Id = team2.TeamsTournamentId;
                                    }
                                }
                                Context.GamesTournaments.Local.FirstOrDefault(
                                    p => p.TeamsTournament1Id == team1.TeamsTournamentId &&
                                         p.TeamsTournament2Id == team2.TeamsTournamentId && p.EventId == SelectedEvent.EventId).Score = WinnerHelper(winnerButton, matchInfo.Winner);
                                OneLabelDrawing(SelectedCanvas, top + 15, matchInfo.Winner, left + 50);

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
            EventChanged(SelectedEvent, SelectedCanvas);
        }
        private void OnPreviewMouseDownTabRectangles(object sender, MouseButtonEventArgs e)
        {
            Context.GamesTournaments.Where(p => p.EventId == SelectedEvent.EventId).Load();
            if (e.ClickCount != 2) return;
            string header = (TabControl.SelectedItem as TabItem).Header.ToString();
            int tabsCount = TabControl.SelectedIndex;
            int kolVoLudey = int.Parse(SelectedEvent.DrawType);
            for (int i = 0; i < tabsCount; i++)
            {
                kolVoLudey /= 2;
            }
            for (int i = 0; i < kolVoLudey - 1; i++)
            {
                if ((sender as Rectangle) == RectanglesTab[SelectedEvent][header][i])
                {
                    MessageBox.Show("i = " + i + ", header = " + header);
                }
            }
        }
        #endregion

        #region FirstStageWorkers
        public void FirstRoundGamesFormer(Event eEvent, List<int> numsForDraw)
        {
            //Context.GamesTournaments.Load(); // нужно будет сделать Where
            //Context.TeamsTournaments.Load();
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
            GamesTournament gamesTournamentLast = new GamesTournament();
            gamesTournamentLast.ForPlace = 1;
            gamesTournamentLast.PlaceInDraw = 1;
            gamesTournamentLast.StageId = 8;
            gamesTournamentLast.EventId = eEvent.EventId;
            Context.GamesTournaments.Local.Add(gamesTournamentLast);
            Context.SaveChanges();
        }
        private void FirstStageLabelsDrawing(Event eEvent, Canvas canvas)
        {
            var firstStage = CalculateFirstStageId(eEvent);
            var gamesToDraw = Context.GamesTournaments.Local.Where(p => p.EventId == eEvent.EventId && p.StageId == firstStage && p.ForPlace == 1)
                .ToList();
            var gamesToDrawSecondX = Context.GamesTournaments.Local.Where(p => p.EventId == eEvent.EventId && p.StageId == firstStage + 1 && p.ForPlace == 1)
                .ToList();
            int y = 30, score1Y = 60;
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
                OneLabelDrawing(canvas, score1Y, ScoreConverter(gamesTournament.Score), 190);
                score1Y += 60;
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
                    OneLabelDrawing(canvas, y2, gamesTournament.TeamsTournament1.TeamName, 140);
                y2 += 60;
                OneLabelDrawing(canvas, scoreY, ScoreConverter(gamesTournament.Score), 310);
                scoreY += 120;
                if (gamesTournament.TeamsTournament2Id != null)
                    OneLabelDrawing(canvas, y2, gamesTournament.TeamsTournament2.TeamName, 140);
                y2 += 60;
            }
        }
        private int CalculateFirstStageId(Event eEvent)
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
        private int CalculateFirstStageId(int numberOfPersons)
        {
            switch (numberOfPersons)
            {
                case 64:
                    return 2;
                case 32:
                    return 3;
                case 16:
                    return 4;
                case 8:
                    return 5;
                case 4:
                    return 6;
                case 2:
                    return 7;
                default: return 1;
            }
        }
        private void GamesTournamentXCreating(GamesTournament gamesTournament, int notXPlayerIs)
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
        #endregion

        #region ConvertersHelpers
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
        private int CalculatePlaceInDraw(int currentNumber)//для лейблов
        {
            int placeInDraw;
            if (currentNumber % 2 == 0)
                placeInDraw = currentNumber / 2;
            else
                placeInDraw = currentNumber / 2 + 1;
            return placeInDraw;
        }
        private int CalculatePrecedentGame(int numberOfRectangle, int numberOfPlace)
        {
            if (numberOfRectangle % 2 == 0)
                return numberOfPlace * 2 - 1;
            return numberOfPlace * 2;
        }
        private string WinnerHelper(int winnerPlayer, string winnerScore)
        {
            string score;
            if (winnerPlayer == 1)
                return winnerScore;
            List<string> scores = new List<string>();
            scores = winnerScore.Split(',').ToList();
            for (int i = 0; i < scores.Count; i++)
                scores[i] = (int.Parse(scores[i]) * (-1)).ToString();
            if (scores.Count == 2)
                score = scores[0] + ", " + scores[1];
            else
                score = scores[0] + ", " + scores[1] + ", " + scores[2];
            return score;
        }
        private string ScoreConverter(string winnerScore)
        {
            if (winnerScore == null)
                return winnerScore;
            string score;
            List<string> scores = new List<string>();
            scores = winnerScore.Split(',').ToList();
            int minusCount = 0;
            for (int i = 0; i < scores.Count; i++)
            {
                if (int.Parse(scores[i]) < 0)
                    minusCount++;
            }
            if (minusCount > 1)
            {
                for (int i = 0; i < scores.Count; i++)
                    scores[i] = (int.Parse(scores[i]) * (-1)).ToString();
                if (scores.Count == 2)
                    score = scores[0] + ", " + scores[1];
                else
                    score = scores[0] + ", " + scores[1] + ", " + scores[2];
            }
            else
                score = winnerScore;
            return score;
        }

        #endregion

        #region Tabs
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
                canvas.Background = Brushes.AliceBlue;
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
        private void TabsMaker(Event eEvent)
        {
            if (eEvent.Type.TypeName.Equals("Одиночка"))
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
        }
        #endregion

        public void GamesForLoosersFormer(Event eEvent, int numberOfPeople, int forPlace)
        {
            int numberPlace = CalculateFirstStageId(numberOfPeople) /*+ 1*/, draw = numberOfPeople;
            while (numberPlace <= 7)
            {
                for (int i = 1; i <= draw; i += 2)
                {
                    GamesTournament gamesTournament = new GamesTournament();
                    gamesTournament.ForPlace = forPlace;
                    gamesTournament.PlaceInDraw = (i / 2) + 1;
                    gamesTournament.StageId = numberPlace;
                    gamesTournament.EventId = eEvent.EventId;
                    Context.GamesTournaments.Local.Add(gamesTournament);
                }
                numberPlace++;
                draw /= 2;
            }
            GamesTournament gamesTournamentLast = new GamesTournament();
            gamesTournamentLast.ForPlace = forPlace;
            gamesTournamentLast.PlaceInDraw = 1;
            gamesTournamentLast.StageId = 8;
            gamesTournamentLast.EventId = eEvent.EventId;
            Context.GamesTournaments.Local.Add(gamesTournamentLast);
            Context.SaveChanges();
        }
        public int ForPlaceCalculate(string header)
        {
            string[] splitter = header.Split(' ');
            return int.Parse(splitter[1]);
        }
        private void AllRoundGamesForLoosersLabelsDrawing(Event eEvent, string header, int numberOfPersons)
        {
            int kolVoLudey = numberOfPersons;
            int left = 20, topNach = 30, top, yMnozh = 1, krug = CalculateFirstStageId(numberOfPersons);
            int forPlace = ForPlaceCalculate(header);
            while (krug < 8)
            {
                top = topNach;
                for (int j = 1; j <= kolVoLudey; j += 2)
                {
                    int placeInDraw = CalculatePlaceInDraw(j);
                    var selectedGame = Context.GamesTournaments.Local
                        .FirstOrDefault(p => p.StageId == krug && p.PlaceInDraw == placeInDraw &&
                                             p.EventId == eEvent.EventId && p.ForPlace == forPlace);

                    if (selectedGame.TeamsTournament1Id != null)
                    {
                        OneLabelDrawing(CanvasDictionary[eEvent][header], top, selectedGame.TeamsTournament1.TeamName, left); // добавдение в сетку 1 команды
                        OneLabelDrawing(CanvasDictionary[eEvent][header], top + (30 * yMnozh / 2) + 15, ScoreConverter(selectedGame.Score), left + 170);
                    }
                    top += 30 * yMnozh;
                    if (selectedGame.TeamsTournament2Id != null)
                        OneLabelDrawing(CanvasDictionary[eEvent][header], top, selectedGame.TeamsTournament2.TeamName, left); // добавление на сетку 2 команды
                    top += 30 * yMnozh;
                }
                left += 120;
                topNach += 15 * yMnozh;
                yMnozh *= 2;
                kolVoLudey /= 2;
                krug++;
            }
            var winner = Context.GamesTournaments.Local
                .FirstOrDefault(p => p.StageId == 8 && p.PlaceInDraw == 1 && p.EventId == eEvent.EventId &&
                                     p.ForPlace == forPlace);
            if (winner.TeamsTournament1Id != null)
                OneLabelDrawing(CanvasDictionary[eEvent][header], topNach, winner.TeamsTournament1.TeamName, left);
        }


    }
}
