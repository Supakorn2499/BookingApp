using BookingApp.Server.Models;
using BookingApp.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.Server.Controllers
{
    public class AddressController : ControllerBase
    {
        private readonly AddressService _addressService;

        public AddressController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DatabaseConfig");
            _addressService = new AddressService(connectionString);
        }


        [HttpPost("Address/Create")]
        public async Task<IActionResult> CreateUser([FromBody] Address? address)
        {
            try
            {
                if (address == null)
                    return BadRequest();

                var id = await _addressService.AddAsync(address);
                return CreatedAtAction(nameof(GetUserById), new { id }, address);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpDelete("Address/Delete")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var success = await _addressService.DeleteAsync(id);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPut("Address/Update")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] Address? address)
        {
            try
            {
                if (id != address.id)
                    return BadRequest();

                var success = await _addressService.UpdateAsync(address);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }


        [HttpGet("Address/GetById")]
        public async Task<IActionResult> GetUserById(int addresstype, int refid)
        {
            var user = await _addressService.GetByIdAsync(addresstype, refid);

            if (user == null)
                return NotFound($"Address with addresstype {addresstype} and refid {refid} not found.");

            return Ok(user);
        }


    }
}
