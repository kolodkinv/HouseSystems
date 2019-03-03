using System;
using Monitoring;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Monitoring.Exceptions;
using Monitoring.Models.Meters;

namespace HouseSystem.Controllers
{
    [Route("api/[controller]")]
    public class WaterMetersController : Controller
    {
        private IMonitor _monitor;

        public WaterMetersController(IMonitor monitor)
        {
            _monitor = monitor;
        }

        /// <summary>
        /// Получение данных о водяном счетчике
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var meter = _monitor.GetMeter(id);
            if (meter != null)
            {
                return Ok(meter);
            }

            return NotFound();
        }
        
        /// <summary>
        /// Регистрация нового счетчика
        /// </summary>
        /// <param name="waterMeter">Водяной счетчик</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create([FromBody]WaterMeter waterMeter)
        {
            if (ModelState.IsValid)
            {
                _monitor.AddMeter(waterMeter); 
                return CreatedAtAction(nameof(Get), new { id = waterMeter.Id }, waterMeter);
            }

            return BadRequest();
        }
    }
}