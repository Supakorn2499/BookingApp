using BookingApp.Server.Models;
using BookingApp.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.Server.Controllers
{
    public class RentalSpaceController : ControllerBase
    {
        private readonly RentalSpaceService _RentalSpaceService;

        public RentalSpaceController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DatabaseConfig");
            _RentalSpaceService = new RentalSpaceService(connectionString);
        }


        [HttpPost("RentalSpace/Create")]
        public async Task<IActionResult> CreateUser([FromBody] RentalSpace? RentalSpace)
        {
            try
            {
                if (RentalSpace == null)
                    return BadRequest();

                var id = await _RentalSpaceService.AddAsync(RentalSpace);
                return CreatedAtAction(nameof(GetUserById), new { id }, RentalSpace);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpDelete("RentalSpace/Delete")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var success = await _RentalSpaceService.DeleteAsync(id);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPut("RentalSpace/Update")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] RentalSpace? RentalSpace)
        {
            try
            {
                if (id != RentalSpace.id)
                    return BadRequest();

                var success = await _RentalSpaceService.UpdateAsync(RentalSpace);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpGet("RentalSpace/Search")]
        public async Task<IActionResult> Search(
                    [FromQuery] string? keyword = null,
                    [FromQuery] int pageNumber = 1,
                    [FromQuery] int pageSize = 20)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Invalid pagination parameters.");

            var result = await _RentalSpaceService.GetAsync(keyword, pageNumber, pageSize);
            var response = new
            {
                Data = result.RentalSpaces,
                TotalRecords = result.TotalRecords,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(result.TotalRecords / (double)pageSize)
            };

            return Ok(response);
        }

        [HttpGet("RentalSpace/GetById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _RentalSpaceService.GetByIdAsync(id);

            if (user == null)
                return NotFound($"RentalSpace with ID {id} not found.");

            return Ok(user);
        }


    }
}
