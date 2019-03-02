using System;
using Monitoring.Models.Buildings;

namespace Monitoring.Models.Meters
{
    // Измерительный прибор
    public class Meter
    {
        private double _value;
        // Заводской номер прибора
        public string Id { get; set; }
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
        public Building Building { get; set; }
    }
}