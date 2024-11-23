using BookingApp.Server.Models;
using BookingApp.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.Server.Controllers
{
    public class BankController : ControllerBase
    {
        private readonly BankService _bankService;

        public BankController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DatabaseConfig");
            _bankService = new BankService(connectionString);
        }


        [HttpPost("Bank/Create")]
        public async Task<IActionResult> CreateUser([FromBody] Bank? bank)
        {
            try
            {
                if (bank == null)
                    return BadRequest();

                var id = await _bankService.AddAsync(bank);
                return CreatedAtAction(nameof(GetUserById), new { id }, bank);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpDelete("Bank/Delete")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var success = await _bankService.DeleteAsync(id);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPut("Bank/Update")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] Bank? bank)
        {
            try
            {
                if (id != bank.id)
                    return BadRequest();

                var success = await _bankService.UpdateAsync(bank);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpGet("Bank/Search")]
        public async Task<IActionResult> Search(
                    [FromQuery] string? keyword = null,
                    [FromQuery] int pageNumber = 1,
                    [FromQuery] int pageSize = 20)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Invalid pagination parameters.");

            var result = await _bankService.GetAsync(keyword, pageNumber, pageSize);
            var response = new
            {
                Data = result.Banks,
                TotalRecords = result.TotalRecords,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(result.TotalRecords / (double)pageSize)
            };

            return Ok(response);
        }

        [HttpGet("Bank/GetById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _bankService.GetByIdAsync(id);

            if (user == null)
                return NotFound($"Bank with ID {id} not found.");

            return Ok(user);
        }


    }
}
