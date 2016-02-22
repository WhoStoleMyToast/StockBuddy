using StockBuddy.Core.Domain;
using System;
using System.Data;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using StockBuddy.Data.Extensions;

namespace StockBuddy.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T>
    where T : class
    {
        readonly DbContext _context;

        public GenericRepository(DbContext context)
        {
            _context = context;
        }

        public IQueryable<T> Get
        {
            get { return _context.Set<T>(); }
        }

        public IQueryable<T> GetIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public T Find(object[] keyValues)
        {
            return _context.Set<T>().Find(keyValues);
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                _context.Set<T>().Attach(entity);
                entry = _context.Entry(entity);
            }
            entry.State = EntityState.Modified;
        }

        public void AddOrUpdate(T entity)
        {
            //uses DbContextExtensions to check value of primary key
            _context.Set<T>().AddOrUpdate(entity);
        }

        public void AddOrUpdate(T[] entities)
        {
            //uses DbContextExtensions to check value of primary key
            _context.Set<T>().AddOrUpdate(entities);
        }

        public void AddOrUpdate(Expression<Func<T, object>> identifierExpression, Expression<Func<T, object>> updatingExpression, params T[] entities)
        {
            _context.Upsert(identifierExpression, updatingExpression, entities);
        }

        public void Delete(object id)
        {
            T entityToDelete = _context.Set<T>().Find(id);

            Delete(entityToDelete);
        }

        public void Delete(T entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _context.Set<T>().Attach(entityToDelete);
            }

            _context.Set<T>().Remove(entityToDelete);
        }

        public void RemoveRange(IEnumerable<T> entitiesToRemove)
        {
            _context.Set<T>().RemoveRange(entitiesToRemove);
        }
    }
}

