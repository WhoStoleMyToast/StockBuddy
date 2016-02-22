namespace StockBuddy.Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Pick
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal? Rating { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal ClosePrice { get; set; }
        public decimal AdustedClosePrice { get; set; }
        public decimal PreviousClosePrice { get; set; }
        public decimal HighPrice { get; set; }
        public decimal LowPrice { get; set; }
        public int Volume { get; set; }
        public decimal? MACD { get; set; }
        public decimal? Divergence { get; set; }
        public decimal? EMA30 { get; set; }
        public decimal? EMA200 { get; set; }
    }
}
