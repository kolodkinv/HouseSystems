using System.Collections.Generic;
using Monitoring.Models.Meters;

namespace Monitoring.Models.Buildings
{
    // Постройка у которой имеется набор измерительных приборов
    public class Building
    {
        public int Id { get; set; }
        // Уникальный адрес здания
        public string Address { get; set; }
        // Набор измерительных приборов.
        // Возможно расширение типов и количества счетчиков в зданиии
        public ICollection<Meter> Meters { get; set; }
        
        // Набор водянных счетчиков
        // Возможно расширение водяных счетчиков до количества больше 1
        public WaterMeter WaterMeter { get; set; }
    }
}