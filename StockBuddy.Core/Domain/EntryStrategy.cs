using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StockBuddy.Core.Domain
{
    [Table("EntryStrategy")]
    public class EntryStrategy
    {
        [Key]
        public int EntryStrategyID { get; set; }

        public DateTime Date { get; set; }

        public decimal? MinADX { get; set; }

        public decimal? MaxADX { get; set; }

        public decimal? MinVolume { get; set; }

        public decimal? MinDivergence { get; set; }

        public decimal? MaxDivergence { get; set; }

        public decimal? MinPercentChange { get; set; }

        public decimal? MaxPercentChange { get; set; }

        public decimal? MaxMacd { get; set; }

        public decimal? MinMacd { get; set; }

        public bool useMacdSma { get; set; }

        public bool recentMacdCross { get; set; }

        public bool isLongPosition { get; set; }

        public bool isActive { get; set; }
    }
}