using System.Collections.Generic;
using System.Linq;
using badmintonDataBase.Models;

namespace BadmintonWPF.Helpers.DrawsHelpers
{
    public class CalcsForDraws
    {
        /// <summary>
        /// Считаем отступ для позиционировани игрока
        /// </summary>
        /// <param name="left">отступ слева</param>
        /// <param name="topNach">отступ сверху</param>
        /// <param name="yMnozh"> множитель для того, чтобы правильно расчитать позицию для каждого круга</param>
        /// <param name="round"> номер круга</param>
        /// <param name="whichPlayer">какой игрок выиграл</param>
        /// <param name="placeInDraw">позиция встречи в сетке</param>
        public void CalculateMarginForNeededLabel(ref int left, ref int topNach, ref int yMnozh, int round, int whichPlayer, int placeInDraw, Event eEvent)
        {
            for (int y = 0; y < round - CalculateFirstStageId(eEvent); y++) //проходим до нужного нам круга
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
        /// <summary>
        /// расчет круга по событию
        /// </summary>
        /// <param name="eEvent"></param>
        /// <returns></returns>
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
        /// <summary>
        /// расчет круга по количеству человек, подходит не только для 1 круга 
        /// </summary>
        /// <param name="numberOfPersons"></param>
        /// <returns></returns>
        public int CalculateFirstStageId(int numberOfPersons)
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
        /// <summary>
        /// определение круга при нажатии на определенный прямоугольник
        /// </summary>
        /// <param name="numberOfRectangle">номер прямоугольника</param>
        /// <param name="numberOfPlayers">количество игроков</param>
        /// <returns></returns>
        public int CalculateRoundForRectangle(int numberOfRectangle, int numberOfPlayers)
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
        /// <summary>
        /// расчет позции игрока в сетке для определенного круга, идея в том, что для каждого круга позция начинается с 1 т.е.
        /// для определения позиции встречи нужно знать 3 параметра: за какое место, круг и позицию в этом круге
        /// </summary>
        /// <param name="numberOfRectangle">номер прямоугольника</param>
        /// <param name="numberOfPlayers">количество игроков в сетке</param>
        /// <returns></returns>
        public int CalculatePlaceInDraw(int numberOfRectangle, int numberOfPlayers)
        {
            int startNumber = 0; // стартовое значение 
            while (startNumber < numberOfRectangle) // если значение наше меньше, чем номер прямоуг.
            {
                numberOfPlayers /= 2; // делим на 2, т.е. для того, чтобы проверить круг
                if ((startNumber + numberOfPlayers) > numberOfRectangle)// проверяем находимся ли мы на текущем круге
                {
                    break; // да
                }//нет
                startNumber += numberOfPlayers;//перекидываем наше начальное значение на след круг
            }
            return (numberOfRectangle - startNumber) / 2 + 1; // возвращаем номер встречи(делим на 2, т.к. встреча из 2 игроков, а +1, т.к. прямоуг. нумеруются с 0)
        }
        /// <summary>
        /// расчет позиции
        /// </summary>
        /// <param name="currentNumber"></param>
        /// <returns></returns>
        public int CalculatePlaceInDraw(int currentNumber)//для лейблов
        {
            int placeInDraw;
            if (currentNumber % 2 == 0)
                placeInDraw = currentNumber / 2;
            else
                placeInDraw = currentNumber / 2 + 1;
            return placeInDraw;
        }
        /// <summary>
        /// расчитываем позиции в предыдущем круге встречи, которя шла до текущей
        /// </summary>
        /// <param name="numberOfRectangle"></param>
        /// <param name="numberOfPlace"></param>
        /// <returns></returns>
        public int CalculatePrecedentGame(int numberOfRectangle, int numberOfPlace)
        {
            if (numberOfRectangle % 2 == 0)
                return numberOfPlace * 2 - 1;
            return numberOfPlace * 2;
        }
        /// <summary>
        /// преобразование счета для БД
        /// </summary>
        /// <param name="winnerPlayer"></param>
        /// <param name="winnerScore"></param>
        /// <returns></returns>
        public string WinnerHelperSplitter(int winnerPlayer, string winnerScore)
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
        /// <summary>
        /// преобразование счета
        /// </summary>
        /// <param name="winnerScore"></param>
        /// <returns></returns>
        public string ScoreConverter(string winnerScore)
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
        /// <summary>
        /// по номеру круга определяем название для утешительного турнира
        /// </summary>
        /// <param name="round"></param>
        /// <returns></returns>
        public string WhichHeaderChooseForLoosers(int round)
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
    }
}