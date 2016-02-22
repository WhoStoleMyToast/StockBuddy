namespace StockBuddy.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatedStockEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Stock",
                c => new
                    {
                        Symbol = c.String(nullable: false, maxLength: 50),
                        Rating = c.Decimal(precision: 18, scale: 2),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Symbol);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Stock");
        }
    }
}
