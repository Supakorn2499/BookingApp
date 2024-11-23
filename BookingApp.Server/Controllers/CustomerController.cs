using BookingApp.Server.Models;
using BookingApp.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.Server.Controllers
{
    public class CustomerController : ControllerBase
    {
        private readonly CustomerService _customerService;

        public CustomerController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DatabaseConfig");
            _customerService = new CustomerService(connectionString);
        }


        [HttpPost("Customer/Create")]
        public async Task<IActionResult> CreateUser([FromBody] Customer? customer)
        {
            try
            {
                if (customer == null)
                    return BadRequest();

                var id = await _customerService.AddAsync(customer);
                return CreatedAtAction(nameof(GetUserById), new { id }, customer);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpDelete("Customer/Delete")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var success = await _customerService.DeleteAsync(id);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPut("Customer/Update")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] Customer? customer)
        {
            try
            {
                if (id != customer.id)
                    return BadRequest();

                var success = await _customerService.UpdateAsync(customer);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpGet("Customer/Search")]
        public async Task<IActionResult> Search(
                    [FromQuery] string? keyword = null,
                    [FromQuery] int pageNumber = 1,
                    [FromQuery] int pageSize = 20)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Invalid pagination parameters.");

            var result = await _customerService.GetAsync(keyword, pageNumber, pageSize);
            var response = new
            {
                Data = result.Customers,
                TotalRecords = result.TotalRecords,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(result.TotalRecords / (double)pageSize)
            };

            return Ok(response);
        }

        [HttpGet("Customer/GetById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _customerService.GetByIdAsync(id);

            if (user == null)
                return NotFound($"Customer with ID {id} not found.");

            return Ok(user);
        }


    }
}
