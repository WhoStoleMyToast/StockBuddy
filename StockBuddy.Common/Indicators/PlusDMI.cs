using StockBuddy.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jane.Entities.Indicators
{
    public class PlusDMI : IndicatorBase
    {
        public PlusDMI() : base() { }

        public PlusDMI(int period) : base(period) { }

        public PlusDMI(int period, int daysBack) : base(period, daysBack) { }

        public override double Calculate(IList<History> history)
        {
            PlusSmoothDX psdx = new PlusSmoothDX(14);
            psdx.Calculate(history);

            AverageTrueRange atr = new AverageTrueRange(14);
            atr.Calculate(history);

            for (int i = 0; i < Period; i++)
            {
                PastValues.Add(0.0);
            }

            for (int h = Period; h < history.Count; h++)
            {
                PastValues.Add((psdx.PastValues[h] / atr.PastValues[h]) * 100);
            }

            Value = PastValues[PastValues.Count - 1];

            return Value;
        }
    }
}
