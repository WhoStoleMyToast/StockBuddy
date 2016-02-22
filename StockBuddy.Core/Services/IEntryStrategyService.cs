using StockBuddy.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StockBuddy.Core.Services
{
    public interface IEntryStrategyService
    {
        EntryStrategy GetActiveEntryStrategy();
        EntryStrategy GetEntryStrategy(int id);
        IEnumerable<EntryStrategy> GetEntryStrategies();
        void Update(EntryStrategy entryStrategy);
        void Dispose();
    }
}
