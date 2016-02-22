namespace StockBuddy.Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Stock")]
    public partial class Stock
    {
        [Key]
        [StringLength(50)]
        public string Symbol { get; set; }

        public string Name { get; set; }

        public decimal? Rating { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
