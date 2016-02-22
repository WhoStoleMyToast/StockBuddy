namespace StockBuddy.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IntialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EntryStrategy",
                c => new
                    {
                        EntryStrategyID = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.EntryStrategyID);
            
            CreateTable(
                "dbo.ExitStrategy",
                c => new
                    {
                        ExitStrategyID = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.ExitStrategyID);
        }
        
        public override void Down()
        {
            DropTable("dbo.ExitStrategy");
            DropTable("dbo.EntryStrategy");
        }
    }
}
