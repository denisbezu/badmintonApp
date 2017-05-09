namespace badmintonDataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fdsfafs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GamesTournaments",
                c => new
                    {
                        GamesTournamentId = c.Int(nullable: false, identity: true),
                        TeamsTournament1Id = c.Int(),
                        TeamsTournament2Id = c.Int(),
                        Score = c.String(maxLength: 50),
                        StageId = c.Int(),
                        ForPlace = c.Int(nullable: false),
                        PlaceInDraw = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GamesTournamentId)
                .ForeignKey("dbo.Stages", t => t.StageId)
                .ForeignKey("dbo.TeamsTournaments", t => t.TeamsTournament1Id)
                .ForeignKey("dbo.TeamsTournaments", t => t.TeamsTournament2Id)
                .Index(t => t.TeamsTournament1Id)
                .Index(t => t.TeamsTournament2Id)
                .Index(t => t.StageId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GamesTournaments", "TeamsTournament2Id", "dbo.TeamsTournaments");
            DropForeignKey("dbo.GamesTournaments", "TeamsTournament1Id", "dbo.TeamsTournaments");
            DropForeignKey("dbo.GamesTournaments", "StageId", "dbo.Stages");
            DropIndex("dbo.GamesTournaments", new[] { "StageId" });
            DropIndex("dbo.GamesTournaments", new[] { "TeamsTournament2Id" });
            DropIndex("dbo.GamesTournaments", new[] { "TeamsTournament1Id" });
            DropTable("dbo.GamesTournaments");
        }
    }
}
