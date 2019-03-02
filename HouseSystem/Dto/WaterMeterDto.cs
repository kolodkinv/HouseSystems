using System;
using Monitoring.Models.Buildings;
using Monitoring.Models.Meters;

namespace HouseSystem.Dto
{
    public class WaterMeterDto
    {
        private double _value;
        
        public string Id { get; set; }
        
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
        
        public int BuildingId { get; set; }
        
        public static explicit operator WaterMeter(WaterMeterDto item)
        {
            return new WaterMeter
            {
                Id = item.Id,
                Value = item.Value,
                Building = new Building
                {
                    Id = item.BuildingId
                }
            };
        }
    }
}