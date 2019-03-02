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
            var waterMeters = await _db.WaterMeters.GetAllAsync();
            var waterMeterMax = waterMeters.OrderByDescending(m => m.Value).FirstOrDefault();
            return waterMeterMax?.Building;
        }

        public async Task<IEnumerable<Building>> GetAllBuildingsAsync()
        {
            return await _db.Buildings.GetAllAsync();
        }

        public async Task<IEnumerable<Building>> GetAllBuildingsAsync(params Expression<Func<Building, object>>[] includeProperties)
        {
            return await _db.Buildings.GetWithIncludeAsync(includeProperties);
        }

        public Building GetBuilding(int id)
        {
            return _db.Buildings.Get(id);
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
            
            if (meter is WaterMeter waterMeter)
            {
                // Очистка счетчиков воды т.к в текущей реализации возможен один счетчик для воды
                building.WaterMeters = new List<WaterMeter> {waterMeter};
                _db.Save();
            }   
        }
    }
}