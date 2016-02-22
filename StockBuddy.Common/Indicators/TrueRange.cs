using StockBuddy.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jane.Entities.Indicators
{
    class TrueRange : IndicatorBase
    {
        public TrueRange() : base() { }

        public TrueRange(int period) : base(period) { }

        public TrueRange(int period, int daysBack) : base(period, daysBack) { }

        public override double Calculate(IList<History> history)
        {
            double tr1;
            double tr2;
            double tr3;

            PastValues.Add(0.0);

            // Get [period] trs from beginning of history (250 records back) 
            for (int i = 1; i < history.Count; i++)
            {
                tr1 = (double)(history[i].HighPrice - history[i].LowPrice);
                tr2 = (double)(Math.Abs(history[i].HighPrice - history[i - 1].ClosePrice));
                tr3 = (double)(Math.Abs(history[i].LowPrice - history[i - 1].ClosePrice));

                PastValues.Add(Math.Max(tr1, Math.Max(tr2, tr3)));
            }

            Value = PastValues[PastValues.Count - 1];

            return Value;
        }
    }
}
