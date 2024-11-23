using BookingApp.Server.Models;
using BookingApp.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.Server.Controllers
{
    public class CompanyController : ControllerBase
    {
        private readonly CompanyService _companyService;

        public CompanyController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DatabaseConfig");
            _companyService = new CompanyService(connectionString);
        }


        [HttpPost("Company/Create")]
        public async Task<IActionResult> CreateUser([FromBody] Company? company)
        {
            try
            {
                if (company == null)
                    return BadRequest();

                var id = await _companyService.AddAsync(company);
                return CreatedAtAction(nameof(GetUserById), new { id }, company);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpDelete("Company/Delete")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var success = await _companyService.DeleteAsync(id);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPut("Company/Update")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] Company? company)
        {
            try
            {
                if (id != company.id)
                    return BadRequest();

                var success = await _companyService.UpdateAsync(company);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpGet("Company/Search")]
        public async Task<IActionResult> Search(
                    [FromQuery] string? keyword = null,
                    [FromQuery] int pageNumber = 1,
                    [FromQuery] int pageSize = 20)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Invalid pagination parameters.");

            var result = await _companyService.GetAsync(keyword, pageNumber, pageSize);
            var response = new
            {
                Data = result.Companys,
                TotalRecords = result.TotalRecords,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(result.TotalRecords / (double)pageSize)
            };

            return Ok(response);
        }

        [HttpGet("Company/GetById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _companyService.GetByIdAsync(id);

            if (user == null)
                return NotFound($"Company with ID {id} not found.");

            return Ok(user);
        }


    }
}
