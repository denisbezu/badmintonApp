using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using badmintonDataBase.DataAccess;
using badmintonDataBase.Models;
using BadmintonWPF.Helpers.DrawsHelpers;
using Microsoft.Win32;
using Excel = Microsoft.Office.Interop.Excel;
using Type = System.Type;

namespace BadmintonWPF.Helpers
{
    public class ReportFormer
    {
        private Excel.Application ExcelApplication { get; set; }
        private Excel.Workbooks Workbooks { get; set; }
        private Excel.Workbook Workbook { get; set; }
        public Excel.Sheets Sheets { get; set; }
        public Excel.Worksheet Worksheet { get; set; }
        private BadmintonContext Context { get; set; }
        public Tournament Tournament { get; set; }
        public Category Category { get; set; }

        public ReportFormer(Tournament tournament)
        {
            Context = new BadmintonContext();
            Tournament = tournament;
            int id = Tournament.TournamentId;

            Context.TeamsTournaments.Where(p => p.Event.TournamentId == id).Load();
            Context.PlayersTeams.Where(p => p.TeamsTournament.Event.TournamentId == id).Load();
            Context.Players.Load();
        }

        public bool OpenWorkbook()
        {
            var result = SaveFilePath();
            if (result == null)
                return false;
            ExcelApplication = new Excel.Application();
            //Получаем набор объектов Workbook (массив ссылок на созданные книги)
            Workbooks = ExcelApplication.Workbooks;
            ExcelApplication.SheetsInNewWorkbook = 11;
            ExcelApplication.Workbooks.Add(Type.Missing);
            Sheets = ExcelApplication.Sheets;
            //Открываем книгу и получаем на нее ссылку
            Workbook = ExcelApplication.Workbooks[1];
            Workbook.SaveAs(result);
            ExcelApplication.Visible = true;
            return true;
        }

        public string SaveFilePath()
        {
            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.Filter = "xml-файл (*.xml)|*.xml";
            saveDlg.FileName = Tournament.TournamentName + " " + Category.CategoryName + "(" +
                               DateTime.Now.ToString("HH.mm.ss") + ")" + ".xlsx";
            Nullable<bool> result = saveDlg.ShowDialog();
            string mainFileName = "";
            if (result == true)
            {
                mainFileName = saveDlg.FileName;
            }
            else
            {
                return null;
            }
            return mainFileName;
        }

        public void WriteHeaderPlayersList()
        {
            Excel.Range excelRange;
            (Sheets[1] as Excel.Worksheet).Name = "Players List";
            Worksheet = Sheets.get_Item(1);
            (Worksheet.get_Range("H1", "H1")).EntireColumn.ColumnWidth = 25;
            (Worksheet.get_Range("A1", "A1")).EntireColumn.ColumnWidth = 5;
            (Worksheet.get_Range("B1", "B1")).EntireColumn.ColumnWidth = 20;
            (Worksheet.get_Range("C1", "C1")).EntireColumn.ColumnWidth = 7;
            (Worksheet.get_Range("D1", "D1")).EntireColumn.ColumnWidth = 10;
            (Worksheet.get_Range("E1", "E1")).EntireColumn.ColumnWidth = 18;
            (Worksheet.get_Range("F1", "F1")).EntireColumn.ColumnWidth = 20;
            (Worksheet.get_Range("G1", "G1")).EntireColumn.ColumnWidth = 10;
            (Worksheet.get_Range("A1", "Z1000")).Font.Size = 12;
            //название шрифта
            (Worksheet.get_Range("A1", "Z100")).Font.Name = "Times New Roman";
            excelRange = Worksheet.get_Range("A1", "H1");
            excelRange.Merge(Type.Missing);
            excelRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            excelRange.Font.Bold = 1;
            excelRange.Value2 = ("СПИСОК УЧАСНИКІВ ТУРНИРУ " + Tournament.TournamentName + " " + Category.CategoryName)
                .ToUpper();
            excelRange = Worksheet.get_Range("B2", "B2");
            excelRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            excelRange.Value2 = "м. " + Tournament.City.CityName;
            excelRange = Worksheet.get_Range("H2", "H2");
            excelRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            excelRange.Value2 = Tournament.StartDate.ToString("dd.MM") + "-" + Tournament.FinishDate.ToString("dd.MM") +
                                " " + Tournament.FinishDate.Year + " року";
            excelRange = Worksheet.get_Range("A3", "H3");
            excelRange.Merge(Type.Missing);
            excelRange.Font.Bold = 1;
            excelRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            excelRange.Value2 = "ЧОЛОВІКИ";
        }
        public void WriteHeaderWS1(Excel.Worksheet worksheet, string rangeFirst, string rangeSecond)
        {
            Excel.Range excelRange;
            excelRange = worksheet.get_Range(rangeFirst, rangeSecond);
            excelRange.Font.Bold = 1;
            excelRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            excelRange.Borders.ColorIndex = 1;
            excelRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            excelRange.Cells[Type.Missing, 1].Value2 = "№";
            excelRange.Cells[Type.Missing, 2].Value2 = "Прізвище, ім'я";
            excelRange.Cells[Type.Missing, 3].Value2 = "Р.Н.";
            excelRange.Cells[Type.Missing, 4].Value2 = "Розряд";
            excelRange.Cells[Type.Missing, 5].Value2 = "Місто";
            excelRange.Cells[Type.Missing, 6].Value2 = "Школа, клуб, тощо";
            excelRange.Cells[Type.Missing, 7].Value2 = "Спілка";
            excelRange.Cells[Type.Missing, 8].Value2 = "Тренер";
        }
        public int PlayerListFormer(string rangeFirst, string rangeSecond, string sort)
        {
            Excel.Range excelRange;
            excelRange = Worksheet.get_Range(rangeFirst, rangeSecond);
            var playersTeams = Context.PlayersTeams.Local
                .Where(p => p.TeamsTournament.Event.Type.TypeName.Equals("Одиночка") &&
                            p.TeamsTournament.Event.Category.CategoryName.Equals(Category.CategoryName)
                            && p.TeamsTournament.Event.Sort == sort
                            && p.TeamsTournament.Event.TournamentId == Tournament.TournamentId)
                .Select(p => p.Player).OrderBy(p => p.PlayerSurName);
            int row = 1;
            foreach (var player in playersTeams.ToList())
            {
                excelRange.Cells[row, 1].Value2 = row;
                excelRange.Cells[row, 2].Value2 = player.PlayerSurName + " " + player.PlayerName;
                excelRange.Cells[row, 3].Value2 = player.YearOfBirth;
                excelRange.Cells[row, 4].Value2 = player.Grade.GradeName;
                excelRange.Cells[row, 5].Value2 = player.City.CityName;
                excelRange.Cells[row, 6].Value2 = player.Club.ClubName;
                excelRange.Cells[row, 7].Value2 = player.Union.UnionName;
                excelRange.Cells[row, 8].Value2 = player.Coach.CoachName;
                row++;
            }
            rangeSecond = rangeSecond.Substring(1);
            string range2 = "H" + (row + int.Parse(rangeSecond));
            excelRange = Worksheet.get_Range(rangeFirst, range2);
            excelRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            excelRange = Worksheet.get_Range("B" + int.Parse(rangeSecond), "B" + (int.Parse(rangeSecond) + row));
            excelRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
            if (sort == "Юноши")
            {
                excelRange = Worksheet.get_Range("A" + (row + int.Parse(rangeSecond)),
                    "H" + (row + int.Parse(rangeSecond)));
                excelRange.Merge(Type.Missing);
                excelRange.Font.Bold = 1;
                excelRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                excelRange.Value2 = "ЖІНКИ";
            }

            return playersTeams.Count();
        }
        public void JudgeWriter(Excel.Worksheet worksheet, string rangeFirst, string rangeSecond)
        {
            Excel.Range excelRange;
            excelRange = worksheet.get_Range(rangeFirst, rangeSecond);
            int first = int.Parse(rangeFirst.Substring(1));
            int second = int.Parse(rangeSecond.Substring(1));
            Excel.Range excelRangeToMerge = worksheet.get_Range("E" + first, "F" + second);
            excelRangeToMerge.Merge(Type.Missing);
            excelRangeToMerge.Value2 = "Головний суддя";
            excelRange.Cells[Type.Missing, 3] = Tournament.Judge.ToString();
            excelRangeToMerge = worksheet.get_Range("E" + (first + 2), "F" + (second + 2));
            excelRangeToMerge.Merge(Type.Missing);
            excelRangeToMerge.Value2 = "Головний секретар";
        }

        public void WriteHeaderResults(Excel.Worksheet worksheet, string name)
        {
            Excel.Range excelRange;
            worksheet.Name = name;
            //worksheet = Sheets.get_Item(1);
            (worksheet.get_Range("H1", "H1")).EntireColumn.ColumnWidth = 10;
            (worksheet.get_Range("A1", "A1")).EntireColumn.ColumnWidth = 5;
            (worksheet.get_Range("B1", "B1")).EntireColumn.ColumnWidth = 20;
            (worksheet.get_Range("C1", "C1")).EntireColumn.ColumnWidth = 10;
            (worksheet.get_Range("D1", "D1")).EntireColumn.ColumnWidth = 7;
            (worksheet.get_Range("E1", "E1")).EntireColumn.ColumnWidth = 10;
            (worksheet.get_Range("F1", "F1")).EntireColumn.ColumnWidth = 18;
            (worksheet.get_Range("G1", "G1")).EntireColumn.ColumnWidth = 20;
            (worksheet.get_Range("I1", "I1")).EntireColumn.ColumnWidth = 25;
            (worksheet.get_Range("A1", "Z1000")).Font.Size = 12;
            //название шрифта
            (worksheet.get_Range("A1", "Z100")).Font.Name = "Times New Roman";
            excelRange = worksheet.get_Range("A1", "I1");
            excelRange.Merge(Type.Missing);
            excelRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            excelRange.Font.Bold = 1;
            excelRange.Value2 = ("РЕЗУЛЬТАТИ ТУРНИРУ " + Tournament.TournamentName + " " + Category.CategoryName)
                .ToUpper();
            excelRange = worksheet.get_Range("B2", "B2");
            excelRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            excelRange.Value2 = "м. " + Tournament.City.CityName;
            excelRange = worksheet.get_Range("I2", "I2");
            excelRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            excelRange.Value2 = Tournament.StartDate.ToString("dd.MM") + "-" + Tournament.FinishDate.ToString("dd.MM") +
                                " " + Tournament.FinishDate.Year + " року";
            //excelRange = worksheet.get_Range("A3", "H3");
            //excelRange.Merge(Type.Missing);
            //excelRange.Font.Bold = 1;
            //excelRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            //excelRange.Value2 = "ЧОЛОВІКИ";
        }
        public void WriteHeaderNameResults(Excel.Worksheet worksheet, string categoryName)
        {
            Excel.Range excelRange;
            excelRange = worksheet.get_Range("A3", "I3");
            excelRange.Merge(Type.Missing);
            excelRange.Font.Bold = 1;
            excelRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            excelRange.Value2 = categoryName + " КАТЕГОРІЯ";
        }
        public void WriteHeaderForTableResults(Excel.Worksheet worksheet, string rangeFirst, string rangeSecond)
        {
            Excel.Range excelRange;
            excelRange = worksheet.get_Range(rangeFirst, rangeSecond);
            excelRange.Font.Bold = 1;
            excelRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            excelRange.Borders.ColorIndex = 1;
            excelRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            excelRange.Cells[Type.Missing, 1].Value2 = "№";
            excelRange.Cells[Type.Missing, 2].Value2 = "Прізвище, ім'я";
            excelRange.Cells[Type.Missing, 3].Value2 = "Місце";
            excelRange.Cells[Type.Missing, 4].Value2 = "Р.Н.";
            excelRange.Cells[Type.Missing, 5].Value2 = "Розряд";
            excelRange.Cells[Type.Missing, 6].Value2 = "Місто";
            excelRange.Cells[Type.Missing, 7].Value2 = "Школа, клуб, тощо";
            excelRange.Cells[Type.Missing, 8].Value2 = "Спілка";
            excelRange.Cells[Type.Missing, 9].Value2 = "Тренер";
        }
        public int ResultsPlayersWithPlaces(Excel.Worksheet worksheet, int sheetID)
        {
            Dictionary<Player, int> allPlaces = CalculatePlace(EventBySheetId(sheetID));
            Excel.Range excelRange;
            excelRange = worksheet.get_Range("A6", "I6");
            int row = 1;
            foreach (var pair in allPlaces)
            {
                excelRange.Cells[row, 1].Value2 = row;
                excelRange.Cells[row, 2].Value2 = pair.Key.PlayerSurName + " " + pair.Key.PlayerName;
                excelRange.Cells[row, 3].Value2 = pair.Value;
                excelRange.Cells[row, 4].Value2 = pair.Key.YearOfBirth;
                excelRange.Cells[row, 5].Value2 = pair.Key.Grade.GradeName;
                excelRange.Cells[row, 6].Value2 = pair.Key.City.CityName;
                excelRange.Cells[row, 7].Value2 = pair.Key.Club.ClubName;
                excelRange.Cells[row, 8].Value2 = pair.Key.Union.UnionName;
                excelRange.Cells[row, 9].Value2 = pair.Key.Coach.CoachName;
                row++;
            }
            string range2 = "I" + (row + 6);
            excelRange = worksheet.get_Range("A6", range2);
            excelRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            excelRange = worksheet.get_Range("B" + 6, "B" + (6 + row));
            excelRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
            return allPlaces.Count;
        }
        private Dictionary<Player, int> CalculatePlace(Event eEvent)
        {
            if (eEvent.EventId == 0) return new Dictionary<Player, int>();
            Dictionary<Player, int> placesPlayers = new Dictionary<Player, int>();
            Context.GamesTournaments.Load();
            var winnerChecker = Context.GamesTournaments.Local.FirstOrDefault(p =>
                p.EventId == eEvent.EventId && p.ForPlace == 1 && p.StageId == 8);
            if (winnerChecker != null)
            {
                var winner = winnerChecker
                    .TeamsTournament1Id;
                if (winner != null)
                    DictionaryAdd(ref placesPlayers, 1, GetPlayersFromGamesTournament((int)winner));

                var seconChecker = Context.GamesTournaments.Local.FirstOrDefault(p =>
                    p.EventId == eEvent.EventId && p.ForPlace == 1 && p.StageId == 7);
                var secondPlace = seconChecker?.TeamsTournament1Id;
                if (secondPlace == winner)
                {
                    secondPlace = seconChecker
                        .TeamsTournament2Id;
                    if (secondPlace != null)
                        DictionaryAdd(ref placesPlayers, 2, GetPlayersFromGamesTournament((int)secondPlace));
                }
                var thirdChecker = Context.GamesTournaments.Local.FirstOrDefault(p =>
                    p.EventId == eEvent.EventId && p.ForPlace == 3 && p.StageId == 8);
                if (thirdChecker != null)
                {
                    var thirdPlace = thirdChecker
                        .TeamsTournament1Id;
                    if (thirdPlace != null)
                        DictionaryAdd(ref placesPlayers, 3, GetPlayersFromGamesTournament((int)thirdPlace));
                    var fourthCheker = Context.GamesTournaments.Local.FirstOrDefault(p =>
                        p.EventId == eEvent.EventId && p.ForPlace == 3 && p.StageId == 7);
                    if (fourthCheker != null)
                    {
                        var fourthPlace = fourthCheker
                            .TeamsTournament1Id;
                        if (fourthPlace == thirdPlace)
                        {
                            fourthPlace = fourthCheker.TeamsTournament2Id;
                            if (winner != null && fourthPlace != null)
                                DictionaryAdd(ref placesPlayers, 4, GetPlayersFromGamesTournament((int)fourthPlace));
                        }
                    }
                }
            }
            var fifthPlaces = Context.GamesTournaments.Local.Where(p =>
                p.EventId == eEvent.EventId && p.ForPlace == 5).ToList();
            foreach (var playerFifthPlace in fifthPlaces)
            {
                if (playerFifthPlace.TeamsTournament1Id != null)
                    DictionaryAdd(ref placesPlayers, 5, GetPlayersFromGamesTournament((int)playerFifthPlace.TeamsTournament1Id));
                if (playerFifthPlace.TeamsTournament2Id != null)
                    DictionaryAdd(ref placesPlayers, 5, GetPlayersFromGamesTournament((int)playerFifthPlace.TeamsTournament2Id));
            }
            if (int.Parse(eEvent.DrawType) >= 16)
            {
                var ninethPlaces = Context.GamesTournaments.Local.Where(p =>
                    p.EventId == eEvent.EventId && p.ForPlace == 9).ToList();
                foreach (var ninethPlayer in ninethPlaces)
                {
                    if (ninethPlayer.TeamsTournament1Id != null)
                        DictionaryAdd(ref placesPlayers, 9, GetPlayersFromGamesTournament((int)ninethPlayer.TeamsTournament1Id));
                    if (ninethPlayer.TeamsTournament2Id != null)
                        DictionaryAdd(ref placesPlayers, 9, GetPlayersFromGamesTournament((int)ninethPlayer.TeamsTournament2Id));
                }
            }
            if (int.Parse(eEvent.DrawType) >= 32)
            {
                var seventeenthPlaeyrs = Context.GamesTournaments.Local.Where(p =>
                    p.EventId == eEvent.EventId && p.ForPlace == 17).ToList();
                foreach (var seventeenthPlaeyr in seventeenthPlaeyrs)
                {
                    if (seventeenthPlaeyr.TeamsTournament1Id != null)
                        DictionaryAdd(ref placesPlayers, 17, GetPlayersFromGamesTournament((int)seventeenthPlaeyr.TeamsTournament1Id));
                    if (seventeenthPlaeyr.TeamsTournament2Id != null)
                        DictionaryAdd(ref placesPlayers, 17, GetPlayersFromGamesTournament((int)seventeenthPlaeyr.TeamsTournament2Id));
                }
            }
            if (int.Parse(eEvent.DrawType) >= 64)
            {
                var thirtythirdpPlayers = Context.GamesTournaments.Local.Where(p =>
                    p.EventId == eEvent.EventId && p.ForPlace == 17).ToList();
                foreach (var thirtyThirdPlayer in thirtythirdpPlayers)
                {
                    if (thirtyThirdPlayer.TeamsTournament1Id != null)
                        DictionaryAdd(ref placesPlayers, 33, GetPlayersFromGamesTournament((int)thirtyThirdPlayer.TeamsTournament1Id));
                    if (thirtyThirdPlayer.TeamsTournament2Id != null)
                        DictionaryAdd(ref placesPlayers, 33, GetPlayersFromGamesTournament((int)thirtyThirdPlayer.TeamsTournament2Id));
                }
            }
            return placesPlayers;
        }
        private List<Player> GetPlayersFromGamesTournament(int teamsTournamentId)
        {
            List<Player> players = new List<Player>();
            var allPlayers = Context.PlayersTeams.Local.Where(p => p.TeamsTournamentId == teamsTournamentId)
                .Select(p => p.Player).ToList();
            players = allPlayers;
            return players;
        }
        private void DictionaryAdd(ref Dictionary<Player, int> placesPlayers, int place, List<Player> players)
        {
            foreach (var player in players)
            {
                if (!placesPlayers.ContainsKey(player))
                    placesPlayers.Add(player, place);
            }
        }

        public void DrawFullSheet(Excel.Worksheet worksheet)
        {
            var eEvent = EventBySheetId(worksheet.Index);
            if(eEvent.EventId != 0 && eEvent.IsDrawFormed)
            {
                var looserDraw = ExcelHelper.GetLooserDraw(eEvent);
                LinesHorizontalDrawing(worksheet, "A5", EventBySheetId(worksheet.Index)?.DrawType, 1);
                LinesVerticalDrawing(worksheet, "A6", EventBySheetId(worksheet.Index)?.DrawType, 1);
                if (EventBySheetId(worksheet.Index) != null)
                {
                    int currenRow = 5 + (int.Parse(EventBySheetId(worksheet.Index).DrawType) * 2) + 4;
                    foreach (var looserD in looserDraw)
                    {
                        LinesHorizontalDrawing(worksheet, "A" + currenRow, (looserD -1).ToString(), looserD);
                        LinesVerticalDrawing(worksheet, "A" + (currenRow + 1), (looserD - 1).ToString(), looserD);
                        currenRow += looserD * 2 + 2;
                    }
                }
            }
        }

        public void WriteDrawHeader(Excel.Worksheet worksheet, string name)
        {
            (worksheet.get_Range("A1", "Z1")).EntireColumn.ColumnWidth = 18;
            Excel.Range excelRange;
            worksheet.Cells.Font.Name = "Times New Roman";
            worksheet.Name = "Draw " + name;
            worksheet.Select(Type.Missing);
            ExcelApplication.ActiveWindow.DisplayGridlines = false;
            excelRange = worksheet.get_Range("A1", "C1");
            excelRange.Merge(Type.Missing);
            excelRange.Font.Bold = 1;
            excelRange.Font.Size = 12;
            excelRange.Value2 = Tournament.TournamentName.ToUpper();
            excelRange = worksheet.get_Range("A2", "B2");
            excelRange.Merge(Type.Missing);
            excelRange.Font.Bold = 1;
            excelRange.Font.Size = 12;
            if (Category.CategoryName != "Взрослые")
                excelRange.Value2 = name + " " + Category.CategoryName + " р.н.";
            else
                excelRange.Value2 = name + " " + Category.CategoryName;
            excelRange = worksheet.get_Range("A3", "H3");


            ExcelHelper excelHelper = new ExcelHelper();
            var drawType = EventBySheetId(worksheet.Index)?.DrawType;
            if (drawType != null)
            {
                var a = int.Parse(drawType);
                var listRounds = excelHelper.RoundsDrawing(a);
                int row = 1;
                int column = 1;
                foreach (var round in listRounds)
                {
                    excelRange.Cells[row, column].Value2 = round;
                    excelRange.Cells[row, column].Borders[Excel.XlBordersIndex.xlEdgeBottom].ColorIndex = 1;
                    excelRange.Cells[row, column].Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                    column++;
                }
            }
        }

        public void LinesHorizontalDrawing(Excel.Worksheet worksheet, string startRange, string drawType, int forPlace)
        {
            Excel.Range excelRange;
            worksheet.Select(Type.Missing);
            excelRange = worksheet.get_Range(startRange, startRange);
            //var drawType = EventBySheetId(worksheet.Index)?.DrawType;
            if (drawType != null)
            {
                int playersCount = 0, scoreCount = 0, scoreRound = 0;
                int numberPers = int.Parse(drawType);
                var players = GamesTournament(worksheet, forPlace);
                if(players.Count == 0)
                    return;
                var score = GetScore(worksheet, forPlace);
                int x, y, yMnozh = 1;
                int startNumberPers = numberPers;
                int xNach = 1, yNach = 1;
                while (numberPers > 0)
                {
                    x = xNach;
                    y = yNach;
                    for (int i = 0; i < numberPers; i++)
                    {
                        excelRange.Cells[x, y].Value2 = players[playersCount];
                        if (playersCount == players.Count - 2)
                            excelRange.Cells[x, y + 1].Value2 = "За " + forPlace + " место";
                        if (scoreRound > 0)
                            excelRange.Cells[x + 1, y].Value2 = score[scoreCount];
                        excelRange.Cells[x, y].Borders[Excel.XlBordersIndex.xlEdgeBottom].ColorIndex = 1;
                        excelRange.Cells[x, y].Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle =
                            Excel.XlLineStyle.xlContinuous;
                        //excelRange.Cells[x, y].Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                        x += 2 * yMnozh;
                        playersCount++;
                        
                        if (scoreRound > 0)
                            scoreCount++;
                    }
                    scoreRound++;
                    xNach *= 2;
                    yNach += 1;
                    yMnozh *= 2;
                    numberPers /= 2;
                }
            }
        }

        public void LinesVerticalDrawing(Excel.Worksheet worksheet, string startRange, string drawType, int forPlace)
        {
            Excel.Range excelRange;
            worksheet.Select(Type.Missing);
            excelRange = worksheet.get_Range(startRange, startRange);
            //var drawType = EventBySheetId(worksheet.Index)?.DrawType;
            if (drawType != null)
            {
                int numberPers = int.Parse(drawType);

                int x, y, yMnozh = 1, count = 2;

                int startNumberPers = numberPers;
                int xNach = 1, yNach = 1;
                while (numberPers > 0)
                {
                    x = xNach;
                    y = yNach;
                    for (int i = 0; i < numberPers / 2; i++)
                    {
                        //excelRange.Cells[x, y].Value2 = round;
                        int currentX = x;
                        for (int j = x; j < count + currentX; j++)
                        {
                            excelRange.Cells[j, y].Borders[Excel.XlBordersIndex.xlEdgeRight].ColorIndex = 1;
                            excelRange.Cells[j, y].Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                            x = j;
                        }
                        x += (2 * yMnozh) + 1;
                    }
                    count *= 2;
                    xNach *= 2;
                    yNach += 1;
                    yMnozh *= 2;
                    numberPers /= 2;
                }
                var type = EventBySheetId(worksheet.Index)?.DrawType;
                if (type != null && startNumberPers == int.Parse(type) && forPlace == 1)
                    ThirdPlaceDrawingLines(xNach, yNach, excelRange, worksheet);
            }

        }

        private void ThirdPlaceDrawingLines(int x, int y, Excel.Range excelRange, Excel.Worksheet worksheet)
        {
            var players = GamesTournament(worksheet, 3);
            var score = GetScore(worksheet, 3);
            y -= 1;
            x += 1;
            excelRange.Cells[x, y].Borders[Excel.XlBordersIndex.xlEdgeRight].ColorIndex = 1;
            excelRange.Cells[x, y].Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
            excelRange.Cells[x, y].Borders[Excel.XlBordersIndex.xlEdgeTop].ColorIndex = 1;
            excelRange.Cells[x, y].Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
            excelRange.Cells[x - 1, y].Value2 = players[0];
            excelRange.Cells[x + 1, y + 1].Value2 = score[0];
            excelRange.Cells[x + 1, y].Value2 = players[1];
            excelRange.Cells[x, y + 1].Value2 = players[2];
            excelRange.Cells[x, y + 2].Value2 = "За 3 место";
            excelRange.Cells[x + 1, y].Borders[Excel.XlBordersIndex.xlEdgeRight].ColorIndex = 1;
            excelRange.Cells[x + 1, y].Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
            excelRange.Cells[x + 1, y].Borders[Excel.XlBordersIndex.xlEdgeBottom].ColorIndex = 1;
            excelRange.Cells[x + 1, y].Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;

            excelRange.Cells[x, y + 1].Borders[Excel.XlBordersIndex.xlEdgeBottom].ColorIndex = 1;
            excelRange.Cells[x, y + 1].Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
        }

        private List<string> GamesTournament(Excel.Worksheet worksheet, int forPlace)
        {
            List<string> list = new List<string>();
            Event eEvent = EventBySheetId(worksheet.Index);
            var for1PlacePlayers = Context.GamesTournaments.Where(g => g.EventId == eEvent.EventId && g.ForPlace == forPlace);
            foreach (var gamesTournament in for1PlacePlayers)
            {
                if (gamesTournament.TeamsTournament1 != null)
                    list.Add(gamesTournament.TeamsTournament1.TeamName);
                else
                    list.Add("");
                if (gamesTournament.TeamsTournament2 != null)
                    list.Add(gamesTournament.TeamsTournament2.TeamName);
                else
                    list.Add("");
            }
            return list;
        }

        private List<string> GetScore(Excel.Worksheet worksheet, int forPlace)
        {
            CalcsForDraws calcsForDraws = new CalcsForDraws();
            List<string> list = new List<string>();
            Event eEvent = EventBySheetId(worksheet.Index);
            var for1PlacePlayers = Context.GamesTournaments.Where(g => g.EventId == eEvent.EventId && g.ForPlace == forPlace);
            foreach (var gamesTournament in for1PlacePlayers)
            {
                if (gamesTournament.Score != null)
                    list.Add(calcsForDraws.ScoreConverter(gamesTournament.Score));
                else
                    list.Add("");
            }
            return list;
        }



        public Event EventBySheetId(int sheetId)
        {
            Context.Events.Load();
            int eEventId;
            Event eEvent = new Event();
            switch (sheetId)
            {
                case 7:
                case 2:
                    var singleManEvent = Context.Events.Local.FirstOrDefault(p => p.TournamentId == Tournament.TournamentId
                                                                                  && p.CategoryId == Category.CategoryId &&
                                                                                  p.Type.TypeName.Equals("Одиночка") &&
                                                                                  p.Sort.Equals("Юноши"));
                    if (singleManEvent != null)
                    {
                        eEventId = singleManEvent.EventId;
                        eEvent = Context.Events.Local.FirstOrDefault(p => p.EventId == eEventId);
                    }
                    break;
                case 8:
                case 3:
                    var singleGirlsEvent = Context.Events.Local.FirstOrDefault(p => p.TournamentId == Tournament.TournamentId
                                                                                  && p.CategoryId == Category.CategoryId &&
                                                                                  p.Type.TypeName.Equals("Одиночка") &&
                                                                                  p.Sort.Equals("Женщины"));
                    if (singleGirlsEvent != null)
                    {
                        eEventId = singleGirlsEvent.EventId;
                        eEvent = Context.Events.Local.FirstOrDefault(p => p.EventId == eEventId);
                    }
                    break;
                case 9:
                case 4:
                    var mansDoublesEvent = Context.Events.Local.FirstOrDefault(p => p.TournamentId == Tournament.TournamentId
                                                                                    && p.CategoryId == Category.CategoryId &&
                                                                                    p.Type.TypeName.Equals("Пара") &&
                                                                                    p.Sort.Equals("Юноши"));
                    if (mansDoublesEvent != null)
                    {
                        eEventId = mansDoublesEvent.EventId;
                        eEvent = Context.Events.Local.FirstOrDefault(p => p.EventId == eEventId);
                    }
                    break;
                case 10:
                case 5:
                    var girlsDoublesEvent = Context.Events.Local.FirstOrDefault(p => p.TournamentId == Tournament.TournamentId
                                                                                          && p.CategoryId == Category.CategoryId &&
                                                                                          p.Type.TypeName.Equals("Пара") &&
                                                                                          p.Sort.Equals("Женщины"));
                    if (girlsDoublesEvent != null)
                    {
                        eEventId = girlsDoublesEvent.EventId;
                        eEvent = Context.Events.Local.FirstOrDefault(p => p.EventId == eEventId);
                    }
                    break;
                case 11:
                case 6:
                    var mixtesEvent = Context.Events.Local.FirstOrDefault(p => p.TournamentId == Tournament.TournamentId
                                                                                           && p.CategoryId == Category.CategoryId &&
                                                                                           p.Type.TypeName.Equals("Микст"));
                    if (mixtesEvent != null)
                    {
                        eEventId = mixtesEvent.EventId;
                        eEvent = Context.Events.Local.FirstOrDefault(p => p.EventId == eEventId);
                    }
                    break;
            }
            return eEvent;
        }
    }
}