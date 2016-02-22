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
using StockBuddy.Core.Services;
using StockBuddy.Services;

namespace StockBuddy.Sync
{
    public class Program
    {
        private static readonly YahooDataProvider _yahooDataProvider = new YahooDataProvider();

        public static int Main(string[] args)
        {
            AutoMapperDomainConfiguration.Configure();

            SyncHistory();

            return 0;
        }

        private static void SyncHistory()
        {
            Console.WriteLine("Getting Symbols...");         
            var symbols = StockUtils.GetSymbols();
            Console.WriteLine("");
            Console.WriteLine("Found {0} symbols", symbols.Count());
            List<Historic> historics = null;

            foreach (var symbol in symbols)
            {
                IHistoryService _historyService = new HistoryService();

                //Delete stale data (more than a year old)
                Console.WriteLine("{0} -- Deleting stale data...", symbol);

                _historyService.DeleteStaleHistory(symbol);

                var dbHistories = _historyService.GetHistories(symbol);

                // If there's no history for the symbol on the new list, add a year of history
                if (dbHistories.Count() == 0)
                {
                    Console.WriteLine("{0} -- No history found. Getting a years worth...", symbol);

                    historics = GetHistory(symbol, DateTime.Now.AddYears(-1), DateTime.Now);

                    if (historics != null && historics.Count < 210)
                    {
                        Console.WriteLine("{0} -- Not enough historic records found on yahoo ({1}). [SKIPPED]...", symbol, historics.Count);

                        continue;
                    }
                }
                else
                {
                    //if (dbHistories.OrderByDescending(t => t.Date).First().Date == DateTime.Now.Date)
                    //{
                    //    continue;
                    //}

                    // Add history beginning from it's latest record
                    //var latestHistoryDate = dbHistories.OrderByDescending(t => t.Date).First().Date;
                    
                    //Going a month back (re-calibrating)
                    var latestHistoryDate = dbHistories.OrderByDescending(t => t.Date).First().Date.AddMonths(-1);

                    Console.WriteLine("{0} -- DB History found ({1}). Getting Yahoo history starting at {2}...", symbol, dbHistories.Count(), latestHistoryDate.AddDays(1).ToString());
                    historics = GetHistory(symbol, latestHistoryDate.AddDays(1), DateTime.Now);
                }

                // If we were able to pull records from yahoo, add/update database
                if (historics != null)
                {
                    Console.WriteLine("{0} -- Yahoo History found. Adding {1} records...", symbol, historics.Count());

                    _historyService.AddOrUpdateHistorics(historics);
                }

                _historyService.Dispose();
                Console.WriteLine("~~~ Sleeping ~~~");
                Thread.Sleep(2000);
            }
        }

        // Yahoo
        private static List<Historic> GetHistory(string symbol, DateTime startDate, DateTime endDate)
        {
            XmlReader xr = _yahooDataProvider.GetHistoryReader(symbol, DateUtils.GetValidDate(startDate), DateUtils.GetValidDate(endDate));

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
                    catch (Exception ex)
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

        // Yahoo
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
    }
}
