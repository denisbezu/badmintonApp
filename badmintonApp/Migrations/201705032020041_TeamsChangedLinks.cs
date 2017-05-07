namespace badmintonDataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TeamsChangedLinks : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GamesTournaments", "PlayersTeam1Id", "dbo.PlayersTeams");
            DropForeignKey("dbo.GamesTournaments", "PlayersTeam2Id", "dbo.PlayersTeams");
            DropIndex("dbo.GamesTournaments", new[] { "PlayersTeam1Id" });
            DropIndex("dbo.GamesTournaments", new[] { "PlayersTeam2Id" });
            AddColumn("dbo.TeamsTournaments", "TeamName", c => c.String());
            AddColumn("dbo.GamesTournaments", "TeamsTournament1Id", c => c.Int(nullable: false));
            AddColumn("dbo.GamesTournaments", "TeamsTournament2Id", c => c.Int(nullable: false));
            CreateIndex("dbo.GamesTournaments", "TeamsTournament1Id");
            CreateIndex("dbo.GamesTournaments", "TeamsTournament2Id");
            AddForeignKey("dbo.GamesTournaments", "TeamsTournament1Id", "dbo.TeamsTournaments", "TeamsTournamentId");
            AddForeignKey("dbo.GamesTournaments", "TeamsTournament2Id", "dbo.TeamsTournaments", "TeamsTournamentId");
            DropColumn("dbo.GamesTournaments", "PlayersTeam1Id");
            DropColumn("dbo.GamesTournaments", "PlayersTeam2Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GamesTournaments", "PlayersTeam2Id", c => c.Int(nullable: false));
            AddColumn("dbo.GamesTournaments", "PlayersTeam1Id", c => c.Int(nullable: false));
            DropForeignKey("dbo.GamesTournaments", "TeamsTournament2Id", "dbo.TeamsTournaments");
            DropForeignKey("dbo.GamesTournaments", "TeamsTournament1Id", "dbo.TeamsTournaments");
            DropIndex("dbo.GamesTournaments", new[] { "TeamsTournament2Id" });
            DropIndex("dbo.GamesTournaments", new[] { "TeamsTournament1Id" });
            DropColumn("dbo.GamesTournaments", "TeamsTournament2Id");
            DropColumn("dbo.GamesTournaments", "TeamsTournament1Id");
            DropColumn("dbo.TeamsTournaments", "TeamName");
            CreateIndex("dbo.GamesTournaments", "PlayersTeam2Id");
            CreateIndex("dbo.GamesTournaments", "PlayersTeam1Id");
            AddForeignKey("dbo.GamesTournaments", "PlayersTeam2Id", "dbo.PlayersTeams", "PlayersTeamId");
            AddForeignKey("dbo.GamesTournaments", "PlayersTeam1Id", "dbo.PlayersTeams", "PlayersTeamId");
        }
    }
}
