using BookingApp.Server.Models;
using BookingApp.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;

namespace BookingApp.Server.Controllers
{
    public class VendorController : ControllerBase
    {
        private readonly VendorService _vendorService;

        public VendorController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DatabaseConfig");
            _vendorService = new VendorService(connectionString);
        }


        [HttpPost("Vendor/Create")]
        public async Task<IActionResult> CreateUser([FromBody] Vendor? vendor)
        {
            try
            {
                if (vendor == null)
                    return BadRequest();

                var id = await _vendorService.AddAsync(vendor);
                return CreatedAtAction(nameof(GetUserById), new { id }, vendor);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpDelete("Vendor/Delete")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var success = await _vendorService.DeleteAsync(id);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPut("Vendor/Update")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] Vendor? vendor)
        {
            try
            {
                if (id != vendor.id)
                    return BadRequest();

                var success = await _vendorService.UpdateAsync(vendor);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpGet("Vendor/Search")]
        public async Task<IActionResult> Search(
                    [FromQuery] string? keyword = null,
                    [FromQuery] int pageNumber = 1,
                    [FromQuery] int pageSize = 20)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Invalid pagination parameters.");

            var result = await _vendorService.GetAsync(keyword, pageNumber, pageSize);
            var response = new
            {
                Data = result.Vendors,
                TotalRecords = result.TotalRecords,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(result.TotalRecords / (double)pageSize)
            };

            return Ok(response);
        }

        [HttpGet("Vendor/GetById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _vendorService.GetByIdAsync(id);

            if (user == null)
                return NotFound($"Vendor with ID {id} not found.");

            return Ok(user);
        }


    }
}
