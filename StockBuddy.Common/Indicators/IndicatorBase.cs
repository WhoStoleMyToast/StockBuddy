using StockBuddy.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jane.Entities.Indicators
{
    public abstract class IndicatorBase
    {
        #region Protected Members

        protected int Period;
        protected int DaysBack;

        #endregion

        #region Constructors

        protected IndicatorBase() : this(0) { }

        protected IndicatorBase(int period) : this(period, 0) { }

        protected IndicatorBase(int period, int daysBack)
        {
            Period = period;
            DaysBack = daysBack;

            PastValues = new List<double>();
        }

        #endregion

        #region Public Properties

        public List<double> PastValues { get; set; }

        public double Value { get; set; }

        #endregion

        #region Abstract Methods

        public abstract double Calculate(IList<History> history);

        #endregion
    }
}
