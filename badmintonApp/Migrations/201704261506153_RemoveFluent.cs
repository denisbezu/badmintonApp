namespace badmintonDataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveFluent : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Judges", new[] { "CityId" });
            AlterColumn("dbo.Judges", "CityId", c => c.Int());
            CreateIndex("dbo.Judges", "CityId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Judges", new[] { "CityId" });
            AlterColumn("dbo.Judges", "CityId", c => c.Int(nullable: false));
            CreateIndex("dbo.Judges", "CityId");
        }
    }
}
