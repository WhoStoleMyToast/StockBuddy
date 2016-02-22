using StockBuddy.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jane.Entities.Indicators
{
    public class ADX : IndicatorBase
    {
        public ADX() : base() { }

        public ADX(int period) : base(period) { }

        public ADX(int period, int daysBack) : base(period, daysBack) { }

        public override double Calculate(IList<History> history)
        {
            DX dx = new DX(14);
            dx.Calculate(history);

            double sum = 0.0;

            // filler
            for (int i = 0; i < Period; i++)
            {
                PastValues.Add(0.0);
            }

            var dxObj = new DX(14);
            var dxVal = dxObj.Calculate(history);

            // Get first
            for (int h = Period; h < Period * 2; h++)
            {
                PastValues.Add(0.0);
                sum += 0.0;
            }

            PastValues[Period * 2 - 1] = sum / (double)Period;

            for (int h = Period * 2; h < history.Count; h++)
            {
                PastValues.Add(((PastValues[PastValues.Count - 1] * (Period - 1)) +
                                  (dx.PastValues[h])) / (double)Period);
            }

            Value = PastValues[PastValues.Count - 1];

            return Value;
        }
    }
}
