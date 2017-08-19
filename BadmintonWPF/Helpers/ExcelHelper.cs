using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using badmintonDataBase.Models;

namespace BadmintonWPF.Helpers
{
    public class ExcelHelper
    {
        public List<string> Circles { get; set; }

        public ExcelHelper()
        {
            Circles = new List<string>();
        }
        public List<string> RoundsDrawing(int numberPers)
        {
            List<string> resultCircles = new List<string>();
            Circles.Add("Первый круг");
            Circles.Add("Второй круг");
            Circles.Add("Третий круг");
            Circles.Add("Четвертьфинал");
            Circles.Add("Полуфинал");
            Circles.Add("Финал");
            Circles.Add("Победитель");
            if (numberPers == 64)
            {
                foreach (var i in Circles)
                {
                    resultCircles.Add(i);
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
                    resultCircles.Add(i);
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
                    resultCircles.Add(i);
                    if (i == "Победитель")
                        break;
                }
            }
            else if (numberPers == 8)
            {
                for (int i = 3; i < Circles.Count; i++)
                {
                    resultCircles.Add(Circles[i]);
                    if (Circles[i] == "Победитель")
                        break;
                }
            }
            else if (numberPers == 4)
            {
                for (int i = 4; i < Circles.Count; i++)
                {
                    resultCircles.Add(Circles[i]);
                    if (Circles[i] == "Победитель")
                        break;
                }
            }

            return resultCircles;
        }

        public static List<int> GetLooserDraw(Event eEvent)
        {
            List<int> list = new List<int>();
            if (int.Parse(eEvent.DrawType) == 64)
            {
                list.Add(33);
                list.Add(17);
                list.Add(9);
                list.Add(5);
            }
            if (int.Parse(eEvent.DrawType) == 32)
            {
                list.Add(17);
                list.Add(9);
                list.Add(5);
            }
            if (int.Parse(eEvent.DrawType) == 16)
            {
                list.Add(9);
                list.Add(5);
            }
            if (int.Parse(eEvent.DrawType) == 8)
            {
                list.Add(5);
            }
            return list;
        }

    }

}
