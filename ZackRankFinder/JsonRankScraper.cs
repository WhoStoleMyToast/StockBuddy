using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZackRankFinder
{
    public class JsonRankScraper : IRankScraper
    {
        private readonly IHttpClientFactory _httpFactory;
        private readonly ILogger _logger;

        public JsonRankScraper(IHttpClientFactory httpFactory, ILogger<RankScraper> logger)
        {
            _httpFactory = httpFactory;
            _logger = logger;
        }


        public async Task<int> GetRank(Stock stock)
        {
            return int.TryParse(stock?.zacks_rank, out int rank) ? rank : 6;
        }

        public async Task<Stock> GetStock(string symbol)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"https://quote-feed.zacks.com/index.php?t={symbol}");

            var client = _httpFactory.CreateClient();

            try
            {
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    string bodyText = await response.Content.ReadAsStringAsync();
                    int startIndex = bodyText.IndexOf(':') + 1;
                    bodyText = bodyText.Substring(startIndex, bodyText.Length - startIndex - 1);
                    var stock = JsonSerializer.Deserialize<Stock>(bodyText);

                    return stock;
                }
                else
                {
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogDebug(ex, "Something happened while scraping for ({Symbol})", symbol);

                return null;
            }
        }
    }


}
