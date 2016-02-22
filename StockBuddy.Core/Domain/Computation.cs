namespace StockBuddy.Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Computation")]
    public partial class Computation
    {
        public int ComputationID { get; set; }

        public int TechnicalID { get; set; }

        public int StockID { get; set; }

        public double? CurrentValue { get; set; }

        public double? RealTimeValue { get; set; }

        [Column(TypeName = "money")]
        public decimal? RealTimePrice { get; set; }

        public virtual Stock Stock { get; set; }

        public virtual Technical Technical { get; set; }
    }
}
