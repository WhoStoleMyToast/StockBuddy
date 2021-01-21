using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ZackRankFinder
{
    public class SymbolFetcher : ISymbolFetcher
    {
        private ILogger _logger;

        public SymbolFetcher(ILogger<SymbolFetcher> logger)
        {
            _logger = logger;
        }

        public async Task<List<string>> GetSymbols()
        {
            var symbols = new List<string>();

            try
            {
                string otherListed = string.Empty;
                FtpWebRequest request =
    (FtpWebRequest)WebRequest.Create("ftp://ftp.nasdaqtrader.com/SymbolDirectory/otherlisted.txt");
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                using (Stream stream = request.GetResponse().GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    otherListed = await reader.ReadToEndAsync();
                }

                otherListed = otherListed.Replace("ACT Symbol|Security Name|Exchange|CQS Symbol|ETF|Round Lot Size|Test Issue|NASDAQ Symbol\r\n", "");

                var stocks = otherListed.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var stock in stocks)
                {
                    symbols.Add(stock.Split(new char[] { '|' })[0]);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetSymbols --> {0}");
            }

            return symbols;
        }
    }
}
