namespace badmintonDataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForPlaceAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GamesTournaments", "ForPlace", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GamesTournaments", "ForPlace");
        }
    }
}
