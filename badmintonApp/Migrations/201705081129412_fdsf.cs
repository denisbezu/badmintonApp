namespace badmintonDataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fdsf : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GamesTournaments", "StageId", "dbo.Stages");
            DropForeignKey("dbo.GamesTournaments", "TeamsTournament1Id", "dbo.TeamsTournaments");
            DropForeignKey("dbo.GamesTournaments", "TeamsTournament2Id", "dbo.TeamsTournaments");
            DropIndex("dbo.GamesTournaments", new[] { "TeamsTournament1Id" });
            DropIndex("dbo.GamesTournaments", new[] { "TeamsTournament2Id" });
            DropIndex("dbo.GamesTournaments", new[] { "StageId" });
            
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.GamesTournaments",
                c => new
                    {
                        GamesTournamentId = c.Int(nullable: false, identity: true),
                        TeamsTournament1Id = c.Int(nullable: false),
                        TeamsTournament2Id = c.Int(nullable: false),
                        Score = c.String(nullable: false, maxLength: 50),
                        StageId = c.Int(),
                        ForPlace = c.Int(nullable: false),
                        PlaceInDraw = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GamesTournamentId);
            
            CreateIndex("dbo.GamesTournaments", "StageId");
            CreateIndex("dbo.GamesTournaments", "TeamsTournament2Id");
            CreateIndex("dbo.GamesTournaments", "TeamsTournament1Id");
            AddForeignKey("dbo.GamesTournaments", "TeamsTournament2Id", "dbo.TeamsTournaments", "TeamsTournamentId");
            AddForeignKey("dbo.GamesTournaments", "TeamsTournament1Id", "dbo.TeamsTournaments", "TeamsTournamentId");
            AddForeignKey("dbo.GamesTournaments", "StageId", "dbo.Stages", "StageId");
        }
    }
}
