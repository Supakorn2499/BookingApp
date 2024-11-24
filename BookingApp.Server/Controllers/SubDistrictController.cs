using BookingApp.Server.Models;
using BookingApp.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.Server.Controllers
{
    public class SubDistrictController : ControllerBase
    {
        private readonly SubDistrictService _districtService;

        public SubDistrictController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DatabaseConfig");
            _districtService = new SubDistrictService(connectionString);
        }


        [HttpPost("SubDistrict/Create")]
        public async Task<IActionResult> CreateUser([FromBody] SubDistrict? district)
        {
            try
            {
                if (district == null)
                    return BadRequest();

                var id = await _districtService.AddAsync(district);
                return CreatedAtAction(nameof(GetUserById), new { id }, district);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpDelete("SubDistrict/Delete")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var success = await _districtService.DeleteAsync(id);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPut("SubDistrict/Update")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] SubDistrict? district)
        {
            try
            {
                if (id != district.id)
                    return BadRequest();

                var success = await _districtService.UpdateAsync(district);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpGet("SubDistrict/Search")]
        public async Task<IActionResult> Search(
                    [FromQuery] string provinceCode = "10",
                    [FromQuery] string districtCode = "1001",
                    [FromQuery] string? keyword = null,
                    [FromQuery] int pageNumber = 1,
                    [FromQuery] int pageSize = 20)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Invalid pagination parameters.");

            var result = await _districtService.GetAsync(provinceCode, districtCode, keyword, pageNumber, pageSize);
            var response = new
            {
                Data = result.Districts,
                TotalRecords = result.TotalRecords,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(result.TotalRecords / (double)pageSize)
            };

            return Ok(response);
        }

        [HttpGet("SubDistrict/GetById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _districtService.GetByIdAsync(id);

            if (user == null)
                return NotFound($"SubDistrict with ID {id} not found.");

            return Ok(user);
        }


    }
}
