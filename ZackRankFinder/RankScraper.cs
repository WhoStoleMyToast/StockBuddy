﻿using Microsoft.Extensions.Logging;
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

        public async Task<int> GetRank(Stock stock)
        {
            return int.TryParse(stock?.zacks_rank, out int rank) ? rank : 6;
        }

        public async Task<Stock> GetStock(string symbol)
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

                    Stock stock = new Stock();
                    stock.zacks_rank = rankStr;

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
