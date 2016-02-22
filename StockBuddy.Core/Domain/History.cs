namespace StockBuddy.Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("History")]
    public partial class History
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string Symbol { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "date")]
        public DateTime Date { get; set; }

        [Column(TypeName = "money")]
        public decimal OpenPrice { get; set; }

        [Column(TypeName = "money")]
        public decimal ClosePrice { get; set; }

        [Column(TypeName = "money")]
        public decimal AdustedClosePrice { get; set; }

        [Column(TypeName = "money")]
        public decimal PreviousClosePrice { get; set; }

        [Column(TypeName = "money")]
        public decimal HighPrice { get; set; }

        [Column(TypeName = "money")]
        public decimal LowPrice { get; set; }

        public int Volume { get; set; }

        public decimal? MACD { get; set; }

        public decimal? Divergence { get; set; }

        public decimal? EMA30 { get; set; }

        public decimal? EMA200 { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
