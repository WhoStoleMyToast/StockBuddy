using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockBuddy.Core.Domain
{
    public class HistoryDto
    {
        public string Symbol { get; set; }
        public System.DateTime Date { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal ClosePrice { get; set; }
        public decimal AdustedClosePrice { get; set; }
        public decimal PreviousClosePrice { get; set; }
        public decimal HighPrice { get; set; }
        public decimal LowPrice { get; set; }
        public int Volume { get; set; }
        public Nullable<decimal> MACD { get; set; }
        public Nullable<decimal> Divergence { get; set; }
        public decimal DivergenceAverage { get; set; }
    }
}