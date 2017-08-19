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
using badmintonDataBase.DataAccess;
using badmintonDataBase.Migrations;
using badmintonDataBase.Models;
using BadmintonWPF.Helpers.DrawsHelpers;
using BadmintonWPF.Views;
using Line = System.Windows.Shapes.Line;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace BadmintonWPF.Helpers
{
    public class DrawsFormer
    {
        #region Properties
        private Dictionary<Event, Dictionary<string, List<Rectangle>>> RectanglesTab { get; set; }
        public CalcsForDraws CalcsForDraws { get; set; }
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
            CalcsForDraws = new CalcsForDraws();
        }
        #region DrawingOnCanvas
        private void CanvasDrawing(Event eEvent, Canvas canvas)
        {
            RoundsDrawing(int.Parse(eEvent.DrawType), canvas);
            LinesDrawing(int.Parse(eEvent.DrawType), canvas);
            if (eEvent.IsDrawFormed)
            {
                int n = int.Parse(eEvent.DrawType) / 2, i = 0;
                while (n > 2)
                {
                    LinesDrawing(n, TabsWorker.CanvasDictionary[eEvent].ElementAt(i).Value);
                    AllRoundGamesForLoosersLabelsDrawing(eEvent, TabsWorker.CanvasDictionary[eEvent].ElementAt(i).Key,
                        n);
                    Rectangles_drawing_tabs(eEvent, TabsWorker.CanvasDictionary[eEvent].ElementAt(i).Value,
                        TabsWorker.CanvasDictionary[eEvent].ElementAt(i).Key, n / 2);
                    n /= 2;
                    i++;
                }
                //}
                AllRoundsLabelsDrawing(eEvent, canvas);
                FirstStageLabelsDrawing(canvas);
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
            int startNumberPers = numberPers;
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
                for (int i = 0; i < numberPers; i++) // длля горизонтальных линий
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
                } // для вертикальных линий
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
            if (startNumberPers == int.Parse(SelectedEvent.DrawType))
                ThirdPlaceDrawingLines(x1Nach, x2Nach, y1Nach, y2Nach, canvas);
        }
        private void ThirdPlaceDrawingLines(int x1, int x2, int y1, int y2, Canvas canvas)
        {
            x1 -= 120;
            x2 -= 120;
            for (int i = 0; i < 2; i++)
            {
                Line l1 = new Line();
                l1.Stroke = Brushes.Black;
                l1.StrokeThickness = 1;
                l1.X1 = x1;
                l1.X2 = x2;
                l1.Y1 = y1;
                l1.Y2 = y2;
                canvas.Children.Add(l1);
                y1 += 30;
                y2 += 30;
            }

            Line l2 = new Line();
            l2.Stroke = Brushes.Black;
            l2.StrokeThickness = 1;
            l2.X1 = x2;
            l2.X2 = x2;
            l2.Y1 = y1 - 60;
            l2.Y2 = y2 - 30;//+ 30;
            canvas.Children.Add(l2);
            x1 += 120;
            x2 += 120;
            Line l3 = new Line();
            l3.Stroke = Brushes.Black;
            l3.StrokeThickness = 1;
            l3.X1 = x1;
            l3.X2 = x2;
            l3.Y1 = y1 - 45;
            l3.Y2 = y2 - 45;
            canvas.Children.Add(l3);
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
            ThirdPlaceRectangleDrawing(eEvent, topNach + 15, left, canvas);
        }
        private void ThirdPlaceRectangleDrawing(Event eEvent, int top, int left, Canvas canvas)
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
            //top += 60 * mnozhTop;
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
            int left = 260, topNach = 75, top, yMnozh = 4, krug = CalcsForDraws.CalculateFirstStageId(eEvent) + 2;
            kolVoLudey /= 4;
            while (krug < 8)
            {
                top = topNach;
                for (int j = 1; j <= kolVoLudey; j += 2)
                {
                    int placeInDraw = CalcsForDraws.CalculatePlaceInDraw(j);
                    var selectedGame = Context.GamesTournaments.Local
                        .FirstOrDefault(p => p.StageId == krug && p.PlaceInDraw == placeInDraw &&
                                             p.EventId == eEvent.EventId && p.ForPlace == 1);
                    if (selectedGame.TeamsTournament1Id != null)
                    {
                        OneLabelDrawing(canvas, top, selectedGame.TeamsTournament1.TeamName, left); // добавдение в сетку 1 команды
                        OneLabelDrawing(canvas, top + (30 * yMnozh / 2) + 15, CalcsForDraws.ScoreConverter(selectedGame.Score), left + 170);
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
            ThirdPlaceDrawingLabels(left, topNach * 2 - 15, canvas);

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
        private void AllRoundGamesForLoosersLabelsDrawing(Event eEvent, string header, int numberOfPersons)
        {
            int kolVoLudey = numberOfPersons; //количество людей
            int left = 20, topNach = 30, top, yMnozh = 1, krug = CalcsForDraws.CalculateFirstStageId(numberOfPersons); // расчет круга и инициализациыя переменных
            int forPlace = ForPlaceCalculate(header);// расчет места
            if (forPlace == -1)//если у нас индекс > 6
                return;
            while (krug < 8)
            {
                top = topNach;
                for (int j = 1; j <= kolVoLudey; j += 2)
                {
                    int placeInDraw = CalcsForDraws.CalculatePlaceInDraw(j); // место в сетке
                    var selectedGame = Context.GamesTournaments.Local// выбранная игра, на которую нужно поместить проигравшего игрока
                        .FirstOrDefault(p => p.StageId == krug && p.PlaceInDraw == placeInDraw &&
                                             p.EventId == eEvent.EventId && p.ForPlace == forPlace);
                    if(selectedGame == null)
                        return;
                    if (selectedGame.TeamsTournament1Id != null)
                    {
                        OneLabelDrawing(TabsWorker.CanvasDictionary[eEvent][header], top, selectedGame.TeamsTournament1.TeamName, left); // добавдение в сетку 1 команды
                        OneLabelDrawing(TabsWorker.CanvasDictionary[eEvent][header], top + (30 * yMnozh / 2) + 15, CalcsForDraws.ScoreConverter(selectedGame.Score), left + 170);//добавляем счет
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
        private void ThirdPlaceDrawingLabels(int left, int topNach, Canvas canvas)
        {
            var selectedGame = Context.GamesTournaments.Local
                .FirstOrDefault(p => p.StageId == 7 && p.PlaceInDraw == 1 &&
                                     p.EventId == SelectedEvent.EventId && p.ForPlace == 3);
            if (selectedGame.TeamsTournament1Id != null)
            {
                OneLabelDrawing(canvas, topNach, selectedGame.TeamsTournament1.TeamName, left); // добавдение в сетку 1 команды
                OneLabelDrawing(canvas, topNach + 30, CalcsForDraws.ScoreConverter(selectedGame.Score), left + 170);
            }
            topNach += 30;
            if (selectedGame.TeamsTournament2Id != null)
                OneLabelDrawing(canvas, topNach, selectedGame.TeamsTournament2.TeamName, left); // добавление на сетку 2 команды




            var winner = Context.GamesTournaments.Local
                .FirstOrDefault(p => p.StageId == 8 && p.PlaceInDraw == 1 && p.EventId == SelectedEvent.EventId &&
                                     p.ForPlace == 3);
            if (winner.TeamsTournament1Id != null)
                OneLabelDrawing(canvas, topNach - 15, winner.TeamsTournament1.TeamName, left + 120);
        }
        private void ThirdPlaceResults()
        {
            try
            {
                var team1 = Context.GamesTournaments.Local
                    .FirstOrDefault(p => p.ForPlace == 3 && p.PlaceInDraw == 1 &&
                                         p.StageId == 7 && p.EventId == SelectedEvent.EventId).TeamsTournament1;//первая команда;
                var team2 = Context.GamesTournaments.Local
                    .FirstOrDefault(p => p.ForPlace == 3 && p.PlaceInDraw == 1 &&
                                         p.StageId == 7 && p.EventId == SelectedEvent.EventId).TeamsTournament2;//вторая команда
                if (team1 != null && team2 != null)
                {
                    MatchInfo matchInfo = MatchDialogInfoCreator(team1, team2);
                    if (matchInfo.WinnerChanged)//если мы определили победителя
                    {
                        var selectedGame = Context.GamesTournaments.Local
                            .FirstOrDefault(p => p.ForPlace == 3 && p.PlaceInDraw == 1 &&
                                                 p.StageId == 8 && p.EventId == SelectedEvent.EventId);//следующая игра
                        int winnerButton;
                        if (matchInfo.radioButton1.IsChecked == true)//если выиграл первый игрок
                        {
                            winnerButton = 1;
                            selectedGame.TeamsTournament1Id = team1.TeamsTournamentId;//если на место первого нужно рисовать по расчетам выше
                        }
                        else//если победил второй
                        {
                            winnerButton = 2;
                            selectedGame.TeamsTournament1Id = team2.TeamsTournamentId;//если на место первого нужно рисовать по расчетам выше
                        }
                        Context.GamesTournaments.Local.FirstOrDefault(
                            p => p.TeamsTournament1Id == team1.TeamsTournamentId &&
                                 p.TeamsTournament2Id == team2.TeamsTournamentId && p.EventId == SelectedEvent.EventId).Score = CalcsForDraws.WinnerHelperSplitter(winnerButton, matchInfo.Winner);//заносим счет в бд
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
        }
        #endregion

        #region EventsHandler and EventChanger
        public void EventChanged(Canvas canvas)
        {
            try
            {
                canvas.Children.Clear();
                TabsWorker.TabsCreatorRemover(SelectedEvent);
                CanvasDrawing(SelectedEvent, canvas);
            }
            catch
            {
                MessageBox.Show("Возникла ошибка", "Невозможно отобразить сетку", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
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
                    int placeInDraw = CalcsForDraws.CalculatePlaceInDraw(i, numberOfPeople);
                    int round = CalcsForDraws.CalculateRoundForRectangle(i, numberOfPeople);

                    var whichPlayer = i % 2 == 0 ? 1 : 2;
                    int precedentNumber = CalcsForDraws.CalculatePrecedentGame(i, placeInDraw);
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
                                CalcsForDraws.CalculateMarginForNeededLabel(ref left, ref topNach, ref yMnozh, round, whichPlayer,
                                    placeInDraw, SelectedEvent);
                                var selectedGame = Context.GamesTournaments.Local
                                   .FirstOrDefault(p => p.ForPlace == 1 && p.PlaceInDraw == placeInDraw &&
                                                        p.StageId == round && p.EventId == SelectedEvent.EventId);//следующая игра
                                var winnerButton = WinnerDeterminer(SelectedCanvas, matchInfo, topNach, left, whichPlayer, team1, team2, selectedGame, CalcsForDraws.WhichHeaderChooseForLoosers(round - 1));
                                Context.GamesTournaments.Local.FirstOrDefault(
                                    p => p.TeamsTournament1Id == team1.TeamsTournamentId &&
                                         p.TeamsTournament2Id == team2.TeamsTournamentId && p.EventId == SelectedEvent.EventId).Score = CalcsForDraws.WinnerHelperSplitter(winnerButton, matchInfo.Winner);//заносим счет в бд
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
                    if (FirstConcelationXCheker(1) /*&& SelectedEvent.Type.TypeName.Equals("Одиночка")*/)
                    {
                        InvokeXCreator(XFirstConcelationDrawer(
                            ForPlaceCalculate((TabsWorker.TabControl.Items[1] as TabItem).Header.ToString()),
                            round));
                    }

                }
            }
            if ((sender as Rectangle) == DictRectangles[SelectedEvent][numberOfPeople - 1])
                ThirdPlaceResults();
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
                    int placeInDraw = CalcsForDraws.CalculatePlaceInDraw(i, kolVoLudey);
                    int round = CalcsForDraws.CalculateRoundForRectangle(i, kolVoLudey);
                    var whichPlayer = i % 2 == 0 ? 1 : 2;
                    int precedentNumber = CalcsForDraws.CalculatePrecedentGame(i, placeInDraw);
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
                                CalcsForDraws.CalculateMarginForNeededLabel(ref left, ref topNach, ref yMnozh, round - tabsCount, whichPlayer, placeInDraw, SelectedEvent);
                                var selectedGame = Context.GamesTournaments.Local
                                    .FirstOrDefault(p => p.ForPlace == forPlace && p.PlaceInDraw == placeInDraw &&
                                                         p.StageId == round && p.EventId == SelectedEvent.EventId);//следующая игра
                                var winnerButton = WinnerDeterminer(TabsWorker.CanvasDictionary[SelectedEvent][header], matchInfo, topNach, left, whichPlayer, team1, team2, selectedGame, CalcsForDraws.WhichHeaderChooseForLoosers(round - 1));
                                Context.GamesTournaments.Local.FirstOrDefault(
                                    p => p.TeamsTournament1Id == team1.TeamsTournamentId &&
                                         p.TeamsTournament2Id == team2.TeamsTournamentId && p.EventId == SelectedEvent.EventId).Score = CalcsForDraws.WinnerHelperSplitter(winnerButton, matchInfo.Winner);//заносим счет в бд
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
        /// <summary>
        /// Формирование в БД полной основной сетки, включая все круги
        /// </summary>
        /// <param name="numsForDraw">Макет номеров посева игроков</param>
        public void FirstRoundGamesFormer(List<int> numsForDraw)
        {
            for (int i = 1; i <= numsForDraw.Count; i += 2)//формирование первого круга
            {
                GamesTournament gamesTournament = new GamesTournament();
                gamesTournament.ForPlace = 1;
                gamesTournament.PlaceInDraw = (i / 2) + 1;
                TeamsTournament teamsTournament1 = Context.TeamsTournaments.Local.FirstOrDefault
                    (p => p.EventId == SelectedEvent.EventId && p.SeedingNumber == numsForDraw[i - 1]);
                if (teamsTournament1 != null)
                    gamesTournament.TeamsTournament1Id = teamsTournament1.TeamsTournamentId;
                TeamsTournament teamsTournament2 = Context.TeamsTournaments.Local.FirstOrDefault
                    (p => p.EventId == SelectedEvent.EventId && p.SeedingNumber == numsForDraw[i]);
                if (teamsTournament2 != null)
                    gamesTournament.TeamsTournament2Id = teamsTournament2.TeamsTournamentId;
                gamesTournament.StageId = CalcsForDraws.CalculateFirstStageId(SelectedEvent);
                gamesTournament.EventId = SelectedEvent.EventId;
                Context.GamesTournaments.Local.Add(gamesTournament);
            }
            int numberPlace = CalcsForDraws.CalculateFirstStageId(SelectedEvent) + 1, draw = numsForDraw.Count / 2; //формирование 2 и дальше кргов до победителя
            while (numberPlace <= 7)
            {
                for (int i = 1; i <= draw; i += 2)
                {
                    GamesTournament gamesTournament = GamesTournamentFormer(1, (i / 2) + 1, numberPlace);
                    Context.GamesTournaments.Local.Add(gamesTournament);
                }
                numberPlace++;
                draw /= 2;
            }
            GamesTournament gamesTournamentLast = GamesTournamentFormer(1, 1, 8); // победитель
            Context.GamesTournaments.Local.Add(GamesTournamentFormer(3, 1, 7));
            Context.GamesTournaments.Local.Add(GamesTournamentFormer(3, 1, 8));
            Context.GamesTournaments.Local.Add(gamesTournamentLast);
            Context.SaveChanges();
        }
        private GamesTournament GamesTournamentFormer(int forPlace, int placeInDraw, int stageId)
        {
            GamesTournament gamesTournament = new GamesTournament();
            gamesTournament.ForPlace = forPlace;
            gamesTournament.PlaceInDraw = placeInDraw;
            gamesTournament.StageId = stageId;
            gamesTournament.EventId = SelectedEvent.EventId;
            return gamesTournament;
        }
        /// <summary>
        /// Рисование первого круга сетки, занос в базу победителей Х и рисуем победителей
        /// </summary>
        /// <param name="canvas">Канвас, на котором рисуем</param>
        private void FirstStageLabelsDrawing(Canvas canvas)
        {
            var firstStage = CalcsForDraws.CalculateFirstStageId(SelectedEvent);//круг в БД
            var gamesToDraw = Context.GamesTournaments.Local.Where(p => p.EventId == SelectedEvent.EventId && p.StageId == firstStage && p.ForPlace == 1)
                .ToList();//какие встречи нужно нарисовать
            var gamesToDrawSecondX = Context.GamesTournaments.Local.Where(p => p.EventId == SelectedEvent.EventId && p.StageId == firstStage + 1 && p.ForPlace == 1)
                .ToList(); //если игрок попал на Х его нужно сразу нарисовать как победителя
            int y = 30, score1Y = 60; // отступ сверху для имени игрока и для счета
            foreach (var gamesTournament in gamesToDraw)
            {
                if (gamesTournament.TeamsTournament1Id != null)
                {
                    OneLabelDrawing(canvas, y, gamesTournament.TeamsTournament1.TeamName, 20);//рисуем
                    y += 30;
                }
                else
                {
                    GamesTournamentXCreating(gamesTournament, 1); // определяем  и заносим победителя в БД
                    OneLabelDrawing(canvas, y, "X", 20); // рисуем Х 
                    y += 30;
                }
                OneLabelDrawing(canvas, score1Y, CalcsForDraws.ScoreConverter(gamesTournament.Score), 190);
                score1Y += 60;
                if (gamesTournament.TeamsTournament2Id != null)
                {
                    OneLabelDrawing(canvas, y, gamesTournament.TeamsTournament2.TeamName, 20);
                    y += 30;
                }
                else
                {
                    GamesTournamentXCreating(gamesTournament, 2); // определяем  и заносим победителя в БД
                    OneLabelDrawing(canvas, y, "X", 20);
                    y += 30;
                }

            }
            int y2 = 45, scoreY = 90;
            foreach (var gamesTournament in gamesToDrawSecondX) // рисуем победителей в первом круге, которые выиграли у Х
            {
                if (gamesTournament.TeamsTournament1Id != null)
                    OneLabelDrawing(canvas, y2, gamesTournament.TeamsTournament1.TeamName, 140);
                y2 += 60;
                OneLabelDrawing(canvas, scoreY, CalcsForDraws.ScoreConverter(gamesTournament.Score), 310);
                scoreY += 120;
                if (gamesTournament.TeamsTournament2Id != null)
                    OneLabelDrawing(canvas, y2, gamesTournament.TeamsTournament2.TeamName, 140);
                y2 += 60;
            }
        }
        /// <summary>
        /// Расчет иксов и победителей
        /// </summary>
        /// <param name="gamesTournament">игра, в которой есть Х</param>
        /// <param name="xPlayerIs">номер в паре (1 или 2) икса</param>
        private void GamesTournamentXCreating(GamesTournament gamesTournament, int xPlayerIs)
        {

            int placeInDraw = CalcsForDraws.CalculatePlaceInDraw(gamesTournament.PlaceInDraw); // расчет позиции в сетке
            var whichPlayer = gamesTournament.PlaceInDraw % 2 == 0 ? 2 : 1; //на какое место ставить игрока
            int? selectedTeamId; // проигравший игрок
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
            LooserWritingToDataBase(CalcsForDraws.WhichHeaderChooseForLoosers((int)gamesTournament.StageId), (int)gameToChange.StageId, placeInDraw, teamToAdd, whichPlayer);//добавляем победителя в БД
            if (/*SelectedEvent.Type.TypeName.Equals("Одиночка") &&*/ gamesTournament.ForPlace == 1)
                XValuesConcelationCalc(CalcsForDraws.WhichHeaderChooseForLoosers((int)gamesTournament.StageId), (int)gameToChange.StageId - 1, whichPlayer, placeInDraw);
            Context.SaveChanges();
        }
        #endregion

        #region ConvertersHelpers
        /// <summary>
        /// Определитель победителя
        /// </summary>
        /// <param name="canvas">Канвас, на котором рисуем</param>
        /// <param name="matchInfo">инфо о матче</param>
        /// <param name="top">отступ сверху</param>
        /// <param name="left">отступ слева</param>
        /// <param name="whichPlayer">на место какого игрока</param>
        /// <param name="team1">первый игрок</param>
        /// <param name="team2">второй игрок</param>
        /// <param name="selectedGame">выбранная игра</param>
        /// <param name="header">заголовок сетки</param>
        /// <returns></returns>
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
            if (/*SelectedEvent.Type.TypeName.Equals("Одиночка") && */selectedGame.ForPlace == 1)//для 
                LooserWritingToDataBase(header, (int)selectedGame.StageId, selectedGame.PlaceInDraw, selectedTeam, whichPlayer);
            else if ((SelectedEvent.Type.TypeName.Equals("Пара") || SelectedEvent.Type.TypeName.Equals("Микст")) && selectedGame.ForPlace == 1)
                DoublesMixtesDatabaseWriter(header, (int)selectedGame.StageId, selectedGame.PlaceInDraw, selectedTeam, whichPlayer);
            return winnerButton;
        }

        private void DoublesMixtesDatabaseWriter(string header, int round, int placeInDraw, TeamsTournament teamToAdd, int whichPlayerWon)
        {
            int forPlace;
            if (round == 7)
                forPlace = 3;
            else
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
        /// <summary>
        /// Диалог для ввода счета
        /// </summary>
        /// <param name="team1"></param>
        /// <param name="team2"></param>
        /// <returns></returns>
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

        #endregion

        #region LoosersDraws
        public void GamesForLoosersFormer(Event eEvent, int numberOfPeople, int forPlace)
        {
            int numberPlace = CalcsForDraws.CalculateFirstStageId(numberOfPeople), draw = numberOfPeople;
            while (numberPlace <= 7)
            {
                for (int i = 1; i <= draw; i += 2)
                {
                    GamesTournament gamesTournament = GamesTournamentFormer(forPlace, (i / 2) + 1, numberPlace);
                    Context.GamesTournaments.Local.Add(gamesTournament);
                }
                numberPlace++;
                draw /= 2;
            }
            GamesTournament gamesTournamentLast = GamesTournamentFormer(forPlace, 1, 8);
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
            return int.Parse(splitter[1]); // берем только место
        }
        private void LooserWritingToDataBase(string header, int round, int placeInDraw, TeamsTournament teamToAdd, int whichPlayerWon)
        {
            if (teamToAdd == null)
                return;
            int forPlace = ForPlaceCalculate(header);
            if (forPlace == -1)
            {
                if (round == 7)
                    forPlace = 3;
                else
                    return;
            }
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
            CalcsForDraws.CalculateMarginForNeededLabel(ref left, ref topNach, ref yMnozh, round, whichPlayer, placeInDraw, SelectedEvent);
            OneLabelDrawing(TabsWorker.CanvasDictionary[SelectedEvent][header], topNach, "X", left); // отображаем
        }
        private bool FirstConcelationXCheker(int forPlace)
        {
            var secondStage = CalcsForDraws.CalculateFirstStageId(int.Parse(SelectedEvent.DrawType) / 2);
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
        #endregion
    }
}