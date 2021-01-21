using System.Threading.Tasks;

namespace ZackRankFinder
{
    public interface IRankScraper
    {
        Task<int> GetRank(string symbol);
    }
}
