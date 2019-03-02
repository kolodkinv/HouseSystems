using HouseSystem.Dto;
using Monitoring;
using Microsoft.AspNetCore.Mvc;
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
        
        [HttpPost]
        public IActionResult Create([FromBody]WaterMeterDto waterMeter)
        {
            if (ModelState.IsValid)
            {
                _monitor.AddMeter((WaterMeter)waterMeter);
                return CreatedAtAction(nameof(Get), new { id = waterMeter.Id }, (WaterMeter)waterMeter);
            }

            return BadRequest();
        }
    }
}