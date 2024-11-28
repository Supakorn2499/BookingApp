using BookingApp.Server.Models;
using BookingApp.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.Server.Controllers
{
    public class VattypeController : ControllerBase
    {
        private readonly VattypeService _vattypeService;

        public VattypeController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DatabaseConfig");
            _vattypeService = new VattypeService(connectionString);
        }


        [HttpPost("Vattype/Create")]
        public async Task<IActionResult> CreateUser([FromBody] Vattype? vattype)
        {
            try
            {
                if (vattype == null)
                    return BadRequest();

                var id = await _vattypeService.AddAsync(vattype);
                return CreatedAtAction(nameof(GetUserById), new { id }, vattype);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpDelete("Vattype/Delete")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var success = await _vattypeService.DeleteAsync(id);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPut("Vattype/Update")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] Vattype? vattype)
        {
            try
            {
                if (id != vattype.id)
                    return BadRequest();

                var success = await _vattypeService.UpdateAsync(vattype);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpGet("Vattype/Search")]
        public async Task<IActionResult> Search(
                    [FromQuery] string? keyword = null,
                    [FromQuery] int pageNumber = 1,
                    [FromQuery] int pageSize = 20)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Invalid pagination parameters.");

            var result = await _vattypeService.GetAsync(keyword, pageNumber, pageSize);
            var response = new
            {
                Data = result.Vattypes,
                TotalRecords = result.TotalRecords,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(result.TotalRecords / (double)pageSize)
            };

            return Ok(response);
        }

        [HttpGet("Vattype/GetById")]
        public async Task<IActionResult> GetUserById(int id = 1)
        {
            if(id == 0) { id = 1; };

            var user = await _vattypeService.GetByIdAsync(id);

            if (user == null)
                return NotFound($"Vattype with ID {id} not found.");

            return Ok(user);
        }


    }
}
