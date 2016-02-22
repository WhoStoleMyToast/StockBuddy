using StockBuddy.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jane.Entities.Indicators
{
    public class MinusDMI : IndicatorBase
    {
        public MinusDMI() : base() { }

        public MinusDMI(int period) : base(period) { }

        public MinusDMI(int period, int daysBack) : base(period, daysBack) { }

        public override double Calculate(IList<History> history)
        {
            MinusSmoothDX msdx = new MinusSmoothDX(14);
            msdx.Calculate(history);

            AverageTrueRange atr = new AverageTrueRange(14);
            atr.Calculate(history);

            for (int i = 0; i < Period; i++)
            {
                PastValues.Add(0.0);
            }

            for (int h = Period; h < history.Count; h++)
            {
                PastValues.Add((msdx.PastValues[h] / atr.PastValues[h]) * 100);
            }

            Value = PastValues[PastValues.Count - 1];

            return Value;
        }
    }
}
