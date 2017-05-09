namespace badmintonDataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class eveveve : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GamesTournaments", "EventId", c => c.Int());
            CreateIndex("dbo.GamesTournaments", "EventId");
            AddForeignKey("dbo.GamesTournaments", "EventId", "dbo.Events", "EventId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GamesTournaments", "EventId", "dbo.Events");
            DropIndex("dbo.GamesTournaments", new[] { "EventId" });
            DropColumn("dbo.GamesTournaments", "EventId");
        }
    }
}
