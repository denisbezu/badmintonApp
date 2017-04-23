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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            #region GamesTournament
            
            modelBuilder.Entity<GamesTournament>()
                .HasRequired(m => m.PlayersTeam1)
                .WithMany(t => t.FirstPlayer)
                .HasForeignKey(m => m.PlayersTeam1Id)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<GamesTournament>()
                .HasRequired(m => m.PlayersTeam2)
                .WithMany(t => t.SecondPlayer)
                .HasForeignKey(m => m.PlayersTeam2Id)
                .WillCascadeOnDelete(false);
            #endregion

            #region Coach 
            modelBuilder.Entity<Coach>()
                .HasRequired(m => m.City)
                .WithMany(t => t.Coaches)
                .HasForeignKey(m => m.CityId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Coach>()
                .HasRequired(m => m.Club)
                .WithMany(t => t.Coaches)
                .HasForeignKey(m => m.ClubId)
                .WillCascadeOnDelete(false);
            #endregion

            #region Event

            modelBuilder.Entity<Event>()
                .HasRequired(m => m.Type)
                .WithMany(t => t.Events)
                .HasForeignKey(m => m.TypeId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Event>()
                .HasRequired(m => m.Category)
                .WithMany(t => t.Events)
                .HasForeignKey(m => m.CategoryId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Event>()
                .HasRequired(m => m.Tournament)
                .WithMany(t => t.Events)
                .HasForeignKey(m => m.TournamentId)
                .WillCascadeOnDelete(false);

            #endregion

            #region History

            modelBuilder.Entity<History>()
                .HasRequired(m => m.Player)
                .WithMany(t => t.Histories)
                .HasForeignKey(m => m.PlayerId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<History>()
                .HasRequired(m => m.Parameter)
                .WithMany(t => t.Histories)
                .HasForeignKey(m => m.ParameterId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Judge
            modelBuilder.Entity<Judge>()
                .HasRequired(m => m.City)
                .WithMany(t => t.Judges)
                .HasForeignKey(m => m.CityId)
                .WillCascadeOnDelete(false);
            #endregion

            #region Player

            modelBuilder.Entity<Player>()
                .HasRequired(m => m.City)
                .WithMany(t => t.Players)
                .HasForeignKey(m => m.CityId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Player>()
                .HasRequired(m => m.Grade)
                .WithMany(t => t.Players)
                .HasForeignKey(m => m.GradeId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Player>()
                .HasRequired(m => m.Club)
                .WithMany(t => t.Players)
                .HasForeignKey(m => m.ClubId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Player>()
                .HasRequired(m => m.Union)
                .WithMany(t => t.Players)
                .HasForeignKey(m => m.UnionId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Player>()
                .HasRequired(m => m.Coach)
                .WithMany(t => t.Players)
                .HasForeignKey(m => m.CoachId)
                .WillCascadeOnDelete(false);
            #endregion

            #region PlayersTeam

            modelBuilder.Entity<PlayersTeam>()
                .HasRequired(m => m.TeamsTournament)
                .WithMany(t => t.PlayersTeams)
                .HasForeignKey(m => m.TeamsTournamentId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<PlayersTeam>()
                .HasRequired(m => m.Player)
                .WithMany(t => t.PlayersTeams)
                .HasForeignKey(m => m.PlayerId)
                .WillCascadeOnDelete(false);
            #endregion

            #region TeamsTournament

            modelBuilder.Entity<TeamsTournament>()
                .HasRequired(m => m.Event)
                .WithMany(t => t.TeamsTournaments)
                .HasForeignKey(m => m.EventId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Tournament

            modelBuilder.Entity<Tournament>()
                .HasRequired(m => m.City)
                .WithMany(t => t.Tournaments)
                .HasForeignKey(m => m.CityId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Tournament>()
                .HasRequired(m => m.Judge)
                .WithMany(t => t.Tournaments)
                .HasForeignKey(m => m.JudgeId)
                .WillCascadeOnDelete(false);

            #endregion
        }
    }
}