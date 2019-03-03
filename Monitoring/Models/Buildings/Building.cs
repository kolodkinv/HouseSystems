using System.Collections.Generic;
using Monitoring.Models.Meters;

namespace Monitoring.Models.Buildings
{
    /// <summary>
    /// Строение, к которого имеется набор измерительных счетчиков
    /// </summary>
    public class Building
    {
        public int Id { get; set; }
        
        /// <summary>
        /// Адрес здания. Его уникальность устанавливается через Fluent API
        /// </summary>
        public string Address { get; set; }
        
        /// <summary>
        /// Набор измерительных счетчиков в строении.
        /// Хранит в себе разные типы счетчиков
        /// </summary>
        public ICollection<Meter> Meters { get; set; }
        
        /// <summary>
        /// Водяной счетчик установленный в здании
        /// </summary>
        public WaterMeter WaterMeter { get; set; }
    }
}