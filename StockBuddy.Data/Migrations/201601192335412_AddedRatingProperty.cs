namespace StockBuddy.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRatingProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Archive", "Rating", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.History", "Rating", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.History", "Rating");
            DropColumn("dbo.Archive", "Rating");
        }
    }
}
