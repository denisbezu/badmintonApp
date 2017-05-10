namespace badmintonDataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FalseAdd : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TeamsTournaments", "EventId", "dbo.Events");
            AddForeignKey("dbo.TeamsTournaments", "EventId", "dbo.Events", "EventId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeamsTournaments", "EventId", "dbo.Events");
            AddForeignKey("dbo.TeamsTournaments", "EventId", "dbo.Events", "EventId");
        }
    }
}
