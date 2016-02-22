using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StockBuddy.Common.Utilities
{
    public static class StockUtils
    {
        public static List<string> GetSymbols()
        {
            var symbols = new List<string>();

            try
            {
                WebClient client = new WebClient();
                Stream stream = client.OpenRead("ftp://ftp.nasdaqtrader.com/SymbolDirectory/otherlisted.txt");
                StreamReader reader = new StreamReader(stream);
                string otherListed = reader.ReadToEnd();
                reader.Close();
                otherListed = otherListed.Replace("ACT Symbol|Security Name|Exchange|CQS Symbol|ETF|Round Lot Size|Test Issue|NASDAQ Symbol\r\n", "");

                stream = client.OpenRead("ftp://ftp.nasdaqtrader.com/SymbolDirectory/nasdaqlisted.txt");
                reader = new StreamReader(stream);
                string nasdaqListed = reader.ReadToEnd();
                reader.Close();
                nasdaqListed = nasdaqListed.Replace("Symbol|Security Name|Market Category|Test Issue|Financial Status|Round Lot Size\r\n", "");

                var stocksStr = (nasdaqListed + otherListed);

                var stocks = stocksStr.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var stock in stocks)
                {
                    symbols.Add(stock.Split(new char[] { '|' })[0]);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("GetSymbols --> {0}", ex.Message);
            }

            return symbols;
        }
    }
}
