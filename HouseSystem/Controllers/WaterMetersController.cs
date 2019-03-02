using Monitoring.Models.Meters;
using Monitoring;
using Microsoft.AspNetCore.Mvc;

namespace HouseSystem.Controllers
{
    public class WaterMetersController : Controller
    {
        private IMonitor _monitor;

        public WaterMetersController(IMonitor monitor)
        {
            _monitor = monitor;
        }

        public IActionResult Create(WaterMeter waterMeter)
        {
            if (ModelState.IsValid)
            {
                _monitor.AddMeter(waterMeter);
            }

            return BadRequest();
        }
    }
}