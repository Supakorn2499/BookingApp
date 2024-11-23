using BookingApp.Server.Models;
using BookingApp.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.Server.Controllers
{
    public class BankBranchController : ControllerBase
    {
        private readonly BankBranchService _bankService;

        public BankBranchController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DatabaseConfig");
            _bankService = new BankBranchService(connectionString);
        }


        [HttpPost("BankBranch/Create")]
        public async Task<IActionResult> CreateUser([FromBody] BankBranch? bank)
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

        [HttpDelete("BankBranch/Delete")]
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

        [HttpPut("BankBranch/Update")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] BankBranch? bank)
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

        [HttpGet("BankBranch/Search")]
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
                Data = result.BankBranchs,
                TotalRecords = result.TotalRecords,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(result.TotalRecords / (double)pageSize)
            };

            return Ok(response);
        }

        [HttpGet("BankBranch/GetById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _bankService.GetByIdAsync(id);

            if (user == null)
                return NotFound($"BankBranch with ID {id} not found.");

            return Ok(user);
        }


    }
}
