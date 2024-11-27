using BookingApp.Server.Models;
using BookingApp.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;

namespace BookingApp.Server.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly JwtService _jwtService;
        public AuthController(IConfiguration configuration, JwtService jwtService)
        {
            var connectionString = configuration.GetConnectionString("DatabaseConfig");
            _userService = new UserService(connectionString);
            _jwtService = jwtService;
        }

        [HttpPost("Auth/Login")]
        public async Task<IActionResult> Login([FromBody] AuthRequest? req)
        {
            try
            {
                if (req == null)
                    return BadRequest();

                var result = await _userService.LoginAsync(req.username, req.password);

                if (result == null)
                    return StatusCode(401, "Invalid username or password");//Unauthorized(new { message = "Invalid username or password" });

                var token = _jwtService.GenerateToken(req.username, "admin");
                return Ok(new AuthResponse { token = token, role = "admin" });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

    }
}
