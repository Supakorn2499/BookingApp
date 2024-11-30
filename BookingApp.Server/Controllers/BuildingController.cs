using BookingApp.Server.Models;
using BookingApp.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.Server.Controllers
{
    public class BuildingController : ControllerBase
    {
        private readonly BuildingService _BuildingService;

        public BuildingController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DatabaseConfig");
            _BuildingService = new BuildingService(connectionString);
        }


        [HttpPost("Building/Create")]
        public async Task<IActionResult> CreateUser([FromBody] Building? Building)
        {
            try
            {
                if (Building == null)
                    return BadRequest();

                var id = await _BuildingService.AddAsync(Building);
                return CreatedAtAction(nameof(GetUserById), new { id }, Building);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpDelete("Building/Delete")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var success = await _BuildingService.DeleteAsync(id);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPut("Building/Update")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] Building? Building)
        {
            try
            {
                if (id != Building.id)
                    return BadRequest();

                var success = await _BuildingService.UpdateAsync(Building);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpGet("Building/Search")]
        public async Task<IActionResult> Search(
                    [FromQuery] string? keyword = null,
                    [FromQuery] int pageNumber = 1,
                    [FromQuery] int pageSize = 20)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Invalid pagination parameters.");

            var result = await _BuildingService.GetAsync(keyword, pageNumber, pageSize);
            var response = new
            {
                Data = result.Buildings,
                TotalRecords = result.TotalRecords,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(result.TotalRecords / (double)pageSize)
            };

            return Ok(response);
        }

        [HttpGet("Building/GetById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _BuildingService.GetByIdAsync(id);

            if (user == null)
                return NotFound($"Building with ID {id} not found.");

            return Ok(user);
        }


    }
}
