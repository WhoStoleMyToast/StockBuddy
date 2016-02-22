using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBuddy.Common.Utilities
{
    public static class DateUtils
    {
        public static string GetValidDate(DateTime start)
        {
            return start.ToString("yyyy-MM-dd");
        }
    }
}
