namespace badmintonDataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ADdGames : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GamesTournaments", "EventId", "dbo.Events");
            DropIndex("dbo.GamesTournaments", new[] { "EventId" });
            AlterColumn("dbo.GamesTournaments", "EventId", c => c.Int(nullable: false));
            CreateIndex("dbo.GamesTournaments", "EventId");
            AddForeignKey("dbo.GamesTournaments", "EventId", "dbo.Events", "EventId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GamesTournaments", "EventId", "dbo.Events");
            DropIndex("dbo.GamesTournaments", new[] { "EventId" });
            AlterColumn("dbo.GamesTournaments", "EventId", c => c.Int());
            CreateIndex("dbo.GamesTournaments", "EventId");
            AddForeignKey("dbo.GamesTournaments", "EventId", "dbo.Events", "EventId");
        }
    }
}
