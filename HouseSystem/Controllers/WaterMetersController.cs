using Monitoring.Models.Meters;
using Monitoring;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        public IActionResult Create([FromBody]WaterMeter waterMeter)
        {
            if (ModelState.IsValid)
            {
                _monitor.AddMeter(waterMeter);
                return Ok();
            }

            return BadRequest();
        }
    }
}