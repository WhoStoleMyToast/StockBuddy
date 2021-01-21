using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZackRankFinder
{
    public class RankScraper : IRankScraper
    {
        private readonly IHttpClientFactory _httpFactory;
        private readonly ILogger _logger;

        public RankScraper(IHttpClientFactory httpFactory, ILogger<RankScraper> logger)
        {
            _httpFactory = httpFactory;
            _logger = logger;
        }

        public async Task<int> GetRank(string symbol)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"https://www.zacks.com/stock/quote/{symbol}?q={symbol}");

            var client = _httpFactory.CreateClient();

            try
            {
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    string bodyText = await response.Content.ReadAsStringAsync();

                    string rankStr = "";

                    foreach (Match m in Regex.Matches(bodyText, "<span class=\"rank_chip rankrect_[\\d]\">(\\d)</span> </dd>"))
                    {
                        if (m.Groups.Count > 1)
                        {
                            try
                            {
                                rankStr = m.Groups[1].Value;
                            }
                            catch
                            { }
                        }
                    }

                    return int.TryParse(rankStr, out int rank) ? rank : 6;
                }
                else
                {
                    return 6;
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Something happened while scraping for ({Symbol})", symbol);

                return 6;
            }
        }
    }
}
