using BookingApp.Server.Models;
using BookingApp.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;

namespace BookingApp.Server.Controllers
{
    public class SalemanController : ControllerBase
    {
        private readonly SalemanService _salemanService;

        public SalemanController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DatabaseConfig");
            _salemanService = new SalemanService(connectionString);
        }


        [HttpPost("Saleman/Create")]
        public async Task<IActionResult> CreateUser([FromBody] Saleman? saleman)
        {
            try
            {
                if (saleman == null)
                    return BadRequest();

                var id = await _salemanService.AddAsync(saleman);
                return CreatedAtAction(nameof(GetUserById), new { id }, saleman);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpDelete("Saleman/Delete")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var success = await _salemanService.DeleteAsync(id);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPut("Saleman/Update")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] Saleman? saleman)
        {
            try
            {
                if (id != saleman.id)
                    return BadRequest();

                var success = await _salemanService.UpdateAsync(saleman);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpGet("Saleman/Search")]
        public async Task<IActionResult> Search(
                    [FromQuery] string? keyword = null,
                    [FromQuery] int pageNumber = 1,
                    [FromQuery] int pageSize = 20)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Invalid pagination parameters.");

            var result = await _salemanService.GetAsync(keyword, pageNumber, pageSize);
            var response = new
            {
                Data = result.Salemans,
                TotalRecords = result.TotalRecords,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(result.TotalRecords / (double)pageSize)
            };

            return Ok(response);
        }

        [HttpGet("Saleman/GetById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _salemanService.GetByIdAsync(id);

            if (user == null)
                return NotFound($"Unit with ID {id} not found.");

            return Ok(user);
        }


    }
}
