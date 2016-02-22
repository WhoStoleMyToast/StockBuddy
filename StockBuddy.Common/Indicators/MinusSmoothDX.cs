using StockBuddy.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jane.Entities.Indicators
{
    public class MinusSmoothDX : IndicatorBase
    {
        public MinusSmoothDX(int period) : base(period) { }

        public MinusSmoothDX(int period, int daysBack) : base(period, daysBack) { }

        public override double Calculate(IList<History> history)
        {
            double sumMinus = 0.0;

            MinusDX mdx = new MinusDX();
            mdx.Calculate(history);

            PastValues.Add(0.0);

            // Get first 
            for (int i = 1; i <= Period; i++)
            {
                PastValues.Add(mdx.PastValues[i]);
                sumMinus += mdx.PastValues[i];
            }

            // First smoothdx values
            PastValues[Period] = (sumMinus / (double)Period);

            for (int h = Period + 1; h < mdx.PastValues.Count; h++)
            {
                PastValues.Add(((PastValues[PastValues.Count - 1] * (Period - 1)) +
                                  (mdx.PastValues[h])) / (double)Period);
            }

            Value = PastValues[PastValues.Count - 1];

            return Value;
        }
    }
}
