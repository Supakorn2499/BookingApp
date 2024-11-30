using BookingApp.Server.Models;
using BookingApp.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.Server.Controllers
{
    public class RentalTypeController : ControllerBase
    {
        private readonly RentalTypeService _RentalTypeService;

        public RentalTypeController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DatabaseConfig");
            _RentalTypeService = new RentalTypeService(connectionString);
        }


        [HttpPost("RentalType/Create")]
        public async Task<IActionResult> CreateUser([FromBody] RentalType? RentalType)
        {
            try
            {
                if (RentalType == null)
                    return BadRequest();

                var id = await _RentalTypeService.AddAsync(RentalType);
                return CreatedAtAction(nameof(GetUserById), new { id }, RentalType);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpDelete("RentalType/Delete")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var success = await _RentalTypeService.DeleteAsync(id);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPut("RentalType/Update")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] RentalType? RentalType)
        {
            try
            {
                if (id != RentalType.id)
                    return BadRequest();

                var success = await _RentalTypeService.UpdateAsync(RentalType);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpGet("RentalType/Search")]
        public async Task<IActionResult> Search(
                    [FromQuery] string? keyword = null,
                    [FromQuery] int pageNumber = 1,
                    [FromQuery] int pageSize = 20)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Invalid pagination parameters.");

            var result = await _RentalTypeService.GetAsync(keyword, pageNumber, pageSize);
            var response = new
            {
                Data = result.RentalTypes,
                TotalRecords = result.TotalRecords,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(result.TotalRecords / (double)pageSize)
            };

            return Ok(response);
        }

        [HttpGet("RentalType/GetById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _RentalTypeService.GetByIdAsync(id);

            if (user == null)
                return NotFound($"RentalType with ID {id} not found.");

            return Ok(user);
        }


    }
}
