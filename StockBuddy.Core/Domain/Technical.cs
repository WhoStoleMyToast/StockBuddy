namespace StockBuddy.Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Technical")]
    public partial class Technical
    {
        public Technical()
        {
            Computations = new HashSet<Computation>();
        }

        public int TechnicalID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public virtual ICollection<Computation> Computations { get; set; }
    }
}
