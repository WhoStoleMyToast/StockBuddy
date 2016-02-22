using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StockBuddy.Common.Utilities
{
    public static class ValueUtils
    {
        public static decimal GetValidDecimal(XElement value)
        {
            decimal goodValue;

            if (value != null)
            {
                if (decimal.TryParse(value.Value, out goodValue))
                {
                    return goodValue;
                }
            }

            return -1;
            
        }

        public static int GetValidInteger(XElement value)
        {
            int goodValue;

            if (value != null)
            {
                if (int.TryParse(value.Value, out goodValue))
                {
                    return goodValue;
                }
            }

            return -1;
        }
    }
}
