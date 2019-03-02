using System.Linq;
using System.Threading.Tasks;
using Monitoring;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _monitor.GetAllBuildingsAsync(b => b.WaterMeters));
        }

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

        [HttpGet("api/[controller]/max/water")]
        public async Task<IActionResult> GetWithMaxWaterAsync()
        {
            var house = await _monitor.GetBuildingWithMaxWaterConsumptionAsync();
            if (house != null)
            {
                return Ok(house);
            }

            return NotFound();
        }
    
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