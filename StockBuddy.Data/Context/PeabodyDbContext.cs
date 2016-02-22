namespace StockBuddy.Data.Context
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using StockBuddy.Core.Domain;

    public partial class PeabodyDbContext : DbContext
    {
        public PeabodyDbContext()
            : base("name=PeabodyDbContext")
        {
        }

        public virtual DbSet<Stock> Stocks { get; set; }
        public virtual DbSet<Archive> Archives { get; set; }
        public virtual DbSet<History> Histories { get; set; }
        public virtual DbSet<EntryStrategy> EntryStrategies { get; set; }
        public virtual DbSet<ExitStrategy> ExitStrategies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Archive>()
                .Property(e => e.Symbol)
                .IsUnicode(false);

            modelBuilder.Entity<Archive>()
                .Property(e => e.OpenPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Archive>()
                .Property(e => e.ClosePrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Archive>()
                .Property(e => e.AdustedClosePrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Archive>()
                .Property(e => e.PreviousClosePrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Archive>()
                .Property(e => e.HighPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Archive>()
                .Property(e => e.LowPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Archive>()
                .Property(e => e.MACD)
                .HasPrecision(8, 2);

            modelBuilder.Entity<Archive>()
                .Property(e => e.Divergence)
                .HasPrecision(8, 2);

            modelBuilder.Entity<Archive>()
                .Property(e => e.EMA30)
                .HasPrecision(8, 2);

            modelBuilder.Entity<Archive>()
                .Property(e => e.EMA200)
                .HasPrecision(8, 2);

            modelBuilder.Entity<History>()
                .Property(e => e.Symbol)
                .IsUnicode(false);

            modelBuilder.Entity<History>()
                .Property(e => e.OpenPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<History>()
                .Property(e => e.ClosePrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<History>()
                .Property(e => e.AdustedClosePrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<History>()
                .Property(e => e.PreviousClosePrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<History>()
                .Property(e => e.HighPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<History>()
                .Property(e => e.LowPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<History>()
                .Property(e => e.MACD)
                .HasPrecision(8, 2);

            modelBuilder.Entity<History>()
                .Property(e => e.Divergence)
                .HasPrecision(8, 2);

            modelBuilder.Entity<History>()
                .Property(e => e.EMA30)
                .HasPrecision(8, 2);

            modelBuilder.Entity<History>()
                .Property(e => e.EMA200)
                .HasPrecision(8, 2);
        }
    }
}
