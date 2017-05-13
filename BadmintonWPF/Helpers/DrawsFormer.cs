using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using badmintonDataBase.DataAccess;
using badmintonDataBase.Models;
using BadmintonWPF.Helpers.DrawsHelpers;
using BadmintonWPF.Views;

namespace BadmintonWPF.Helpers
{
    public class DrawsFormer
    {
        #region Properties
        private Dictionary<Event, Dictionary<string, List<Rectangle>>> RectanglesTab { get; set; }
        
        public Event SelectedEvent { get; set; }
        public Canvas SelectedCanvas { get; set; }
        public Dictionary<Event, List<Rectangle>> DictRectangles { get; set; }
        public List<string> Circles { get; set; }
        public BadmintonContext Context { get; set; }
        public TabsWorker TabsWorker { get; set; }
        #endregion

        public DrawsFormer(BadmintonContext context, TabsWorker tabsWorker)
        {
            SelectedEvent = new Event();
            Circles = new List<string>();
            DictRectangles = new Dictionary<Event, List<Rectangle>>();
            RectanglesTab = new Dictionary<Event, Dictionary<string, List<Rectangle>>>();
            Context = context;
            Context.GamesTournaments.Load(); //сделать отбор по выбранному турниру
            Context.TeamsTournaments.Load();
            TabsWorker = tabsWorker;
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
                        LinesDrawing(n, TabsWorker.CanvasDictionary[eEvent].ElementAt(i).Value);
                        AllRoundGamesForLoosersLabelsDrawing(eEvent, TabsWorker.CanvasDictionary[eEvent].ElementAt(i).Key, n);
                        Rectangles_drawing_tabs(eEvent, TabsWorker.CanvasDictionary[eEvent].ElementAt(i).Value, TabsWorker.CanvasDictionary[eEvent].ElementAt(i).Key, n / 2);
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
            int x1, x2, y1, y2, yMnozh = 1;
            int x1Nach = 20, x2Nach = 140, y1Nach = 50, y2Nach = 50;
            int xHorNach = 140, y1HorNach = 50, y2HorNach = 80, xHor, y1Hor, y2Hor;
            while (numberPers > 0)
            {
                x1 = x1Nach;
                x2 = x2Nach;
                y1 = y1Nach;
                y2 = y2Nach;
                xHor = xHorNach;
                y1Hor = y1HorNach;
                y2Hor = y2HorNach;
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
                    y1 += 30 * yMnozh;
                    y2 += 30 * yMnozh;
                }
                for (int i = 0; i < numberPers / 2; i++)
                {
                    Line l1 = new Line();
                    l1.Stroke = Brushes.Black;
                    l1.StrokeThickness = 1;
                    l1.X1 = xHor;
                    l1.X2 = xHor;
                    l1.Y1 = y1Hor;
                    l1.Y2 = y2Hor;
                    canvas.Children.Add(l1);
                    y1Hor += 60 * yMnozh;
                    y2Hor += 60 * yMnozh;
                }
                x1Nach += 120;
                x2Nach += 120;
                y1Nach += 15 * yMnozh;
                y2Nach += 15 * yMnozh;
                xHorNach += 120;
                y1HorNach += 15 * yMnozh;
                y2HorNach += 45 * yMnozh;
                yMnozh *= 2;
                numberPers /= 2;
            }
        }
        private void RectanglesDrawing(Event eEvent, Canvas canvas)
        {
            int numberPers = int.Parse(eEvent.DrawType);
            int left = 140, topNach = 50, top, mnozhTop = 1;
            numberPers /= 2;
            if (!DictRectangles.ContainsKey(eEvent))
                DictRectangles.Add(eEvent, new List<Rectangle>());
            else
                DictRectangles[eEvent].Clear();
            while (numberPers > 0)
            {
                top = topNach;
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
                    top += 60 * mnozhTop;
                }
                left += 120;
                topNach += 30 * mnozhTop;
                mnozhTop *= 2;
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
                    RectanglesTab[eEvent].Add(header, new List<Rectangle>());//добавляем новый утеш турнир, если для события еще не был он создан
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
        public void EventChanged(Canvas canvas)
        {
            canvas.Children.Clear();
            TabsWorker.TabsCreatorRemover(SelectedEvent);
            CanvasDrawing(SelectedEvent, canvas);
        }
        private void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Context.GamesTournaments.Where(p => p.EventId == SelectedEvent.EventId).Load();
            if (e.ClickCount != 2) return;
            int numberOfPeople = int.Parse(SelectedEvent.DrawType);
            for (int i = 0; i < numberOfPeople - 1; i++)
            {
                if ((sender as Rectangle) == DictRectangles[SelectedEvent][i])
                {
                    
                    int placeInDraw = CalculatePlaceInDraw(i, numberOfPeople);
                    int round = CalculateRoundForRectangle(i, numberOfPeople);
                    
                    var whichPlayer = i % 2 == 0 ? 1 : 2;
                    int precedentNumber = CalculatePrecedentGame(i, placeInDraw);
                    try
                    {
                        var team1 = Context.GamesTournaments.Local
                            .FirstOrDefault(p => p.ForPlace == 1 && p.PlaceInDraw == precedentNumber &&
                                                 p.StageId == round - 1 && p.EventId == SelectedEvent.EventId).TeamsTournament1;//первая команда;
                        var team2 = Context.GamesTournaments.Local
                            .FirstOrDefault(p => p.ForPlace == 1 && p.PlaceInDraw == precedentNumber &&
                                                 p.StageId == round - 1 && p.EventId == SelectedEvent.EventId).TeamsTournament2;//вторая команда
                        if (team1 != null && team2 != null)
                        {
                            MatchInfo matchInfo = MatchDialogInfoCreator(team1, team2);
                            if (matchInfo.WinnerChanged)//если мы определили победителя
                            {
                                int left = 20, topNach = 30, yMnozh = 1;
                                CalculateMarginForNeededLabel(ref left, ref topNach, ref yMnozh, round, whichPlayer,
                                    placeInDraw);
                                var selectedGame = Context.GamesTournaments.Local
                                   .FirstOrDefault(p => p.ForPlace == 1 && p.PlaceInDraw == placeInDraw &&
                                                        p.StageId == round && p.EventId == SelectedEvent.EventId);//следующая игра
                                var winnerButton = WinnerDeterminer(SelectedCanvas, matchInfo, topNach, left, whichPlayer, team1, team2, selectedGame, WhichHeaderChooseForLoosers(round - 1));
                                Context.GamesTournaments.Local.FirstOrDefault(
                                    p => p.TeamsTournament1Id == team1.TeamsTournamentId &&
                                         p.TeamsTournament2Id == team2.TeamsTournamentId && p.EventId == SelectedEvent.EventId).Score = WinnerHelperSplitter(winnerButton, matchInfo.Winner);//заносим счет в бд
                                OneLabelDrawing(SelectedCanvas, topNach + 15, matchInfo.Winner, left + 50); // отображаем счет
                                matchInfo.WinnerChanged = false;
                            }
                        }
                        else
                            throw new Exception();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Невозможно добавить результат", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    Context.SaveChanges();
                    if (FirstConcelationXCheker(1) && SelectedEvent.Type.TypeName.Equals("Одиночка"))
                    {
                        InvokeXCreator(XFirstConcelationDrawer(
                            ForPlaceCalculate((TabsWorker.TabControl.Items[1] as TabItem).Header.ToString()),
                            round));
                    }
                    
                }
            }
            EventChanged(SelectedCanvas);
        }

        private void OnPreviewMouseDownTabRectangles(object sender, MouseButtonEventArgs e)
        {
            Context.GamesTournaments.Where(p => p.EventId == SelectedEvent.EventId).Load();
            if (e.ClickCount != 2) return;
            string header = (TabsWorker.TabControl.SelectedItem as TabItem).Header.ToString();
            int tabsCount = TabsWorker.TabControl.SelectedIndex;
            int forPlace = ForPlaceCalculate(header);
            int kolVoLudey = int.Parse(SelectedEvent.DrawType);
            for (int i = 0; i < tabsCount; i++)
            {
                kolVoLudey /= 2;
            }
            for (int i = 0; i < kolVoLudey - 1; i++)
            {
                if ((sender as Rectangle) == RectanglesTab[SelectedEvent][header][i])
                {
                    //MessageBox.Show("i = " + i + ", header = " + header);
                    int placeInDraw = CalculatePlaceInDraw(i, kolVoLudey);
                    int round = CalculateRoundForRectangle(i, kolVoLudey);
                    var whichPlayer = i % 2 == 0 ? 1 : 2;
                    int precedentNumber = CalculatePrecedentGame(i, placeInDraw);
                    try
                    {
                        var team1 = Context.GamesTournaments.Local
                            .FirstOrDefault(p => p.ForPlace == forPlace && p.PlaceInDraw == precedentNumber &&
                                                 p.StageId == round - 1 && p.EventId == SelectedEvent.EventId).TeamsTournament1;//первая команда;
                        var team2 = Context.GamesTournaments.Local
                            .FirstOrDefault(p => p.ForPlace == forPlace && p.PlaceInDraw == precedentNumber &&
                                                 p.StageId == round - 1 && p.EventId == SelectedEvent.EventId).TeamsTournament2;//вторая команда
                        if (team1 != null && team2 != null)
                        {
                            MatchInfo matchInfo = MatchDialogInfoCreator(team1, team2);
                            if (matchInfo.WinnerChanged)//если мы определили победителя
                            {
                                int left = 20, topNach = 30, yMnozh = 1;
                                CalculateMarginForNeededLabel(ref left, ref topNach, ref yMnozh, round - tabsCount, whichPlayer, placeInDraw);
                                var selectedGame = Context.GamesTournaments.Local
                                    .FirstOrDefault(p => p.ForPlace == forPlace && p.PlaceInDraw == placeInDraw &&
                                                         p.StageId == round && p.EventId == SelectedEvent.EventId);//следующая игра
                                var winnerButton = WinnerDeterminer(TabsWorker.CanvasDictionary[SelectedEvent][header], matchInfo, topNach, left, whichPlayer, team1, team2, selectedGame, WhichHeaderChooseForLoosers(round - 1));
                                Context.GamesTournaments.Local.FirstOrDefault(
                                    p => p.TeamsTournament1Id == team1.TeamsTournamentId &&
                                         p.TeamsTournament2Id == team2.TeamsTournamentId && p.EventId == SelectedEvent.EventId).Score = WinnerHelperSplitter(winnerButton, matchInfo.Winner);//заносим счет в бд
                                OneLabelDrawing(TabsWorker.CanvasDictionary[SelectedEvent][header], topNach + 15, matchInfo.Winner, left + 50); // отображаем счет
                                matchInfo.WinnerChanged = false;
                            }
                        }
                        else
                            throw new Exception();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Невозможно добавить результат", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    Context.SaveChanges();
                }
            }
            EventChanged(SelectedCanvas);
            TabsWorker.TabControl.SelectedIndex = tabsCount;
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
        private void GamesTournamentXCreating(GamesTournament gamesTournament, int xPlayerIs)
        {
            int placeInDraw;
            if (gamesTournament.PlaceInDraw % 2 == 0)
                placeInDraw = gamesTournament.PlaceInDraw / 2;
            else
                placeInDraw = gamesTournament.PlaceInDraw / 2 + 1;
            var whichPlayer = gamesTournament.PlaceInDraw % 2 == 0 ? 2 : 1;
            int? selectedTeamId;
            #region GamesTournament

            GamesTournament gamesTournamentSelected =
                Context.GamesTournaments.Local.FirstOrDefault(p => p.EventId == gamesTournament.EventId &&
                                                                   p.PlaceInDraw == placeInDraw && p.StageId ==
                                                                   (gamesTournament.StageId + 1) && p.ForPlace == gamesTournament.ForPlace);
            GamesTournament gameToChange =
                Context.GamesTournaments.Local.FirstOrDefault(
                    p => p.GamesTournamentId == gamesTournamentSelected.GamesTournamentId);

            #endregion
            if (whichPlayer == 1 && xPlayerIs == 2)
            {
                gameToChange.TeamsTournament1Id = gamesTournament.TeamsTournament1Id;
                selectedTeamId = gamesTournament.TeamsTournament2Id;
            }
            else if (whichPlayer == 1 && xPlayerIs == 1)
            {
                gameToChange.TeamsTournament1Id = gamesTournament.TeamsTournament2Id;
                selectedTeamId = gamesTournament.TeamsTournament1Id;
            }
            else if (whichPlayer == 2 && xPlayerIs == 2)
            {
                gameToChange.TeamsTournament2Id = gamesTournament.TeamsTournament1Id;
                selectedTeamId = gamesTournament.TeamsTournament2Id;
            }
            else
            {
                gameToChange.TeamsTournament2Id = gamesTournament.TeamsTournament2Id;
                selectedTeamId = gamesTournament.TeamsTournament1Id;
            }
            TeamsTournament teamToAdd = null;
            if (Context.TeamsTournaments.Local.FirstOrDefault(p => p.TeamsTournamentId == selectedTeamId) != null)
                teamToAdd = Context.TeamsTournaments.Local.FirstOrDefault(p => p.TeamsTournamentId == selectedTeamId);
            LooserWritingToDataBase(WhichHeaderChooseForLoosers((int)gamesTournament.StageId), (int)gameToChange.StageId, placeInDraw, teamToAdd, whichPlayer);
            if (SelectedEvent.Type.TypeName.Equals("Одиночка") && gamesTournament.ForPlace == 1)
                XValuesConcelationCalc(WhichHeaderChooseForLoosers((int)gamesTournament.StageId), (int)gameToChange.StageId - 1, whichPlayer, placeInDraw);
            Context.SaveChanges();
        }
        #endregion

        #region ConvertersHelpers
        private void CalculateMarginForNeededLabel(ref int left, ref int topNach, ref int yMnozh, int round, int whichPlayer, int placeInDraw)
        {
            for (int y = 0; y < round - CalculateFirstStageId(SelectedEvent); y++)
            {
                left += 120;
                topNach += 15 * yMnozh;
                yMnozh *= 2;
            }//проход на нужную позицию

            int goUntil;//для того, чтобы определить топ отсуп
            if (whichPlayer == 1)
                goUntil = placeInDraw * 2 - 1;
            else
                goUntil = placeInDraw * 2;
            for (int j = 0; j < goUntil - 1; j++)
                topNach += 30 * yMnozh;
        }
        private int CalculateRoundForRectangle(int numberOfRectangle, int numberOfPlayers)
        {
            int startNumber = 0, round = 8;
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
                case 4:
                    round = 7;
                    break;
                case 2:
                    round = 8;
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
        private string WinnerHelperSplitter(int winnerPlayer, string winnerScore)
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
        private int WinnerDeterminer(Canvas canvas, MatchInfo matchInfo, int top, int left, int whichPlayer, TeamsTournament team1, TeamsTournament team2, GamesTournament selectedGame, string header)
        {
            int winnerButton;
            TeamsTournament selectedTeam;
            if (matchInfo.radioButton1.IsChecked == true)//если выиграл первый игрок
            {
                winnerButton = 1;//победил 1ый
                OneLabelDrawing(canvas, top, team1.TeamName, left);//рисуем
                selectedTeam = team2;
                if (whichPlayer == 1)
                    selectedGame.TeamsTournament1Id = team1.TeamsTournamentId;//если на место первого нужно рисовать по расчетам выше
                else
                    selectedGame.TeamsTournament2Id = team1.TeamsTournamentId;//если на место второго надо рисовать по расчетам выше
            }
            else//если победил второй
            {
                winnerButton = 2;//второй победил
                OneLabelDrawing(canvas, top, team2.TeamName, left);//рисуем
                selectedTeam = team1;
                if (whichPlayer == 1)
                    selectedGame.TeamsTournament1Id = team2.TeamsTournamentId;//если на место первого нужно рисовать по расчетам выше
                else
                    selectedGame.TeamsTournament2Id = team2.TeamsTournamentId;//если на место второго нужно рисовать по расчетам выше
            }
            if (SelectedEvent.Type.TypeName.Equals("Одиночка") && selectedGame.ForPlace == 1)//для 
                LooserWritingToDataBase(header, (int)selectedGame.StageId, selectedGame.PlaceInDraw, selectedTeam, whichPlayer);
            return winnerButton;
        }
        private MatchInfo MatchDialogInfoCreator(TeamsTournament team1, TeamsTournament team2)
        {
            MatchInfo matchInfo = new MatchInfo();
            matchInfo.igrok1.Content = team1.TeamName;
            matchInfo.igrok2.Content = team2.TeamName;
            matchInfo.radioButton1.Content = team1.TeamName;
            matchInfo.radioButton2.Content = team2.TeamName;//Создание окна выбора счета и заполнение лейблов
            matchInfo.ShowDialog();
            return matchInfo;
        }
        private string WhichHeaderChooseForLoosers(int round)
        {
            switch (round)
            {
                case 2:
                    return "За 33 место";
                case 3:
                    return "За 17 место";
                case 4:
                    return "За 9 место";
                case 5:
                    return "За 5 место";
            }
            return "";
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
            if (header == "")
            {
                return -1; // не считает полуфинал и финал
            }
            string[] splitter = header.Split(' ');// делим на слова
            return int.Parse(splitter[1]); // ю=ьерем только место
        }
        private void AllRoundGamesForLoosersLabelsDrawing(Event eEvent, string header, int numberOfPersons)
        {
            int kolVoLudey = numberOfPersons; //количество людей
            int left = 20, topNach = 30, top, yMnozh = 1, krug = CalculateFirstStageId(numberOfPersons); // расчет круга и инициализациыя переменных
            int forPlace = ForPlaceCalculate(header);// расчет места
            if (forPlace == -1)//если у нас индекс > 6
                return;
            while (krug < 8)
            {
                top = topNach;
                for (int j = 1; j <= kolVoLudey; j += 2)
                {
                    int placeInDraw = CalculatePlaceInDraw(j); // место в сетке
                    var selectedGame = Context.GamesTournaments.Local// выбранная игра, на которую нужно поместить проигравшего игрока
                        .FirstOrDefault(p => p.StageId == krug && p.PlaceInDraw == placeInDraw &&
                                             p.EventId == eEvent.EventId && p.ForPlace == forPlace);
                    //if(selectedGame == null)
                    //    return;
                    if (selectedGame.TeamsTournament1Id != null)
                    {
                        OneLabelDrawing(TabsWorker.CanvasDictionary[eEvent][header], top, selectedGame.TeamsTournament1.TeamName, left); // добавдение в сетку 1 команды
                        OneLabelDrawing(TabsWorker.CanvasDictionary[eEvent][header], top + (30 * yMnozh / 2) + 15, ScoreConverter(selectedGame.Score), left + 170);//добавляем счет
                    }
                    top += 30 * yMnozh;
                    if (selectedGame.TeamsTournament2Id != null)
                        OneLabelDrawing(TabsWorker.CanvasDictionary[eEvent][header], top, selectedGame.TeamsTournament2.TeamName, left); // добавление на сетку 2 команды
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
                OneLabelDrawing(TabsWorker.CanvasDictionary[eEvent][header], topNach, winner.TeamsTournament1.TeamName, left);
        }

        private void LooserWritingToDataBase(string header, int round, int placeInDraw, TeamsTournament teamToAdd, int whichPlayerWon)
        {
            if (teamToAdd == null)
                return;
            int forPlace = ForPlaceCalculate(header);
            if (forPlace == -1)
                return;

            if (whichPlayerWon == 1)
                Context.GamesTournaments.Local
                        .FirstOrDefault(p => p.EventId == SelectedEvent.EventId && p.StageId == round &&
                                             p.PlaceInDraw == placeInDraw && p.ForPlace == forPlace)
                        .TeamsTournament1Id = teamToAdd.TeamsTournamentId;
            else
            {
                Context.GamesTournaments.Local
                        .FirstOrDefault(p => p.EventId == SelectedEvent.EventId && p.StageId == round &&
                                             p.PlaceInDraw == placeInDraw && p.ForPlace == forPlace)
                        .TeamsTournament2Id = teamToAdd.TeamsTournamentId;
            }
        }

        private void XValuesConcelationCalc(string header, int round, int whichPlayer, int placeInDraw)
        {
            int left = 20, topNach = 30, yMnozh = 1;
            CalculateMarginForNeededLabel(ref left, ref topNach, ref yMnozh, round, whichPlayer, placeInDraw);
            OneLabelDrawing(TabsWorker.CanvasDictionary[SelectedEvent][header], topNach, "X", left); // отображаем
        }

        private bool FirstConcelationXCheker(int forPlace)
        {
            var secondStage = CalculateFirstStageId(int.Parse(SelectedEvent.DrawType) / 2);
            var secondRoundTeams = Context.GamesTournaments.Local.Where(
                p => p.EventId == SelectedEvent.EventId && p.ForPlace == forPlace
                     && p.StageId == secondStage);
            foreach (var gamesTournament in secondRoundTeams)
            {
                if (gamesTournament.TeamsTournament1 == null)
                    return false;
                if (gamesTournament.TeamsTournament2 == null)
                    return false;
            }
            return true;
        }

        private IEnumerable<GamesTournament> XFirstConcelationDrawer(int forPlace, int round)
        {
            var firstConcelationX = Context.GamesTournaments.Local.Where(
                p => p.EventId == SelectedEvent.EventId && p.ForPlace == forPlace
                     && p.StageId == round);
            firstConcelationX = firstConcelationX.Where(p => p.TeamsTournament1 == null || p.TeamsTournament2 == null);
            return firstConcelationX;
        }

        private void InvokeXCreator(IEnumerable<GamesTournament> firstConcelationX)
        {
            foreach (var gamesTournament in firstConcelationX)
            {
                int whichPlayerX;
                if (gamesTournament.TeamsTournament1 == null)
                    whichPlayerX = 1;
                else
                    whichPlayerX = 2;
                XCreator(gamesTournament, whichPlayerX);
            }
        }
        private void XCreator(GamesTournament gamesTournament, int xPlayerIs)
        {
            int placeInDraw;
            if (gamesTournament.PlaceInDraw % 2 == 0)
                placeInDraw = gamesTournament.PlaceInDraw / 2;
            else
                placeInDraw = gamesTournament.PlaceInDraw / 2 + 1;
            var whichPlayer = gamesTournament.PlaceInDraw % 2 == 0 ? 2 : 1;
            #region GamesTournament

            GamesTournament gamesTournamentSelected =
                Context.GamesTournaments.Local.FirstOrDefault(p => p.EventId == gamesTournament.EventId &&
                                                                   p.PlaceInDraw == placeInDraw && p.StageId ==
                                                                   (gamesTournament.StageId + 1) && p.ForPlace == gamesTournament.ForPlace);

            GamesTournament gameToChange =
                Context.GamesTournaments.Local.FirstOrDefault(
                    p => { return gamesTournamentSelected != null && p.GamesTournamentId == gamesTournamentSelected.GamesTournamentId; });
            if (gameToChange == null)
                return;
            #endregion
            if (whichPlayer == 1 && xPlayerIs == 2)
                gameToChange.TeamsTournament1Id = gamesTournament.TeamsTournament1Id;
            else if (whichPlayer == 1 && xPlayerIs == 1)
                gameToChange.TeamsTournament1Id = gamesTournament.TeamsTournament2Id;
            else if (whichPlayer == 2 && xPlayerIs == 2)
                gameToChange.TeamsTournament2Id = gamesTournament.TeamsTournament1Id;
            else
                gameToChange.TeamsTournament2Id = gamesTournament.TeamsTournament2Id;
            Context.SaveChanges();
        }
    }
}
