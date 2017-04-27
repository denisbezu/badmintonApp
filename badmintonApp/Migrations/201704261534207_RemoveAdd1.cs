namespace badmintonDataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveAdd1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GamesTournaments", "StageId", "dbo.Stages");
            DropIndex("dbo.Events", new[] { "TypeId" });
            DropIndex("dbo.Events", new[] { "CategoryId" });
            DropIndex("dbo.GamesTournaments", new[] { "StageId" });
            DropIndex("dbo.Players", new[] { "GradeId" });
            DropIndex("dbo.Players", new[] { "ClubId" });
            DropIndex("dbo.Players", new[] { "UnionId" });
            DropIndex("dbo.Players", new[] { "CoachId" });
            DropIndex("dbo.Coaches", new[] { "ClubId" });
            DropIndex("dbo.Tournaments", new[] { "JudgeId" });
            AlterColumn("dbo.Events", "TypeId", c => c.Int());
            AlterColumn("dbo.Events", "CategoryId", c => c.Int());
            AlterColumn("dbo.GamesTournaments", "StageId", c => c.Int());
            AlterColumn("dbo.Players", "GradeId", c => c.Int());
            AlterColumn("dbo.Players", "ClubId", c => c.Int());
            AlterColumn("dbo.Players", "UnionId", c => c.Int());
            AlterColumn("dbo.Players", "CoachId", c => c.Int());
            AlterColumn("dbo.Coaches", "ClubId", c => c.Int());
            AlterColumn("dbo.Tournaments", "JudgeId", c => c.Int());
            CreateIndex("dbo.Events", "TypeId");
            CreateIndex("dbo.Events", "CategoryId");
            CreateIndex("dbo.GamesTournaments", "StageId");
            CreateIndex("dbo.Players", "GradeId");
            CreateIndex("dbo.Players", "ClubId");
            CreateIndex("dbo.Players", "UnionId");
            CreateIndex("dbo.Players", "CoachId");
            CreateIndex("dbo.Coaches", "ClubId");
            CreateIndex("dbo.Tournaments", "JudgeId");
            AddForeignKey("dbo.GamesTournaments", "StageId", "dbo.Stages", "StageId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GamesTournaments", "StageId", "dbo.Stages");
            DropIndex("dbo.Tournaments", new[] { "JudgeId" });
            DropIndex("dbo.Coaches", new[] { "ClubId" });
            DropIndex("dbo.Players", new[] { "CoachId" });
            DropIndex("dbo.Players", new[] { "UnionId" });
            DropIndex("dbo.Players", new[] { "ClubId" });
            DropIndex("dbo.Players", new[] { "GradeId" });
            DropIndex("dbo.GamesTournaments", new[] { "StageId" });
            DropIndex("dbo.Events", new[] { "CategoryId" });
            DropIndex("dbo.Events", new[] { "TypeId" });
            AlterColumn("dbo.Tournaments", "JudgeId", c => c.Int(nullable: false));
            AlterColumn("dbo.Coaches", "ClubId", c => c.Int(nullable: false));
            AlterColumn("dbo.Players", "CoachId", c => c.Int(nullable: false));
            AlterColumn("dbo.Players", "UnionId", c => c.Int(nullable: false));
            AlterColumn("dbo.Players", "ClubId", c => c.Int(nullable: false));
            AlterColumn("dbo.Players", "GradeId", c => c.Int(nullable: false));
            AlterColumn("dbo.GamesTournaments", "StageId", c => c.Int(nullable: false));
            AlterColumn("dbo.Events", "CategoryId", c => c.Int(nullable: false));
            AlterColumn("dbo.Events", "TypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Tournaments", "JudgeId");
            CreateIndex("dbo.Coaches", "ClubId");
            CreateIndex("dbo.Players", "CoachId");
            CreateIndex("dbo.Players", "UnionId");
            CreateIndex("dbo.Players", "ClubId");
            CreateIndex("dbo.Players", "GradeId");
            CreateIndex("dbo.GamesTournaments", "StageId");
            CreateIndex("dbo.Events", "CategoryId");
            CreateIndex("dbo.Events", "TypeId");
            AddForeignKey("dbo.GamesTournaments", "StageId", "dbo.Stages", "StageId", cascadeDelete: true);
        }
    }
}
