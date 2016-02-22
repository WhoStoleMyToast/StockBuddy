namespace StockBuddy.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedIsLongPositionProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EntryStrategy", "isLongPosition", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EntryStrategy", "isLongPosition");
        }
    }
}
