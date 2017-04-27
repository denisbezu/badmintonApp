namespace badmintonDataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteYearJudge : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Judges", "JudgeSurName", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Judges", "YearOfBirth");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Judges", "YearOfBirth", c => c.Int());
            DropColumn("dbo.Judges", "JudgeSurName");
        }
    }
}
