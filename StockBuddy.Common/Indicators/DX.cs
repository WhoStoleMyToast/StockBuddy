using StockBuddy.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jane.Entities.Indicators
{
    public class DX : IndicatorBase
    {
        public DX() : base() { }

        public DX(int period) : base(period) { }

        public DX(int period, int daysBack) : base(period, daysBack) { }

        public override double Calculate(IList<History> history)
        {
            PlusDMI pdmi = new PlusDMI(14);
            MinusDMI mdmi = new MinusDMI(14);

            pdmi.Calculate(history);
            mdmi.Calculate(history);

            for (int i = 0; i < Period; i++)
            {
                PastValues.Add(0.0);
            }

            for (int h = Period; h < history.Count; h++)
            {
                PastValues.Add((Math.Abs(pdmi.PastValues[h] - mdmi.PastValues[h]) /
                             (pdmi.PastValues[h] + mdmi.PastValues[h])) *
                             100);
            }

            Value = PastValues[PastValues.Count - 1];

            return Value;
        }
    }
}
