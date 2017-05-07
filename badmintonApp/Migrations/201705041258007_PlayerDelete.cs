namespace badmintonDataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlayerDelete : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PlayersTeams", "PlayerId", "dbo.Players");
            AddForeignKey("dbo.PlayersTeams", "PlayerId", "dbo.Players", "PlayerId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PlayersTeams", "PlayerId", "dbo.Players");
            AddForeignKey("dbo.PlayersTeams", "PlayerId", "dbo.Players", "PlayerId");
        }
    }
}
