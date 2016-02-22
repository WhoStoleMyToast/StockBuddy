using StockBuddy.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StockBuddy.Core.Services
{
    public interface IHistoryService
    {
        IQueryable<History> GetAllHistories();
        IEnumerable<History> GetHistories(string symbol);
        History GetHistory(string symbol, DateTime date);
        void AddOrUpdateHistorics(IEnumerable<Historic> historics);
        void AddOrUpdateHistories(IEnumerable<History> histories);
        void UpsertHistories(Expression<Func<History, object>> identifierExpression, Expression<Func<History, object>> updatingExpression, IEnumerable<History> histories);
        void DeleteStaleHistory(string symbol);
        void Dispose();
    }
}
