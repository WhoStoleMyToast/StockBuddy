namespace StockBuddy.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNamePropertyToStockEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stock", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stock", "Name");
        }
    }
}
