using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Monitoring.Models.Buildings;
using Newtonsoft.Json;

namespace Monitoring.Models.Meters
{
    /// <summary>
    /// Измерительный счетчик
    /// </summary>
    public class Meter
    {
        private double _value;
        private int _id;
        
        /// <summary>
        /// Заводской номер счетчика
        /// </summary>
        /// <exception cref="ArgumentException">Исключение при установке некорректного заводского номера</exception>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id
        {
            get => _id;
            set
            {
                if (value > 0)
                {
                    _id = value;
                }
                else
                {
                    throw new ArgumentException("Некорректный заводской номер счетчика");
                }
            }
        }
        
        /// <summary>
        /// Показания счетчика
        /// </summary>
        /// <exception cref="ArgumentException">Исключение при установке некорректного значения</exception>
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
        
        /// <summary>
        /// Постройка в котором установлен счетчик
        /// </summary>
        [JsonIgnore] 
        [IgnoreDataMember] 
        public Building Building { get; set; }
        
        /// <summary>
        /// Ид здания в котором установлен счетчик
        /// </summary>
        public int BuildingId { get; set; }
    }
}