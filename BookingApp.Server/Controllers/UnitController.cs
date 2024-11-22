using BookingApp.Server.Models;
using BookingApp.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;

namespace BookingApp.Server.Controllers
{
    public class UnitController : ControllerBase
    {
        private readonly UnitService _unitService;

        public UnitController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DatabaseConfig");
            _unitService = new UnitService(connectionString);
        }


        [HttpPost("Unit/Create")]
        public async Task<IActionResult> CreateUser([FromBody] Unit? unit)
        {
            try
            {
                if (unit == null)
                    return BadRequest();

                var id = await _unitService.AddAsync(unit);
                return CreatedAtAction(nameof(GetUserById), new { id }, unit);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpDelete("Unit/Delete")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var success = await _unitService.DeleteAsync(id);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPut("Unit/Update")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] Unit? unit)
        {
            try
            {
                if (id != unit.id)
                    return BadRequest();

                var success = await _unitService.UpdateAsync(unit);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpGet("Unit/Search")]
        public async Task<IActionResult> Search(
                    [FromQuery] string? keyword = null,
                    [FromQuery] int pageNumber = 1,
                    [FromQuery] int pageSize = 20)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Invalid pagination parameters.");

            var result = await _unitService.GetAsync(keyword, pageNumber, pageSize);
            var response = new
            {
                Data = result.Units,
                TotalRecords = result.TotalRecords,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(result.TotalRecords / (double)pageSize)
            };

            return Ok(response);
        }

        [HttpGet("Unit/GetById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _unitService.GetByIdAsync(id);

            if (user == null)
                return NotFound($"Unit with ID {id} not found.");

            return Ok(user);
        }


    }
}
