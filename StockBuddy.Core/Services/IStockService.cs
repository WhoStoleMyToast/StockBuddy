using StockBuddy.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StockBuddy.Core.Services
{
    public interface IStockService
    {
        Stock GetStock(string symbol);
        void AddOrUpdateStocks(IEnumerable<Stock> stocks);
        void Dispose();
    }
}
