using System.Collections.Generic;
using System.Threading.Tasks;
using Monitoring.Models.Buildings;
using Monitoring.Models.Meters;

namespace HomeSystems.Monitoring
{
    public interface IMonitor
    {  
        /// <summary>
        /// Получить строение с максимальный потреблением
        /// </summary>
        /// <returns></returns>
        Task<Building> GetBuildingWithMaxWaterConsumption();

        /// <summary>
        /// Получение списка всех строений
        /// </summary>
        /// <returns></returns>
        Task<ICollection<Building>> GetAllBuildings();

        /// <summary>
        /// Получить строение
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Building GetBuilding(int id);

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