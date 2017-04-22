using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using badmintonDataBase.DataAccess;
using badmintonDataBase.Models;

namespace badmintonDataBase
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            { // Стратегия работы с базой данных 
                Database.SetInitializer(new DropCreateDatabaseAlways<BadmintonContext>());
                var cities = new List<City>()
                {
                    new City(){CityName = "Kharkov"}
                };
                using (var context = new BadmintonContext())
                {
                    cities.ForEach(c => context.Cities.Add(c));
                    context.SaveChanges();

                }
                Console.WriteLine("База данных на SQL Server создана");

            }
            catch (System.Exception ex)
            {
                Console.WriteLine("База данных не создана. \n Ошибка:\n " + ex.ToString());
            }
            Console.WriteLine("Нажмите любую клавишу, чтобы выйти...");
            Console.ReadKey();
        }
    }
}
