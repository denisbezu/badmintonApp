namespace badmintonDataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSort : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "Sort", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "Sort");
        }
    }
}
