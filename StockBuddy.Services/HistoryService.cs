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
    public class HistoryService : IHistoryService
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public IQueryable<History> GetAllHistories()
        {
            var histories = _unitOfWork.HistoryRepository.Get;

            return histories;
        }

        public IEnumerable<History> GetHistories(string symbol)
        {
            var histories = _unitOfWork.HistoryRepository.Get.Where(h => h.Symbol.ToLower() == symbol.ToLower());

            return histories;
        }

        public History GetHistory(string symbol, DateTime date)
        {
            var history = _unitOfWork.HistoryRepository.Find(new object[]{symbol, date.Date});

            return history;
        }

        public void AddOrUpdateHistorics(IEnumerable<Historic> historics)
        {
            var histories = MapperUtils.Map(historics);
            _unitOfWork.HistoryRepository.AddOrUpdate(histories.ToArray());

            _unitOfWork.Save();
        }

        public void AddOrUpdateHistories(IEnumerable<History> histories)
        {
            _unitOfWork.HistoryRepository.AddOrUpdate(histories.ToArray());

            _unitOfWork.Save();

        }

        public void UpsertHistories(Expression<Func<History, object>> identifierExpression, Expression<Func<History, object>> updatingExpression, IEnumerable<History> histories)
        {
            _unitOfWork.HistoryRepository.AddOrUpdate(identifierExpression, updatingExpression, histories.ToArray());

            _unitOfWork.Save();
        }

        public void DeleteStaleHistory(string symbol)
        {
            var oneYearAgo = DateTime.Now.AddYears(-1);

            var dbStaleHistories = _unitOfWork.HistoryRepository.Get.Where(h => h.Symbol == symbol && h.Date < oneYearAgo);
            _unitOfWork.HistoryRepository.RemoveRange(dbStaleHistories);
            _unitOfWork.Save();
        }
    }
}
