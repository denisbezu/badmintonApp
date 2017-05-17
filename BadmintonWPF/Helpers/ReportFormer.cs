using System;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Windows;
using badmintonDataBase.DataAccess;
using badmintonDataBase.Models;
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
        private Excel.Sheets Sheets { get; set; }
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
        }

        public bool OpenWorkbook()
        {
            var result = SaveFilePath();
            if (result == null)
                return false;
            ExcelApplication = new Excel.Application();
            //Получаем набор объектов Workbook (массив ссылок на созданные книги)
            Workbooks = ExcelApplication.Workbooks;
            ExcelApplication.SheetsInNewWorkbook = 10;
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
            //(Worksheet.get_Range("A1", "J1")).EntireColumn.ColumnWidth = 20;
            (Worksheet.get_Range("H1", "H1")).EntireColumn.ColumnWidth = 25;
            (Worksheet.get_Range("A1", "A1")).EntireColumn.ColumnWidth = 5;
            (Worksheet.get_Range("B1", "B1")).EntireColumn.ColumnWidth = 20;
            (Worksheet.get_Range("C1", "C1")).EntireColumn.ColumnWidth = 7;
            (Worksheet.get_Range("D1", "D1")).EntireColumn.ColumnWidth = 10;
            (Worksheet.get_Range("E1", "E1")).EntireColumn.ColumnWidth = 12;
            (Worksheet.get_Range("F1", "F1")).EntireColumn.ColumnWidth = 20;
            (Worksheet.get_Range("G1", "G1")).EntireColumn.ColumnWidth = 10;
            (Worksheet.get_Range("A1", "Z1000")).Font.Size = 12;
            //название шрифта
            (Worksheet.get_Range("A1", "Z100")).Font.Name = "Times New Roman";
            excelRange = Worksheet.get_Range("A1", "H1");
            excelRange.Merge(Type.Missing);
            excelRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
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
            excelRange = Worksheet.get_Range(rangeFirst,  range2);
            excelRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
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
        public void JudgeWriter(string rangeFirst, string rangeSecond)
        {
            Excel.Range excelRange;
            excelRange = Worksheet.get_Range(rangeFirst, rangeSecond);
            int first = int.Parse(rangeFirst.Substring(1));
            int second = int.Parse(rangeSecond.Substring(1));
            Excel.Range excelRangeToMerge = Worksheet.get_Range("E" + first, "F" + second);
            excelRangeToMerge.Merge(Type.Missing);
            excelRangeToMerge.Value2 = "Головний суддя";
            excelRange.Cells[Type.Missing, 3] = Tournament.Judge.ToString();
            excelRangeToMerge = Worksheet.get_Range("E" + (first+ 2), "F" + (second + 2 ));
            excelRangeToMerge.Merge(Type.Missing);
            excelRangeToMerge.Value2 = "Головний секретар";
        }
    }
}