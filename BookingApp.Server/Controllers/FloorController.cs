using BookingApp.Server.Models;
using BookingApp.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.Server.Controllers
{
    public class FloorController : ControllerBase
    {
        private readonly FloorService _FloorService;

        public FloorController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DatabaseConfig");
            _FloorService = new FloorService(connectionString);
        }


        [HttpPost("Floor/Create")]
        public async Task<IActionResult> CreateUser([FromBody] Floor? Floor)
        {
            try
            {
                if (Floor == null)
                    return BadRequest();

                var id = await _FloorService.AddAsync(Floor);
                return CreatedAtAction(nameof(GetUserById), new { id }, Floor);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpDelete("Floor/Delete")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var success = await _FloorService.DeleteAsync(id);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPut("Floor/Update")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] Floor? Floor)
        {
            try
            {
                if (id != Floor.id)
                    return BadRequest();

                var success = await _FloorService.UpdateAsync(Floor);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpGet("Floor/Search")]
        public async Task<IActionResult> Search(
                    [FromQuery] string? keyword = null,
                    [FromQuery] int pageNumber = 1,
                    [FromQuery] int pageSize = 20)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Invalid pagination parameters.");

            var result = await _FloorService.GetAsync(keyword, pageNumber, pageSize);
            var response = new
            {
                Data = result.Floors,
                TotalRecords = result.TotalRecords,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(result.TotalRecords / (double)pageSize)
            };

            return Ok(response);
        }

        [HttpGet("Floor/GetById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _FloorService.GetByIdAsync(id);

            if (user == null)
                return NotFound($"Floor with ID {id} not found.");

            return Ok(user);
        }


    }
}
