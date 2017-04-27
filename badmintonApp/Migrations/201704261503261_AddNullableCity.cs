namespace badmintonDataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNullableCity : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Clubs", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Coaches", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Judges", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Tournaments", "CityId", "dbo.Cities");
            DropIndex("dbo.Clubs", new[] { "CityId" });
            AlterColumn("dbo.Clubs", "CityId", c => c.Int());
            CreateIndex("dbo.Clubs", "CityId");
            AddForeignKey("dbo.Clubs", "CityId", "dbo.Cities", "CityId");
            AddForeignKey("dbo.Coaches", "CityId", "dbo.Cities", "CityId");
            AddForeignKey("dbo.Judges", "CityId", "dbo.Cities", "CityId");
            AddForeignKey("dbo.Tournaments", "CityId", "dbo.Cities", "CityId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tournaments", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Judges", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Coaches", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Clubs", "CityId", "dbo.Cities");
            DropIndex("dbo.Clubs", new[] { "CityId" });
            AlterColumn("dbo.Clubs", "CityId", c => c.Int(nullable: false));
            CreateIndex("dbo.Clubs", "CityId");
            AddForeignKey("dbo.Tournaments", "CityId", "dbo.Cities", "CityId", cascadeDelete: true);
            AddForeignKey("dbo.Judges", "CityId", "dbo.Cities", "CityId", cascadeDelete: true);
            AddForeignKey("dbo.Coaches", "CityId", "dbo.Cities", "CityId", cascadeDelete: true);
            AddForeignKey("dbo.Clubs", "CityId", "dbo.Cities", "CityId", cascadeDelete: true);
        }
    }
}
