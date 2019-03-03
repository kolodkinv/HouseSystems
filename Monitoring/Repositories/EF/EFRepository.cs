using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Monitoring.Exceptions;

namespace Monitoring.Repositories.EF
{
    /// <summary>
    /// Обобщенный репозиторий работающий с Entity Framework
    /// </summary>
    /// <typeparam name="TEntity">Тип объекта с которым работает репозиторий</typeparam>
    public class EFRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        DbContext _context;
        DbSet<TEntity> _dbSet;
 
        public EFRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }
         
        /// <summary>
        /// Получение всех объектов в репозитории
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Получение объекта в репозитории по его Id
        /// </summary>
        /// <param name="id">Id объекта</param>
        /// <returns>Объект в репозитории</returns>
        public TEntity Get(int id)
        {
            return _dbSet.Find(id);
        }

        /// <summary>
        /// Получить список объектов удовлетворяющих условию
        /// </summary>
        /// <param name="predicate">Условие</param>
        /// <returns>Список найденных объектов</returns>
        public IEnumerable<TEntity> Find(Func<TEntity, bool> predicate)
        {
            return _dbSet.AsNoTracking()
                .AsEnumerable()
                .Where(predicate)
                .ToList();
        }

        /// <summary>
        /// Добавление объекта в репозиторий
        /// </summary>
        /// <param name="item">Добавляемый объекта</param>
        public void Create(TEntity item)
        {
            _dbSet.Add(item);
        }
        
        /// <summary>
        /// Редактирование объекта в репозитории
        /// </summary>
        /// <param name="item">Редактируемый объект</param>
        public void Update(TEntity item)
        {
            _context.Entry(item).State = EntityState.Modified;
            //_context.SaveChanges();
        }
        
        /// <summary>
        /// Сохранение изменений в репозитории
        /// </summary>
        public void Save()
        {
            _context.SaveChanges();
        }

        /// <summary>
        /// Удаление обхекта из репозитория
        /// </summary>
        /// <param name="id">Id удаляемого объекта</param>
        public void Delete(int id)
        {
            var item = Get(id);
            _dbSet.Remove(item);
            //_context.SaveChanges();
        }

        /// <summary>
        /// Получение списка объектов в репозитории включая выбранные свойства
        /// </summary>
        /// <param name="includeProperties">Включаемые в объекты свойства</param>
        /// <returns>Список объектов</returns>
        public async Task<IEnumerable<TEntity>> GetWithIncludeAsync(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return await Include(includeProperties).ToListAsync();
        }
 
        /// <summary>
        /// Получение списка объектов в репозитории удовлетворяюхищ условию включая выбранные свойства
        /// </summary>
        /// <param name="predicate">Условие фильтрации списка</param>
        /// <param name="includeProperties">Включаемые в объекты свойства</param>
        /// <returns>Список объектов</returns>
        public IEnumerable<TEntity> GetWithInclude(Func<TEntity,bool> predicate, 
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query =  Include(includeProperties);
            return query.AsEnumerable()
                .Where(predicate)
                .ToList();
        }
 
        /// <summary>
        /// Включение данных в объекты. Реализация ранней загрузки данных.
        /// </summary>
        /// <param name="includeProperties">Включаемые в объекты свойства</param>
        /// <returns></returns>
        private IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();
            return includeProperties
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        private bool _disposed = false;
 
        public virtual void Dispose(bool disposing)
        {
            if(!_disposed)
            {
                if(disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }
 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}