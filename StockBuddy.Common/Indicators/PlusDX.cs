using StockBuddy.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jane.Entities.Indicators
{
    public class PlusDX : IndicatorBase
    {
        public PlusDX() : base() { }

        public PlusDX(int period) : base(period) { }

        public PlusDX(int period, int daysBack) : base(period, daysBack) { }

        public override double Calculate(IList<History> history)
        {
            double hiDiff;
            double loDiff;

            //AverageTrueRange atr = new AverageTrueRange(14);
            //atr.Calculate(history);

            // First dxs are blank. Might have to set to NAN
            PastValues.Add(0.0);

            // Get [period] trs from beginning of history (250 records back) 
            for (int i = 1; i < history.Count; i++)
            {
                hiDiff = (double)(history[i].HighPrice - history[i - 1].HighPrice);
                loDiff = (double)(history[i - 1].LowPrice - history[i].LowPrice);

                if ((hiDiff < 0.0 && loDiff < 0.0) || (hiDiff == loDiff))
                {
                    PastValues.Add(0.0);
                }
                else if (hiDiff > loDiff)
                {
                    PastValues.Add(hiDiff);
                }
                else if (hiDiff < loDiff)
                {
                    PastValues.Add(0.0);
                }
            }

            Value = PastValues[PastValues.Count - 1];

            return Value;
        }
    }
}
