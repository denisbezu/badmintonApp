namespace badmintonDataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        EventId = c.Int(nullable: false, identity: true),
                        DrawType = c.String(nullable: false, maxLength: 30),
                        TypeId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        TournamentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EventId)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .ForeignKey("dbo.Tournaments", t => t.TournamentId)
                .ForeignKey("dbo.Types", t => t.TypeId)
                .Index(t => t.TypeId)
                .Index(t => t.CategoryId)
                .Index(t => t.TournamentId);
            
            CreateTable(
                "dbo.TeamsTournaments",
                c => new
                    {
                        TeamsTournamentId = c.Int(nullable: false, identity: true),
                        EventId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TeamsTournamentId)
                .ForeignKey("dbo.Events", t => t.EventId)
                .Index(t => t.EventId);
            
            CreateTable(
                "dbo.PlayersTeams",
                c => new
                    {
                        PlayersTeamId = c.Int(nullable: false, identity: true),
                        TeamsTournamentId = c.Int(nullable: false),
                        PlayerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PlayersTeamId)
                .ForeignKey("dbo.Players", t => t.PlayerId)
                .ForeignKey("dbo.TeamsTournaments", t => t.TeamsTournamentId)
                .Index(t => t.TeamsTournamentId)
                .Index(t => t.PlayerId);
            
            CreateTable(
                "dbo.GamesTournaments",
                c => new
                    {
                        GamesTournamentId = c.Int(nullable: false, identity: true),
                        PlayersTeam1Id = c.Int(nullable: false),
                        PlayersTeam2Id = c.Int(nullable: false),
                        Score = c.String(nullable: false, maxLength: 50),
                        StageId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GamesTournamentId)
                .ForeignKey("dbo.PlayersTeams", t => t.PlayersTeam1Id)
                .ForeignKey("dbo.PlayersTeams", t => t.PlayersTeam2Id)
                .ForeignKey("dbo.Stages", t => t.StageId, cascadeDelete: true)
                .Index(t => t.PlayersTeam1Id)
                .Index(t => t.PlayersTeam2Id)
                .Index(t => t.StageId);
            
            CreateTable(
                "dbo.Stages",
                c => new
                    {
                        StageId = c.Int(nullable: false, identity: true),
                        StageName = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.StageId);
            
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        PlayerId = c.Int(nullable: false, identity: true),
                        PlayerName = c.String(nullable: false, maxLength: 25),
                        PlayerSurName = c.String(nullable: false, maxLength: 25),
                        YearOfBirth = c.Int(nullable: false),
                        Sex = c.String(nullable: false, maxLength: 25),
                        GradeId = c.Int(nullable: false),
                        CityId = c.Int(nullable: false),
                        ClubId = c.Int(nullable: false),
                        UnionId = c.Int(nullable: false),
                        CoachId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PlayerId)
                .ForeignKey("dbo.Cities", t => t.CityId)
                .ForeignKey("dbo.Clubs", t => t.ClubId)
                .ForeignKey("dbo.Coaches", t => t.CoachId)
                .ForeignKey("dbo.Grades", t => t.GradeId)
                .ForeignKey("dbo.Unions", t => t.UnionId)
                .Index(t => t.GradeId)
                .Index(t => t.CityId)
                .Index(t => t.ClubId)
                .Index(t => t.UnionId)
                .Index(t => t.CoachId);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        CityId = c.Int(nullable: false, identity: true),
                        CityName = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.CityId);
            
            CreateTable(
                "dbo.Clubs",
                c => new
                    {
                        ClubId = c.Int(nullable: false, identity: true),
                        ClubName = c.String(nullable: false, maxLength: 30),
                        CityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClubId)
                .ForeignKey("dbo.Cities", t => t.CityId, cascadeDelete: true)
                .Index(t => t.CityId);
            
            CreateTable(
                "dbo.Coaches",
                c => new
                    {
                        CoachId = c.Int(nullable: false, identity: true),
                        CoachName = c.String(nullable: false, maxLength: 120),
                        YearOfBirth = c.Int(),
                        CityId = c.Int(nullable: false),
                        ClubId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CoachId)
                .ForeignKey("dbo.Cities", t => t.CityId)
                .ForeignKey("dbo.Clubs", t => t.ClubId)
                .Index(t => t.CityId)
                .Index(t => t.ClubId);
            
            CreateTable(
                "dbo.Judges",
                c => new
                    {
                        JudgeId = c.Int(nullable: false, identity: true),
                        JudgeName = c.String(nullable: false, maxLength: 50),
                        JudgeLastName = c.String(nullable: false, maxLength: 50),
                        CityId = c.Int(nullable: false),
                        YearOfBirth = c.Int(),
                    })
                .PrimaryKey(t => t.JudgeId)
                .ForeignKey("dbo.Cities", t => t.CityId)
                .Index(t => t.CityId);
            
            CreateTable(
                "dbo.Tournaments",
                c => new
                    {
                        TournamentId = c.Int(nullable: false, identity: true),
                        TournamentName = c.String(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        FinishDate = c.DateTime(nullable: false),
                        JudgeId = c.Int(nullable: false),
                        CityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TournamentId)
                .ForeignKey("dbo.Cities", t => t.CityId)
                .ForeignKey("dbo.Judges", t => t.JudgeId)
                .Index(t => t.JudgeId)
                .Index(t => t.CityId);
            
            CreateTable(
                "dbo.Grades",
                c => new
                    {
                        GradeId = c.Int(nullable: false, identity: true),
                        GradeName = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.GradeId);
            
            CreateTable(
                "dbo.Histories",
                c => new
                    {
                        HistoryId = c.Int(nullable: false, identity: true),
                        PlayerId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        ParameterId = c.Int(nullable: false),
                        NewValue = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.HistoryId)
                .ForeignKey("dbo.Parameters", t => t.ParameterId)
                .ForeignKey("dbo.Players", t => t.PlayerId)
                .Index(t => t.PlayerId)
                .Index(t => t.ParameterId);
            
            CreateTable(
                "dbo.Parameters",
                c => new
                    {
                        ParameterId = c.Int(nullable: false, identity: true),
                        ParameterName = c.String(nullable: false, maxLength: 25),
                    })
                .PrimaryKey(t => t.ParameterId);
            
            CreateTable(
                "dbo.Unions",
                c => new
                    {
                        UnionId = c.Int(nullable: false, identity: true),
                        UnionName = c.String(nullable: false, maxLength: 25),
                    })
                .PrimaryKey(t => t.UnionId);
            
            CreateTable(
                "dbo.Types",
                c => new
                    {
                        TypeId = c.Int(nullable: false, identity: true),
                        TypeName = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.TypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "TypeId", "dbo.Types");
            DropForeignKey("dbo.Events", "TournamentId", "dbo.Tournaments");
            DropForeignKey("dbo.PlayersTeams", "TeamsTournamentId", "dbo.TeamsTournaments");
            DropForeignKey("dbo.PlayersTeams", "PlayerId", "dbo.Players");
            DropForeignKey("dbo.Players", "UnionId", "dbo.Unions");
            DropForeignKey("dbo.Histories", "PlayerId", "dbo.Players");
            DropForeignKey("dbo.Histories", "ParameterId", "dbo.Parameters");
            DropForeignKey("dbo.Players", "GradeId", "dbo.Grades");
            DropForeignKey("dbo.Players", "CoachId", "dbo.Coaches");
            DropForeignKey("dbo.Players", "ClubId", "dbo.Clubs");
            DropForeignKey("dbo.Players", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Tournaments", "JudgeId", "dbo.Judges");
            DropForeignKey("dbo.Tournaments", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Judges", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Coaches", "ClubId", "dbo.Clubs");
            DropForeignKey("dbo.Coaches", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Clubs", "CityId", "dbo.Cities");
            DropForeignKey("dbo.GamesTournaments", "StageId", "dbo.Stages");
            DropForeignKey("dbo.GamesTournaments", "PlayersTeam2Id", "dbo.PlayersTeams");
            DropForeignKey("dbo.GamesTournaments", "PlayersTeam1Id", "dbo.PlayersTeams");
            DropForeignKey("dbo.TeamsTournaments", "EventId", "dbo.Events");
            DropForeignKey("dbo.Events", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Histories", new[] { "ParameterId" });
            DropIndex("dbo.Histories", new[] { "PlayerId" });
            DropIndex("dbo.Tournaments", new[] { "CityId" });
            DropIndex("dbo.Tournaments", new[] { "JudgeId" });
            DropIndex("dbo.Judges", new[] { "CityId" });
            DropIndex("dbo.Coaches", new[] { "ClubId" });
            DropIndex("dbo.Coaches", new[] { "CityId" });
            DropIndex("dbo.Clubs", new[] { "CityId" });
            DropIndex("dbo.Players", new[] { "CoachId" });
            DropIndex("dbo.Players", new[] { "UnionId" });
            DropIndex("dbo.Players", new[] { "ClubId" });
            DropIndex("dbo.Players", new[] { "CityId" });
            DropIndex("dbo.Players", new[] { "GradeId" });
            DropIndex("dbo.GamesTournaments", new[] { "StageId" });
            DropIndex("dbo.GamesTournaments", new[] { "PlayersTeam2Id" });
            DropIndex("dbo.GamesTournaments", new[] { "PlayersTeam1Id" });
            DropIndex("dbo.PlayersTeams", new[] { "PlayerId" });
            DropIndex("dbo.PlayersTeams", new[] { "TeamsTournamentId" });
            DropIndex("dbo.TeamsTournaments", new[] { "EventId" });
            DropIndex("dbo.Events", new[] { "TournamentId" });
            DropIndex("dbo.Events", new[] { "CategoryId" });
            DropIndex("dbo.Events", new[] { "TypeId" });
            DropTable("dbo.Types");
            DropTable("dbo.Unions");
            DropTable("dbo.Parameters");
            DropTable("dbo.Histories");
            DropTable("dbo.Grades");
            DropTable("dbo.Tournaments");
            DropTable("dbo.Judges");
            DropTable("dbo.Coaches");
            DropTable("dbo.Clubs");
            DropTable("dbo.Cities");
            DropTable("dbo.Players");
            DropTable("dbo.Stages");
            DropTable("dbo.GamesTournaments");
            DropTable("dbo.PlayersTeams");
            DropTable("dbo.TeamsTournaments");
            DropTable("dbo.Events");
            DropTable("dbo.Categories");
        }
    }
}
