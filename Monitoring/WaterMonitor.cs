using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
        /// Получить строение с максимальным показателем потребления воды
        /// </summary>
        /// <returns>Строение с максимальным показателем потребления воды</returns>
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

        public async Task<IEnumerable<Building>> GetAllBuildingsAsync()
        {
            return await _db.Buildings.GetAllAsync();
        }

        public async Task<IEnumerable<Building>> GetAllBuildingsAsync(
            params Expression<Func<Building, object>>[] includeProperties)
        {
            return await _db.Buildings.GetWithIncludeAsync(includeProperties);
        }

        public Building GetBuilding(int id)
        {
            return _db.Buildings.Get(id);
        }

        public Meter GetMeter(int id)
        {
            return _db.Meters.Get(id);
        }

        public void AddBuilding(Building building)
        {
            _db.Buildings.Create(building);
        }

        public void AddMeter(Meter meter)
        {
            var building = _db.Buildings.Get(meter.Building.Id);
            
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
            
            _db.Save();
        }
    }
}