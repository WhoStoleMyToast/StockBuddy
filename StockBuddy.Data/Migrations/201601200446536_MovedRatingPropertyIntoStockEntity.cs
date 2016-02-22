namespace StockBuddy.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MovedRatingPropertyIntoStockEntity : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Archive", "Rating");
            DropColumn("dbo.History", "Rating");
        }
        
        public override void Down()
        {
            AddColumn("dbo.History", "Rating", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Archive", "Rating", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
