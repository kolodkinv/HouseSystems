using System;
using System.Runtime.Serialization;
using Monitoring.Models.Buildings;
using Newtonsoft.Json;

namespace Monitoring.Models.Meters
{
    // Измерительный прибор
    public class Meter
    {
        private double _value;
        // Заводской номер прибора
        public int Id { get; set; }
        // Показания прибора
        public double Value
        {
            get => _value;
            set
            {
                if (value >= 0)
                {
                    _value = value;
                }
                else
                {
                    throw new ArgumentException("Показания прибора не могут быть отрицательными");
                }
            }
        }
        // Здание в котором он установлен
        [JsonIgnore] 
        [IgnoreDataMember] 
        public Building Building { get; set; }
        
        public int BuildingId { get; set; }
    }
}