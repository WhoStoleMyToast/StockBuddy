namespace StockBuddy.Data.Migrations
{
    using StockBuddy.Core.Domain;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<StockBuddy.Data.Context.PeabodyDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(StockBuddy.Data.Context.PeabodyDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.EntryStrategies.AddOrUpdate(
                e => e.EntryStrategyID,
                new EntryStrategy 
                { 
                    Date = DateTime.Now.AddDays(-2),
                    MinADX = null,
                    MaxADX = null,
                    MinDivergence = (decimal)0.0,
                    MaxDivergence = (decimal)1.0,
                    MinMacd = (decimal)-10.0,
                    MaxMacd = (decimal)-1.0,
                    MinPercentChange = (decimal)-3.0,
                    MaxPercentChange = (decimal)3.0,
                    MinVolume = (decimal)1.5,
                    recentMacdCross = true,
                    isActive = true
                }
            );
        }
    }
}
