using BookingApp.Server.Models;
using BookingApp.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;

namespace BookingApp.Server.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DatabaseConfig");
            _userService = new UserService(connectionString);
        }


        [HttpPost("User/Create")]
        public async Task<IActionResult> CreateUser([FromBody] User? user)
        {
            try
            {
                if (user == null)
                    return BadRequest();

                var result = await _userService.GetByUsernameAsync(user.username);
                if (result != null)
                    return BadRequest("Username is already exit.");

                var id = await _userService.AddAsync(user);
                return CreatedAtAction(nameof(GetUserById), new { id }, user);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpDelete("User/Delete")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var success = await _userService.DeleteAsync(id);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPut("User/Update")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User? user)
        {
            try
            {
                if (id != user.id)
                    return BadRequest();

                var success = await _userService.UpdateAsync(user);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpGet("User/Search")]
        public async Task<IActionResult> Search(
                    [FromQuery] string? keyword = null,
                    [FromQuery] int pageNumber = 1,
                    [FromQuery] int pageSize = 20)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Invalid pagination parameters.");

            var result = await _userService.GetAsync(keyword, pageNumber, pageSize);
            var response = new
            {
                Data = result.Users,
                TotalRecords = result.TotalRecords,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(result.TotalRecords / (double)pageSize)
            };

            return Ok(response);
        }

        [HttpGet("User/GetById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
                return NotFound($"User with ID {id} not found.");

            return Ok(user);
        }


    }
}
