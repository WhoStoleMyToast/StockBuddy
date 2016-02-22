using StockBuddy.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace StockBuddy.Data
{
    public class YahooDataProvider
    {
        #region Historic Methods

        public XmlReader GetHistoryReader(string symbol, string startDate, string endDate)
        {
            return ExecuteHistoryReader(symbol, startDate, endDate);
        }

        #endregion

        public XmlReader ExecuteHistoryReader(params object[] commandParameters)
        {
            return HttpUtils.ExecuteReader("https://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20yahoo.finance.historicaldata%20where%20symbol%20%3D%20%22{0}%22%20and%20startDate%20%3D%20%22{1}%22%20and%20endDate%20%3D%20%22{2}%22%0A%09%09&diagnostics=false&env=http%3A%2F%2Fdatatables.org%2Falltables.env", commandParameters);
        }

        public XmlReader ExecuteQuotesReader(params object[] commandParameters)
        {
            return HttpUtils.ExecuteReader("https://query.yahooapis.com/v1/public/yql?q=select%20symbol%2C%20PreviousClose%2C%20Volume%2C%20Open%2C%20LastTradePriceOnly%2C%20LastTradeRealtimeWithTime%2C%20DaysHigh%2C%20DaysLow%20from%20yahoo.finance.quotes%20where%20symbol%20in%20({0})%0A%09%09&diagnostics=false&env=http%3A%2F%2Fdatatables.org%2Falltables.env", commandParameters);
        }

        public XmlReader GetLastTradeRealtimeWithTime(IEnumerable<string> symbols)
        {
            string symbolList = string.Empty;

            symbolList = string.Format("'{0}'", string.Join(",", symbols));

            return ExecuteQuotesReader(symbolList);
        }

        public decimal? GetStockRating(string symbol)
        {
            string stockUrl = string.Format("http://finance.yahoo.com/q?s={0}", symbol);
            string bodyText = HtmlUtils.DownloadHtml(stockUrl);
            decimal? rating = null;
            foreach (Match m in Regex.Matches(bodyText, "Mean Recommendation[*][:]</th><td class=\"yfnc_tabledata1\"[^>]*>(.*?)</td>"))
            {
                if (m.Value.Length > 0 && !m.Value.Contains("N/A"))
                {
                    try
                    {
                        string ratingStr = HtmlUtils.StripTags(m.Value, false);
                        string temp = ratingStr.Replace("Mean Recommendation*:", "");
                        rating = decimal.Parse(temp);
                    }
                    catch
                    { }
                }
            }

            return rating;
        }
    }
}
