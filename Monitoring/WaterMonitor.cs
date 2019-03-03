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
        private IUnitOfWork _db;
        
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
                // Собираем информацию о строении в котором устновлен счетчик
                var fullWaterMeterMax = _db.WaterMeters
                    .GetWithInclude(m => m.Id == waterMeterMax.Id, wm => wm.Building)
                    .FirstOrDefault();
                return fullWaterMeterMax?.Building;
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
            var building = _db.Buildings
                .GetWithInclude(b => b.Id == meter.Building.Id, wm => wm.WaterMeters)
                .FirstOrDefault();
            
            if (building == null)
            {
                throw new NotFoundException("Постройка не найдена");
            }
            
            if (meter is WaterMeter waterMeter)
            {
                // Очистка счетчиков воды т.к в текущей реализации возможен один счетчик для воды
                if (building.WaterMeters != null)
                {
                    foreach (var oldWaterMeter in building.WaterMeters)
                    {
                        _db.WaterMeters.Delete(oldWaterMeter.Id);
                    } 
                }
                
                // Сохранение нового водяного счетчика
                var item = _db.Buildings.Get(building.Id);
                item.WaterMeters = new List<WaterMeter> {waterMeter};

                _db.Save();
            }   
        }
    }
}