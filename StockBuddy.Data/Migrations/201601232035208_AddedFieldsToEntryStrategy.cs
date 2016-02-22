namespace StockBuddy.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFieldsToEntryStrategy : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EntryStrategy",
                c => new
                    {
                        EntryStrategyID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        MinADX = c.Decimal(precision: 18, scale: 2),
                        MaxADX = c.Decimal(precision: 18, scale: 2),
                        MinVolume = c.Decimal(precision: 18, scale: 2),
                        MinDivergence = c.Decimal(precision: 18, scale: 2),
                        MaxDivergence = c.Decimal(precision: 18, scale: 2),
                        MinPercentChange = c.Decimal(precision: 18, scale: 2),
                        MaxPercentChange = c.Decimal(precision: 18, scale: 2),
                        MaxMacd = c.Decimal(precision: 18, scale: 2),
                        MinMacd = c.Decimal(precision: 18, scale: 2),
                        useMacdSma = c.Boolean(nullable: false),
                        recentMacdCross = c.Boolean(nullable: false),
                        isActive = c.Boolean(nullable: false),
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
