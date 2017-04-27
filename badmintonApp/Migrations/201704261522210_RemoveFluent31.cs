namespace badmintonDataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveFluent31 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Players", new[] { "CityId" });
            DropIndex("dbo.Coaches", new[] { "CityId" });
            DropIndex("dbo.Tournaments", new[] { "CityId" });
            AlterColumn("dbo.Players", "CityId", c => c.Int());
            AlterColumn("dbo.Coaches", "CityId", c => c.Int());
            AlterColumn("dbo.Tournaments", "CityId", c => c.Int());
            CreateIndex("dbo.Players", "CityId");
            CreateIndex("dbo.Coaches", "CityId");
            CreateIndex("dbo.Tournaments", "CityId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Tournaments", new[] { "CityId" });
            DropIndex("dbo.Coaches", new[] { "CityId" });
            DropIndex("dbo.Players", new[] { "CityId" });
            AlterColumn("dbo.Tournaments", "CityId", c => c.Int(nullable: false));
            AlterColumn("dbo.Coaches", "CityId", c => c.Int(nullable: false));
            AlterColumn("dbo.Players", "CityId", c => c.Int(nullable: false));
            CreateIndex("dbo.Tournaments", "CityId");
            CreateIndex("dbo.Coaches", "CityId");
            CreateIndex("dbo.Players", "CityId");
        }
    }
}
