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
using StockBuddy.Common.Utilities;
using StockBuddy.Common;
using StockBuddy.Core.Domain;
using StockBuddy.Data;
using System.Data.Entity.Validation;
using StockBuddy.Core.Services;
using StockBuddy.Services;

namespace StockBuddy.Realtime
{
    public class Program
    {
        private static readonly YahooDataProvider _yahooDataProvider = new YahooDataProvider();

        public static void Main(string[] args)
        {
            AutoMapperDomainConfiguration.Configure();

            UpdateQuotes();
        }

        private static void UpdateQuotes()
        {
            var symbols = StockUtils.GetSymbols().ToList();

            while (symbols != null && symbols.Count() > 0)
            {
                System.Console.Write("\r{0} Remaining", symbols.Count);

                var symbolsChunk = symbols.Take(100).ToList();
                XmlReader xr = _yahooDataProvider.GetLastTradeRealtimeWithTime(symbolsChunk);
                FillStockInfoRealTime(xr, symbolsChunk);
                symbols = symbols.Skip(100).ToList();
            }
        }

        private static void FillStockInfoRealTime(XmlReader xr, IEnumerable<string> symbols)
        {
            try
            {
                XDocument master = XDocument.Load(xr);
                IList<History> histories = new List<History>();

                IHistoryService _historyService = new HistoryService();
                foreach (var symbol in symbols)
                {
                    IEnumerable<XElement> elements =
                        from c in master.Root.Elements("results").Descendants("quote")
                        where c.Attribute("symbol").Value.ToLower() == symbol.ToLower()
                        select c;

                    elements = elements.Elements();

                    decimal lastTradePriceOnly;

                    var element = elements.FirstOrDefault(x => x.Name == "LastTradeRealtimeWithTime");

                    if (element != null && !string.IsNullOrEmpty(element.Value))
                    {
                        lastTradePriceOnly = ValueUtils.GetValidDecimal(element);

                    }
                    else
                    {
                        lastTradePriceOnly = ValueUtils.GetValidDecimal(elements.FirstOrDefault(x => x.Name == "LastTradePriceOnly"));
                    }

                    var daysHigh = ValueUtils.GetValidDecimal(elements.FirstOrDefault(x => x.Name == "DaysHigh"));
                    var daysLow = ValueUtils.GetValidDecimal(elements.FirstOrDefault(x => x.Name == "DaysLow"));
                    var volume = ValueUtils.GetValidInteger(elements.FirstOrDefault(x => x.Name == "Volume"));
                    var open = ValueUtils.GetValidDecimal(elements.FirstOrDefault(x => x.Name == "Open"));
                    var previousClose = ValueUtils.GetValidDecimal(elements.FirstOrDefault(x => x.Name == "PreviousClose"));

                    var dateStamp = DateTime.Now.Date;

                    // In case this was left running
                    if (dateStamp.DayOfWeek == DayOfWeek.Saturday)
                    {
                        dateStamp = dateStamp.AddDays(-1);
                    }
                    else if (dateStamp.DayOfWeek == DayOfWeek.Sunday)
                    {
                        dateStamp = dateStamp.AddDays(-2);
                    }

                    var history = _historyService.GetHistory(symbol, dateStamp);

                    if (history == null)
                    {
                        history = new History { AdustedClosePrice = lastTradePriceOnly, Date = dateStamp, ClosePrice = lastTradePriceOnly, HighPrice = daysHigh, LowPrice = daysLow, OpenPrice = open, PreviousClosePrice = previousClose, Symbol = symbol, Volume = volume };
                    }
                    else
                    {
                        history.AdustedClosePrice = lastTradePriceOnly;
                        history.ClosePrice = lastTradePriceOnly;
                        history.HighPrice = daysHigh;
                        history.LowPrice = daysLow;
                        history.OpenPrice = open;
                        history.PreviousClosePrice = previousClose;
                        history.Volume = volume;
                    }

                    

                    //history.ClosePrice = history.ClosePrice + (decimal).001;

                    histories.Add(history);
                }

                _historyService.AddOrUpdateHistories(histories);
                _historyService.Dispose();
            }
            catch
            { }

            finally
            {
                HttpUtils.CloseConnections(xr, true);
            }
        }
    }
}
