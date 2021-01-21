using System.Collections.Generic;
using System.Threading.Tasks;

namespace ZackRankFinder
{
    public interface ISymbolFetcher
    {
        Task<List<string>> GetSymbols();
    }
}
