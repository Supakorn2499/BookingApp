using BookingApp.Server.Models;
using BookingApp.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.Server.Controllers
{
    public class ProvinceController : ControllerBase
    {
        private readonly ProvinceService _provinceService;

        public ProvinceController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DatabaseConfig");
            _provinceService = new ProvinceService(connectionString);
        }


        [HttpPost("Province/Create")]
        public async Task<IActionResult> CreateUser([FromBody] Province? province)
        {
            try
            {
                if (province == null)
                    return BadRequest();

                var id = await _provinceService.AddAsync(province);
                return CreatedAtAction(nameof(GetUserById), new { id }, province);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpDelete("Province/Delete")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var success = await _provinceService.DeleteAsync(id);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPut("Province/Update")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] Province? province)
        {
            try
            {
                if (id != province.id)
                    return BadRequest();

                var success = await _provinceService.UpdateAsync(province);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpGet("Province/Search")]
        public async Task<IActionResult> Search(
                    [FromQuery] string? keyword = null,
                    [FromQuery] int pageNumber = 1,
                    [FromQuery] int pageSize = 20)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Invalid pagination parameters.");

            var result = await _provinceService.GetAsync(keyword, pageNumber, pageSize);
            var response = new
            {
                Data = result.Provinces,
                TotalRecords = result.TotalRecords,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(result.TotalRecords / (double)pageSize)
            };

            return Ok(response);
        }

        [HttpGet("Province/GetById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _provinceService.GetByIdAsync(id);

            if (user == null)
                return NotFound($"Province with ID {id} not found.");

            return Ok(user);
        }


    }
}
