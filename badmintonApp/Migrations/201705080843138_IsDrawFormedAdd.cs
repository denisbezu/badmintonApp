namespace badmintonDataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsDrawFormedAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "IsDrawFormed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "IsDrawFormed");
        }
    }
}
