using StockBuddy.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jane.Entities.Indicators
{
    public class AverageTrueRange : IndicatorBase
    {
        public AverageTrueRange(int period) : base(period) { }

        public AverageTrueRange(int period, int daysBack) : base(period, daysBack) { }

        public override double Calculate(IList<History> history)
        {
            double sum = 0.0;

            TrueRange tr = new TrueRange();
            tr.Calculate(history);

            PastValues.Add(0.0);

            // Get first atr 
            for (int i = 1; i <= Period; i++)
            {
                PastValues.Add(0.0);
                sum += tr.PastValues[i];
            }

            // First ATR value
            PastValues[Period] = (sum / (double)Period);

            for (int h = Period + 1; h < history.Count; h++)
            {
                PastValues.Add(((PastValues[PastValues.Count - 1] * (Period - 1)) +
                                  (tr.PastValues[h])) / (double)Period);
            }

            Value = PastValues[PastValues.Count - 1];

            return Value;
        }
    }
}
