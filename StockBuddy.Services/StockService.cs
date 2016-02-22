using StockBuddy.Common.Utilities;
using StockBuddy.Core.Domain;
using StockBuddy.Core.Services;
using StockBuddy.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StockBuddy.Services
{
    public class StockService : IStockService
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public Stock GetStock(string symbol)
        {
            throw new NotImplementedException();
        }

        public void AddOrUpdateStocks(IEnumerable<Stock> stocks)
        {
            _unitOfWork.StockRepository.AddOrUpdate(stocks.ToArray());

            _unitOfWork.Save();
        }
    }
}
