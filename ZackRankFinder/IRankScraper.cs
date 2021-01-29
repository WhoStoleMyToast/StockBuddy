using System.Threading.Tasks;

namespace ZackRankFinder
{
    public interface IRankScraper
    {
        Task<Stock> GetStock(string symbol);
        Task<int> GetRank(Stock stock);
    }
}
