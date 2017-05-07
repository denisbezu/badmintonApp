namespace badmintonDataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TeamsTournamentCascadeOnDelete : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PlayersTeams", "TeamsTournamentId", "dbo.TeamsTournaments");
            AddForeignKey("dbo.PlayersTeams", "TeamsTournamentId", "dbo.TeamsTournaments", "TeamsTournamentId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PlayersTeams", "TeamsTournamentId", "dbo.TeamsTournaments");
            AddForeignKey("dbo.PlayersTeams", "TeamsTournamentId", "dbo.TeamsTournaments", "TeamsTournamentId");
        }
    }
}
