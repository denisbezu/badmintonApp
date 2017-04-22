using System.Data.Entity;
using badmintonDataBase.Models;

namespace badmintonDataBase.DataAccess
{
    public class BadmintonContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<Judge> Judges { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Union> Unions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Parameter> Parameters { get; set; }
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<TeamsTournament> TeamsTournaments { get; set; }
        public DbSet<GamesTournament> GameSingles { get; set; }
        public DbSet<PlayersTeam> PlayersTeams { get; set; }
    }
}