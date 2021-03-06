using System;
using System.Linq;
using System.Threading.Tasks;
using HouseSystem.Parameters;
using Monitoring;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Monitoring.Exceptions;
using Monitoring.Models.Buildings;
using Newtonsoft.Json;

namespace HouseSystem.Controllers
{
    [Route("api/[controller]")]
    public class HousesController: Controller
    {
        private readonly IMonitor _monitor;
        
        public HousesController(IMonitor monitor)
        {
            _monitor = monitor;
        }
        
        /// <summary>
        /// Получение информации о домах
        /// </summary>
        /// <param name="filter">Параметр фильтрации</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync(Filter filter)
        {
            if (ModelState.IsValid)
            {
                if (filter.IsEmpty())
                {
                    return Ok(await _monitor.GetAllBuildingsAsync(b => b.WaterMeter));
                    
                }
                return await FilterBuildingsAsync(filter);
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Фильтрация домов по параметрам
        /// </summary>
        /// <param name="filter">Параметры фильтрации</param>
        /// <returns></returns>
        private async Task<IActionResult> FilterBuildingsAsync(Filter filter)
        {
            if (!string.IsNullOrEmpty(filter.Max))
            {
                switch (filter.Max.ToLower())
                {
                    case "water":
                        return Ok(await _monitor.GetBuildingWithMaxWaterConsumptionAsync());
                    default:
                        ModelState.AddModelError(filter.Max, "Неопределенное знание");
                        break;
                }
            }
            else
            {
                ModelState.AddModelError(filter.Max, "Неподдерживаемый атрибут фильтрации");
            }
            
            return BadRequest(ModelState);
        }
        
        /// <summary>
        /// Получение детальной информации о доме
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var house = _monitor.GetBuilding(id);
            if (house != null)
            {
                return Ok(house);
            }

            return NotFound();
        }

        // Регистрация нового дома
        [HttpPost]
        public IActionResult Create([FromBody] House house)
        {
            if (ModelState.IsValid)
            {
                _monitor.AddBuilding(house);    
                return CreatedAtAction(nameof(Get), new { id = house.Id }, house);
            }

            return BadRequest(ModelState);
        }
        
    }
}