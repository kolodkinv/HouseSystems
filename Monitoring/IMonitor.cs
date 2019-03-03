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
        /// Получить строение с максимальный потреблением воды
        /// </summary>
        /// <returns>Данные о постройке</returns>
        Task<Building> GetBuildingWithMaxWaterConsumptionAsync();

        /// <summary>
        /// Получить все постройки включая свойства
        /// </summary>
        /// <param name="includeProperties">Включаемые свойства</param>
        /// <returns>Список построек</returns>
        Task<IEnumerable<Building>> GetAllBuildingsAsync(params Expression<Func<Building, object>>[] includeProperties);

        /// <summary>
        /// Получить строение
        /// </summary>
        /// <param name="id">Id строения</param>
        /// <returns>Данные о строении</returns>
        Building GetBuilding(int id);

        /// <summary>
        /// Получить список строений отфильтрованных по условии и включающих свойства
        /// </summary>
        /// <param name="predicate">Условие фильтрации</param>
        /// <param name="includeProperties">Включаемые свойства</param>
        /// <returns>Список строений</returns>
        Building GetBuilding(Func<Building, bool> predicate, 
            params Expression<Func<Building, object>>[] includeProperties);

        /// <summary>
        /// Получить данные о счетчике
        /// </summary>
        /// <param name="id">Id счетчика</param>
        /// <returns>Данные счетчика</returns>
        Meter GetMeter(int id);

        /// <summary>
        /// Добавить строение к мониторингу
        /// </summary>
        /// <param name="building">Строение</param>
        void AddBuilding(Building building);

        /// <summary>
        /// Зарегистрировать новый измерительный прибор в строении
        /// </summary>
        /// <param name="meter">Регистрируемый измерительный прибор</param>
        void AddMeter(Meter meter);
    }
}