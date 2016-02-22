using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using System.Xml;
using System.Data.Entity.Validation;
using System.Xml.Linq;
using System.Threading;
using StockBuddy.Console;
using StockBuddy.Common.Utilities;
using StockBuddy.Common;
using StockBuddy.Core.Domain;
using StockBuddy.Data;

namespace StockBuddy.Con
{
    public class Program
    {
        private static readonly YahooDataProvider _yahooDataProvider = new YahooDataProvider();

        public static int Main(string[] args)
        {
            //Func<Referral, bool> LastNamesMatch = delegate(Referral r)
            //{
            //    string currentUsername = UserUtils.GetCurrentUsername(User.Identity.Name);
            //    string truncatedCurrentUsername = currentUsername.Substring(0, 8);
            //    string referrerUsername = r.ReferrerName.Substring(0, 1) + r.ReferrerName.Substring(r.ReferrerName.IndexOf(" ") + 1, 7);

            //    return referrerUsername.Equals(truncatedCurrentUsername);
            //}; 

            AutoMapperDomainConfiguration.Configure();
            //var choice = DisplayMenu();

            //if (args[0] == null)
            //{
            //    if (choice == 1)
            //    {
            //        SyncHistory();
            //    }
            //    else if (choice == 2)
            //    {
            //        UpdateQuotes();
            //    }
            //}
            //else
            //{
            //    UpdateQuotes();
            //}

            UpdateQuotes();

            //System.Console.ReadLine();

            return 0;
        }

        private static void SyncHistory()
        {
            var symbols = GetSymbols();
            var historyDictionary = new Dictionary<string, List<Historic>>();
            var oneYearAgo = DateTime.Now.AddYears(-1);
            List<Historic> historics = null;
            var flag = false;

            foreach (var symbol in symbols)
            {
                if (symbol == "OVBC" || flag)
                {
                    flag = true;
                    using (var db = new PeabodyDataContext())
                    {
                        //Delete stale data (more than a year old)
                        var dbStaleHistories = db.Histories.Where(h => h.Symbol == symbol && h.Date < oneYearAgo);
                        db.Histories.RemoveRange(dbStaleHistories);
                        db.SaveChanges();

                        var dbHistories = db.Histories.Where(h => h.Symbol == symbol);

                        // If there's no history for the symbol on the new list, add a year of history
                        if (dbHistories.Count() == 0)
                        {
                            historics = GetHistory(symbol, DateTime.Now.AddYears(-1), DateTime.Now);
                        }
                        else
                        {
                            //if (dbHistories.OrderByDescending(t => t.Date).First().Date == DateTime.Now.Date)
                            //{
                            //    continue;
                            //}

                            // Add history beginning from it's earliest record
                            historics = GetHistory(symbol, dbHistories.OrderByDescending(t => t.Date).First().Date.AddDays(1), DateTime.Now);
                        }

                        // If we were able to pull records from yahoo, add or update database
                        if (historics != null)
                        {
                            var histories = AutoMapper.Mapper.Map<IEnumerable<Historic>, IEnumerable<History>>(historics);

                            db.Histories.AddOrUpdate(histories.ToArray());

                            try
                            {
                                db.SaveChanges();

                                System.Console.WriteLine("{0}", symbol);

                            }
                            catch (DbEntityValidationException e)
                            {
                                foreach (var eve in e.EntityValidationErrors)
                                {
                                    foreach (var ve in eve.ValidationErrors)
                                    {
                                        System.Console.WriteLine("Main --> {0}", ve.ErrorMessage);
                                    }
                                }
                                throw;
                            }
                        }
                    }

                    Thread.Sleep(2000);
                }
            }
        }

        private static void UpdateQuotes()
        {
            var symbols = GetSymbols().ToList();

            while (symbols != null)
            {
                System.Console.Write("\r{0} Remaining", symbols.Count);

                var symbolsChunk = symbols.Take(100).ToList();
                XmlReader xr = _yahooDataProvider.GetLastTradeRealtimeWithTime(symbolsChunk);
                FillStockInfoRealTime(xr, symbolsChunk);
                symbols = symbols.Skip(120).ToList();
            }
        }

        private static void FillStockInfoRealTime(XmlReader xr, IEnumerable<string> symbols)
        {
            try
            {
                XDocument master = XDocument.Load(xr);

                using (var db = new PeabodyDataContext())
                {
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
                        if(dateStamp.DayOfWeek == DayOfWeek.Saturday)
                        {
                            dateStamp = dateStamp.AddDays(-1);
                        }
                        else if (dateStamp.DayOfWeek == DayOfWeek.Sunday)
                        {
                            dateStamp = dateStamp.AddDays(-2);
                        }

                        db.Histories.AddOrUpdate(h => new { h.Symbol, h.Date }, new StockBuddy.Core.Domain.History { AdustedClosePrice = lastTradePriceOnly, Date = dateStamp, ClosePrice = lastTradePriceOnly, HighPrice = daysHigh, LowPrice = daysLow, OpenPrice = open, PreviousClosePrice = previousClose, Symbol = symbol, Volume = volume });
                    }

                    db.SaveChanges();
                }
            }
            catch
            { }

            finally
            {
                HttpUtils.CloseConnections(xr, true);
            }
        }

        private static List<Historic> GetHistory(string symbol, DateTime startDate, DateTime endDate)
        {
            XmlReader xr = _yahooDataProvider.GetHistoryReader(symbol, GetValidDate(startDate), GetValidDate(endDate));

            List<Historic> history = new List<Historic>();

            try
            {
                xr.ReadToFollowing("quote");

                while (xr.Name != "results" && xr.NodeType != XmlNodeType.EndElement)
                {
                    try
                    {
                        //fill business object
                        Historic historic = FillHistoricInfo(xr, false);

                        historic.Symbol = symbol;

                        //add to collection
                        history.Add(historic);
                    }
                    catch(Exception ex)
                    {
                        System.Console.WriteLine("No data loaded for {0}. FillHistoricInfo --> {1}", symbol, ex.Message);
                        return null;
                    }
                }

                // Set Previous Closes
                for (int i = 0; i < history.Count - 1; i++)
                {
                    history[i].PreviousClose = history[i + 1].Close;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("GetHistory --> {0}", ex.Message);
            }
            finally
            {
                //close datareader
                HttpUtils.CloseConnections(xr, true);
            }

            return history.Take(250).ToList();
        }

        private static Historic FillHistoricInfo(XmlReader xr, bool closeXmlReader)
        {
            Historic historic = null;
            try
            {
                //read datareader
                bool bContinue = true;
                if (closeXmlReader)
                {
                    bContinue = false;
                    if (!xr.EOF)
                    {
                        bContinue = true;
                    }
                }
                if (bContinue)
                {
                    historic = new Historic();
                    xr.ReadToDescendant("Date");
                    historic.Date = xr.ReadElementContentAsDateTime();
                    historic.Open = xr.ReadElementContentAsDouble();
                    historic.High = xr.ReadElementContentAsDouble();
                    historic.Low = xr.ReadElementContentAsDouble();
                    historic.Close = xr.ReadElementContentAsDouble();
                    historic.Volume = xr.ReadElementContentAsInt();
                    historic.AdjustedClose = xr.ReadElementContentAsDouble();
                    xr.ReadEndElement();
                }
            }
            catch
            {
                throw;
            }

            return historic;
        }

        private static string GetValidDate(DateTime start)
        {
            return start.ToString("yyyy-MM-dd");
        }

        private static List<string> GetSymbols()
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

        private static int DisplayMenu()
        {
            System.Console.WriteLine(" ------------------------------------------");
            System.Console.WriteLine("|               Stock Buddy                |");
            System.Console.WriteLine(" ------------------------------------------");
            System.Console.WriteLine();
            System.Console.WriteLine("1. Sync History");
            System.Console.WriteLine("2. Run Real-Time Updater");
            System.Console.WriteLine("3. Exit");
            System.Console.WriteLine("");
            System.Console.Write("Choice: ");
            var result = System.Console.ReadLine();
            return Convert.ToInt32(result);
        }

        //public static void GetLastTradeRealtimeWithTime(IEnumerable<string> symbols)
        //{
        //    //int count = 0;
        //    //var stockList = new List<Stock>();

        //    //foreach (var symbol in symbols)
        //    //{
        //    //    stockList.Add(stock);
        //    //    count++;

        //    //    if (count % 500 == 0 || count == stocks.Count)
        //    //    {
        //    //        XmlReader xr = _yahooDataProvider.GetLastTradeRealtimeWithTime(ref stockList);
        //    //        FillStockInfoRealTime(xr, true, ref stockList);

        //    //        stockList.Clear();
        //    //    }
        //    //}

        //    while(symbols != null)
        //    { 
        //        XmlReader xr = GetLastTradeRealtimeWithTime(symbols.Take(500));
        //        FillStockInfoRealTime(xr, true, ref stockList);
        //        symbols = symbols.Skip(500);
        //    }
        //}
    }
}
