using StockBuddy.Core.Domain;
using StockBuddy.Data.Context;
using StockBuddy.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBuddy.Data
{
    public class UnitOfWork : IDisposable
    {
        private GenericRepository<History> _historyRepository;
        private GenericRepository<Stock> _stockRepository;
        private GenericRepository<EntryStrategy> _entryStrategyRepository;

        private PeabodyDbContext context = new PeabodyDbContext();
        private bool disposed = false;

        public GenericRepository<History> HistoryRepository
        {
            get
            {
                if (_historyRepository == null)
                {
                    _historyRepository = new GenericRepository<History>(context);
                }

                return _historyRepository;
            }
        }

        public GenericRepository<Stock> StockRepository
        {
            get
            {
                if (_stockRepository == null)
                {
                    _stockRepository = new GenericRepository<Stock>(context);
                }

                return _stockRepository;
            }
        }

        public GenericRepository<EntryStrategy> EntryStrategyRepository
        {
            get
            {
                if (_stockRepository == null)
                {
                    _entryStrategyRepository = new GenericRepository<EntryStrategy>(context);
                }

                return _entryStrategyRepository;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }

            this.disposed = true;
        }
    }
}
