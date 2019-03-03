using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Monitoring.Models.Buildings;
using Monitoring.Models.Meters;

namespace Monitoring
{
    public interface IMonitor : IDisposable 
    {  
        /// <summary>
        /// Получить строение с максимальный потреблением
        /// </summary>
        /// <returns></returns>
        Task<Building> GetBuildingWithMaxWaterConsumptionAsync();

        Task<IEnumerable<Building>> GetAllBuildingsAsync();
        
        Task<IEnumerable<Building>> GetAllBuildingsAsync(params Expression<Func<Building, object>>[] includeProperties);

        /// <summary>
        /// Получить строение
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Building GetBuilding(int id);

        Building GetBuilding(Func<Building, bool> predicate, params Expression<Func<Building, object>>[] includeProperties);

        Meter GetMeter(int id);

        /// <summary>
        /// Добавить строение к мониторингу
        /// </summary>
        /// <param name="building">Строение</param>
        void AddBuilding(Building building);

        /// <summary>
        /// Зарегистрировать новый измерительный прибор в строение
        /// </summary>
        /// <param name="meter">Регистрируемый измерительный прибор</param>
        /// <param name="building">Строение в котором регистрируют прибор</param>
        void AddMeter(Meter meter);
    }
}