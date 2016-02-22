using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Threading;
using StockBuddy.Data;
using StockBuddy.Common.Utilities;
using StockBuddy.Core.Domain;
using StockBuddy.Core.Services;
using StockBuddy.Services;

namespace StockBuddy.Rating
{
    public class Program
    {
        private static readonly YahooDataProvider _yahooDataProvider = new YahooDataProvider();

        public static void Main(string[] args)
        {
            var symbols = StockUtils.GetSymbols().ToList();

            while (symbols != null && symbols.Count() > 0)
            {
                System.Console.Write("\r{0} Remaining", symbols.Count);

                var symbolsChunk = symbols.Take(10).ToList();
                UpdateRatings(symbolsChunk);
                symbols = symbols.Skip(10).ToList();
            }
        }

        private static void UpdateRatings(IEnumerable<string> symbols)
        {
            var dateTimeStamp = DateTime.Now;

            IList<Stock> stocks = new List<Stock>();

            IStockService _stockService = new StockService();

            //var symbols = new List<string>() { "aame", "dis"};
            foreach (var symbol in symbols.Select((value, index) => new { index, value }))
            {
                //System.Console.Write("\r{0} Remaining", symbols.Count - symbol.index);
                decimal? rating = _yahooDataProvider.GetStockRating(symbol.value);

                if (rating != null)
                {
                    stocks.Add(new Stock { ModifiedDate = dateTimeStamp, Symbol = symbol.value, Rating = rating.Value });

                    //System.Console.WriteLine("{0}: {1}", symbol.value, rating.Value);
                }

                Thread.Sleep(1000);
            }

            _stockService.AddOrUpdateStocks(stocks);
            _stockService.Dispose();
        }
    }
}
