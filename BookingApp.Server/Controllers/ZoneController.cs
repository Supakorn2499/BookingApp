using BookingApp.Server.Models;
using BookingApp.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.Server.Controllers
{
    public class ZoneController : ControllerBase
    {
        private readonly ZoneService _ZoneService;

        public ZoneController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DatabaseConfig");
            _ZoneService = new ZoneService(connectionString);
        }


        [HttpPost("Zone/Create")]
        public async Task<IActionResult> CreateUser([FromBody] Zone? Zone)
        {
            try
            {
                if (Zone == null)
                    return BadRequest();

                var id = await _ZoneService.AddAsync(Zone);
                return CreatedAtAction(nameof(GetUserById), new { id }, Zone);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpDelete("Zone/Delete")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var success = await _ZoneService.DeleteAsync(id);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPut("Zone/Update")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] Zone? Zone)
        {
            try
            {
                if (id != Zone.id)
                    return BadRequest();

                var success = await _ZoneService.UpdateAsync(Zone);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpGet("Zone/Search")]
        public async Task<IActionResult> Search(
                    [FromQuery] string? keyword = null,
                    [FromQuery] int pageNumber = 1,
                    [FromQuery] int pageSize = 20)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Invalid pagination parameters.");

            var result = await _ZoneService.GetAsync(keyword, pageNumber, pageSize);
            var response = new
            {
                Data = result.Zones,
                TotalRecords = result.TotalRecords,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(result.TotalRecords / (double)pageSize)
            };

            return Ok(response);
        }

        [HttpGet("Zone/GetById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _ZoneService.GetByIdAsync(id);

            if (user == null)
                return NotFound($"Zone with ID {id} not found.");

            return Ok(user);
        }


    }
}
