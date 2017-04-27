namespace badmintonDataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveFluent3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Judges", "CityId", "dbo.Cities");
            DropIndex("dbo.Judges", new[] { "CityId" });
            AlterColumn("dbo.Judges", "CityId", c => c.Int());
            CreateIndex("dbo.Judges", "CityId");
            AddForeignKey("dbo.Judges", "CityId", "dbo.Cities", "CityId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Judges", "CityId", "dbo.Cities");
            DropIndex("dbo.Judges", new[] { "CityId" });
            AlterColumn("dbo.Judges", "CityId", c => c.Int(nullable: false));
            CreateIndex("dbo.Judges", "CityId");
            AddForeignKey("dbo.Judges", "CityId", "dbo.Cities", "CityId", cascadeDelete: true);
        }
    }
}
