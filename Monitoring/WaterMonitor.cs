using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Monitoring.Exceptions;
using Monitoring.Models.Buildings;
using Monitoring.Models.Meters;
using Monitoring.Repositories;


namespace Monitoring
{
    public class WaterMonitor : IMonitor
    {
        private readonly IUnitOfWork _db;
        
        public WaterMonitor(IUnitOfWork db)
        {
            _db = db;
        }
        
        /// <summary>
        /// Получить строение с максимальный потреблением воды
        /// </summary>
        /// <returns>Данные о постройке</returns>
        public async Task<Building> GetBuildingWithMaxWaterConsumptionAsync()
        {
            // Находим водяной счетчик с макс. потреблением воды
            var waterMeters = await _db.WaterMeters.GetAllAsync();
            var waterMeterMax = waterMeters
                .OrderByDescending(m => m.Value)
                .FirstOrDefault();
            
            if (waterMeterMax != null)
            {
                return _db.Buildings
                    .GetWithInclude(b => b.Id == waterMeterMax.BuildingId, w => w.WaterMeter)
                    .FirstOrDefault();
            }

            return null;
        }
        
        /// <summary>
        /// Получить все постройки включая свойства
        /// </summary>
        /// <param name="includeProperties">Включаемые свойства</param>
        /// <returns>Список построек</returns>
        public async Task<IEnumerable<Building>> GetAllBuildingsAsync(
            params Expression<Func<Building, object>>[] includeProperties)
        {
            return await _db.Buildings.GetWithIncludeAsync(includeProperties);
        }

        /// <summary>
        /// Получить строение
        /// </summary>
        /// <param name="id">Id строения</param>
        /// <returns>Данные о строении</returns>
        public Building GetBuilding(int id)
        {
            return _db.Buildings.Get(id);
        }

        /// <summary>
        /// Получить список строений отфильтрованных по условии и включающих свойства
        /// </summary>
        /// <param name="predicate">Условие фильтрации</param>
        /// <param name="includeProperties">Включаемые свойства</param>
        /// <returns>Список строений</returns>
        public Building GetBuilding(Func<Building, bool> predicate, params Expression<Func<Building, object>>[] includeProperties)
        {
            return _db.Buildings.GetWithInclude(predicate, includeProperties).FirstOrDefault();
        }

        /// <summary>
        /// Получить данные о счетчике
        /// </summary>
        /// <param name="id">Id счетчика</param>
        /// <returns>Данные счетчика</returns>
        public Meter GetMeter(int id)
        {
            return _db.Meters.Get(id);
        }

        /// <summary>
        /// Добавить строение к мониторингу
        /// </summary>
        /// <param name="building">Строение</param>
        public void AddBuilding(Building building)
        {
            try
            {
                _db.Buildings.Create(building);
                _db.Save();
            }
            catch (DbUpdateException)
            {
                throw new DuplicateException("Строение с таким адресом уже зарегистрировано");
            }        
        }

        /// <summary>
        /// Зарегистрировать новый измерительный прибор в строении
        /// </summary>
        /// <param name="meter">Регистрируемый измерительный прибор</param>
        public void AddMeter(Meter meter)
        {
            var building = _db.Buildings.Get(meter.BuildingId);
            
            if (building == null)
            {
                throw new NotFoundException("Постройка не найдена");
            }

            // Определение типа счетчика
            switch (meter)
            {
                case WaterMeter waterMeter:
                {
                    // Удаление старых водяных счетчиков в строении
                    var oldWaterMeters = _db.WaterMeters.Find(w => w.BuildingId == building.Id);
                    foreach (var oldWaterMeter in oldWaterMeters)
                    {
                        _db.WaterMeters.Delete(oldWaterMeter.Id);
                    }

                    // Устновка нового водяного счетчика
                    building.WaterMeter = waterMeter;                           
                    break;        
                }
                default:
                    throw new ArgumentException("Неопределенный тип счетчика");                 
            }   
            
            try
            {
                _db.Save();
            }
            catch (DbUpdateException)
            {
                throw new DuplicateException("Водяной счетчик с таким номером уже зарегистрирован");
            }  
        }
        
        private bool _disposed = false;
 
        public virtual void Dispose(bool disposing)
        {
            if(!_disposed)
            {
                if(disposing)
                {
                    _db.Dispose();
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