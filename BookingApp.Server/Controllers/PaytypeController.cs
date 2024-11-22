using BookingApp.Server.Models;
using BookingApp.Server.Services;
using Microsoft.AspNetCore.Mvc;


namespace BookingApp.Server.Controllers
{
    public class PaytypeController : ControllerBase
    {
        private readonly PaytypeService _paytypeService;

        public PaytypeController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DatabaseConfig");
            _paytypeService = new PaytypeService(connectionString);
        }


        [HttpPost("Paytype/Create")]
        public async Task<IActionResult> CreateUser([FromBody] Paytype? paytype)
        {
            try
            {
                if (paytype == null)
                    return BadRequest();

                var id = await _paytypeService.AddAsync(paytype);
                return CreatedAtAction(nameof(GetUserById), new { id }, paytype);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpDelete("Paytype/Delete")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var success = await _paytypeService.DeleteAsync(id);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPut("Paytype/Update")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] Paytype? paytype)
        {
            try
            {
                if (id != paytype.id)
                    return BadRequest();

                var success = await _paytypeService.UpdateAsync(paytype);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpGet("Paytype/Search")]
        public async Task<IActionResult> Search(
                    [FromQuery] string? keyword = null,
                    [FromQuery] int pageNumber = 1,
                    [FromQuery] int pageSize = 20)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Invalid pagination parameters.");

            var result = await _paytypeService.GetAsync(keyword, pageNumber, pageSize);
            var response = new
            {
                Data = result.Paytypes,
                TotalRecords = result.TotalRecords,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(result.TotalRecords / (double)pageSize)
            };

            return Ok(response);
        }

        [HttpGet("Paytype/GetById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _paytypeService.GetByIdAsync(id);

            if (user == null)
                return NotFound($"Paytype with ID {id} not found.");

            return Ok(user);
        }


    }
}
