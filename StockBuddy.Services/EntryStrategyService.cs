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
    public class EntryStrategyService : IEntryStrategyService
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public EntryStrategy GetEntryStrategy(int id)
        {
            var entryStrategy = _unitOfWork.EntryStrategyRepository.Find(new object[] { id });

            return entryStrategy;
        }


        public EntryStrategy GetActiveEntryStrategy()
        {
            var entryStrategy = _unitOfWork.EntryStrategyRepository.Get.FirstOrDefault(x => x.isActive);

            return entryStrategy;
        }

        public IEnumerable<EntryStrategy> GetEntryStrategies()
        {
            var entryStrategies = _unitOfWork.EntryStrategyRepository.Get;

            return entryStrategies;
        }


        public void Update(EntryStrategy entryStrategy)
        {
            _unitOfWork.EntryStrategyRepository.Update(entryStrategy);

            _unitOfWork.Save();
        }
    }
}
