using BookingApp.Server.Models;
using BookingApp.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.Server.Controllers
{
    public class MeterTypeController : ControllerBase
    {
        private readonly MeterTypeService _MeterTypeService;

        public MeterTypeController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DatabaseConfig");
            _MeterTypeService = new MeterTypeService(connectionString);
        }


        [HttpPost("MeterType/Create")]
        public async Task<IActionResult> CreateUser([FromBody] MeterType? MeterType)
        {
            try
            {
                if (MeterType == null)
                    return BadRequest();

                var id = await _MeterTypeService.AddAsync(MeterType);
                return CreatedAtAction(nameof(GetUserById), new { id }, MeterType);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpDelete("MeterType/Delete")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var success = await _MeterTypeService.DeleteAsync(id);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPut("MeterType/Update")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] MeterType? MeterType)
        {
            try
            {
                if (id != MeterType.id)
                    return BadRequest();

                var success = await _MeterTypeService.UpdateAsync(MeterType);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpGet("MeterType/Search")]
        public async Task<IActionResult> Search(
                    [FromQuery] string? keyword = null,
                    [FromQuery] int pageNumber = 1,
                    [FromQuery] int pageSize = 20)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Invalid pagination parameters.");

            var result = await _MeterTypeService.GetAsync(keyword, pageNumber, pageSize);
            var response = new
            {
                Data = result.MeterTypes,
                TotalRecords = result.TotalRecords,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(result.TotalRecords / (double)pageSize)
            };

            return Ok(response);
        }

        [HttpGet("MeterType/GetById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _MeterTypeService.GetByIdAsync(id);

            if (user == null)
                return NotFound($"MeterType with ID {id} not found.");

            return Ok(user);
        }


    }
}
