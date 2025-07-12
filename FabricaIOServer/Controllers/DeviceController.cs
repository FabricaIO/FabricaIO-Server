using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FabricaIOServer.FabricaIOLib;
using FabricaIOServer.Models;

namespace FabricaIOServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly DeviceContext _context;

        public DeviceController(DeviceContext context)
        {
            _context = context;
        }

        // GET: api/Device
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Device>>> GetDevices()
        {
            return await _context.Devices.ToListAsync();
        }

        // GET: api/Device/id/5
        [HttpGet("id/{id}")]
        public async Task<ActionResult<Device>> GetDevice(int id)
        {
            var device = await _context.Devices.FindAsync(id);

            if (device == null)
            {
                return NotFound();
            }

            return device;
        }

         // GET: api/Device/actor-DFPeristalticPump
        [HttpGet("{name}")]
        public async Task<ActionResult<Device>> GetDevice(string name)
        {
            var exists = _context.Devices.Where(d => d.name == name);
            if (exists.Count() == 0)
            {
                return NotFound();
            }

            var device = await exists.FirstOrDefaultAsync();
            if (device == null)
            {
                return NotFound();
            }

            return device;
        }

        // PUT: api/Device/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutDevice(int id, Device device)
        // {
        //     if (id != device.id)
        //     {
        //         return BadRequest();
        //     }

        //     _context.Entry(device).State = EntityState.Modified;

        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!DeviceExists(id))
        //         {
        //             return NotFound();
        //         }
        //         else
        //         {
        //             throw;
        //         }
        //     }

        //     return NoContent();
        // }

        // POST: api/Device
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPost]
        // public async Task<ActionResult<Device>> PostDevice(Device device)
        // {
        //     _context.Devices.Add(device);
        //     await _context.SaveChangesAsync();

        //     return CreatedAtAction("GetDevice", new { id = device.id }, device);
        // }

        // // DELETE: api/Device/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteDevice(int id)
        // {
        //     var device = await _context.Devices.FindAsync(id);
        //     if (device == null)
        //     {
        //         return NotFound();
        //     }

        //     _context.Devices.Remove(device);
        //     await _context.SaveChangesAsync();

        //     return NoContent();
        // }

        private bool DeviceExists(int id)
        {
            return _context.Devices.Any(e => e.id == id);
        }
    }
}
