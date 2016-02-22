using StockBuddy.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jane.Entities.Indicators
{
    public class PlusSmoothDX : IndicatorBase
    {
        public PlusSmoothDX(int period) : base(period) { }

        public PlusSmoothDX(int period, int daysBack) : base(period, daysBack) { }

        public override double Calculate(IList<History> history)
        {
            double sumPlus = 0.0;

            PlusDX pdx = new PlusDX();
            pdx.Calculate(history);

            PastValues.Add(0.0);

            // Get first 
            for (int i = 1; i <= Period; i++)
            {
                PastValues.Add(pdx.PastValues[i]);
                sumPlus += pdx.PastValues[i];
            }

            // First smoothdx values
            PastValues[Period] = (sumPlus / (double)Period);

            for (int h = Period + 1; h < history.Count; h++)
            {
                PastValues.Add(((PastValues[PastValues.Count - 1] * (Period - 1)) +
                                  (pdx.PastValues[h])) / (double)Period);
            }

            Value = PastValues[PastValues.Count - 1];

            return Value;
        }
    }
}
