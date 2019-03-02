using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Monitoring.Repositories
{
    public interface IRepository<T> : IDisposable 
        where T : class
    {
        Task<IEnumerable<T>> GetAll();
        T Get(int id);
        Task<T> Get(Expression<Func<T, bool>> predicate);
        IEnumerable<T> Find(Func<T, bool> predicate);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
        void Save();
        IEnumerable<T> GetWithInclude(params Expression<Func<T, object>>[] includeProperties);
        IEnumerable<T> GetWithInclude(Func<T, bool> predicate, params Expression<Func<T, object>>[] includeProperties);
    }
}