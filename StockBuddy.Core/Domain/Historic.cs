using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBuddy.Core.Domain
{
    public class Historic
    {
        public int HistoricId { get; set; }
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public int Volume { get; set; }
        public double AdjustedClose { get; set; }
        public double PreviousClose { get; set; }

        public double? MACD { get; set; }
        public double MACD9DAYEMA { get; set; }
        public double TR { get; set; }
        public double ATR { get; set; }
        public double PlusDX { get; set; }
        public double MinusDX { get; set; }
        public double SmoothDXPlus { get; set; }
        public double SmoothDXMinus { get; set; }
        public double PlusDMI { get; set; }
        public double MinusDMI { get; set; }
        public double DX { get; set; }
        public double ADX { get; set; }

        public double Performance
        {
            get { return ((Close - PreviousClose) / PreviousClose) * 100; }
        }

        public double IntraPerformance
        {
            get { return ((Close - Open) / Open) * 100; }
        }
    }
}
