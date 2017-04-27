namespace badmintonDataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPosev : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TeamsTournaments", "SeedingNumber", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TeamsTournaments", "SeedingNumber");
        }
    }
}
