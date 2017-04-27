namespace badmintonDataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedCascadeDelete : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Coaches", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Judges", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Tournaments", "CityId", "dbo.Cities");
            AddForeignKey("dbo.Coaches", "CityId", "dbo.Cities", "CityId", cascadeDelete: true);
            AddForeignKey("dbo.Judges", "CityId", "dbo.Cities", "CityId", cascadeDelete: true);
            AddForeignKey("dbo.Tournaments", "CityId", "dbo.Cities", "CityId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tournaments", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Judges", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Coaches", "CityId", "dbo.Cities");
            AddForeignKey("dbo.Tournaments", "CityId", "dbo.Cities", "CityId");
            AddForeignKey("dbo.Judges", "CityId", "dbo.Cities", "CityId");
            AddForeignKey("dbo.Coaches", "CityId", "dbo.Cities", "CityId");
        }
    }
}
