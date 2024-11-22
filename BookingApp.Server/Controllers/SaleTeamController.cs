using BookingApp.Server.Models;
using BookingApp.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
namespace BookingApp.Server.Controllers
{
    public class SaleTeamController : ControllerBase
    {
        private readonly SaleTeamService _saleTeamService;

        public SaleTeamController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DatabaseConfig");
            _saleTeamService = new SaleTeamService(connectionString);
        }


        [HttpPost("SaleTeam/Create")]
        public async Task<IActionResult> CreateUser([FromBody] SaleTeam? saleTeam)
        {
            try
            {
                if (saleTeam == null)
                    return BadRequest();

                var id = await _saleTeamService.AddAsync(saleTeam);
                return CreatedAtAction(nameof(GetUserById), new { id }, saleTeam);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpDelete("SaleTeam/Delete")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var success = await _saleTeamService.DeleteAsync(id);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPut("SaleTeam/Update")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] SaleTeam? saleTeam)
        {
            try
            {
                if (id != saleTeam.id)
                    return BadRequest();

                var success = await _saleTeamService.UpdateAsync(saleTeam);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpGet("SaleTeam/Search")]
        public async Task<IActionResult> Search(
                    [FromQuery] string? keyword = null,
                    [FromQuery] int pageNumber = 1,
                    [FromQuery] int pageSize = 20)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Invalid pagination parameters.");

            var result = await _saleTeamService.GetAsync(keyword, pageNumber, pageSize);
            var response = new
            {
                Data = result.SaleTeams,
                TotalRecords = result.TotalRecords,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(result.TotalRecords / (double)pageSize)
            };

            return Ok(response);
        }

        [HttpGet("SaleTeam/GetById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _saleTeamService.GetByIdAsync(id);

            if (user == null)
                return NotFound($"Unit with ID {id} not found.");

            return Ok(user);
        }


    }
}
