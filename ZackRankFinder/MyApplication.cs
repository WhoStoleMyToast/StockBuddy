using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ZackRankFinder
{
    public class MyApplication
    {
        private readonly ILogger _logger;
        private readonly IRankScraper _rankScraper;
        private readonly ISymbolFetcher _symbolFetcher;
        private EventId startEventId = new EventId(1, "startup");
        private EventId symbolEventId = new EventId(2, "symbols");
        private EventId rankEventId = new EventId(3, "rank");

        public MyApplication(ILogger<MyApplication> logger, IRankScraper rankScraper, ISymbolFetcher symbolFetcher)
        {
            _logger = logger;
            _rankScraper = rankScraper;
            _symbolFetcher = symbolFetcher;
        }

        public async Task<string> Run()
        {
            _logger.LogInformation(startEventId, "Application {applicationEvent} at {dateTime}", "Started", DateTime.UtcNow);

            _logger.LogInformation(symbolEventId, "Fetching symbols...");
            var symbols = await _symbolFetcher.GetSymbols();
            _logger.LogInformation(symbolEventId, "Fetched {numberOfSymbols} symbols.", symbols.Count());

            //var symbols = new List<string> { "AMZN" };
            while (symbols?.Any() == true)
            {
                var symbolsChunk = symbols.Take(10).ToList();

                Random rnd = new Random();
                int rand = rnd.Next(1, 6000);

                foreach (var symbol in symbolsChunk)
                {
                    int rank = await _rankScraper.GetRank(symbol);

                    if (rank == 1)
                    {
                        _logger.LogInformation(rankEventId, "{Symbol} - {dateTime}", symbol, DateTime.UtcNow);
                    }

                    Thread.Sleep(rand / 2);
                }

                Thread.Sleep(rand);

                symbols = symbols.Skip(10).ToList();
            }

            return "done";
        }
    }
}
