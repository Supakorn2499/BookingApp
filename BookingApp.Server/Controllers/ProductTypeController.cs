using BookingApp.Server.Models;
using BookingApp.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.Server.Controllers
{
    public class ProductTypeController : ControllerBase
    {
        private readonly ProductTypeService _prodTypeService;

        public ProductTypeController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DatabaseConfig");
            _prodTypeService = new ProductTypeService(connectionString);
        }


        [HttpPost("ProdType/Create")]
        public async Task<IActionResult> CreateUser([FromBody] ProductType? prodType)
        {
            try
            {
                if (prodType == null)
                    return BadRequest();

                var id = await _prodTypeService.AddAsync(prodType);
                return CreatedAtAction(nameof(GetUserById), new { id }, prodType);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpDelete("ProdType/Delete")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var success = await _prodTypeService.DeleteAsync(id);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPut("ProdType/Update")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] ProductType? prodType)
        {
            try
            {
                if (id != prodType.id)
                    return BadRequest();

                var success = await _prodTypeService.UpdateAsync(prodType);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpGet("ProdType/Search")]
        public async Task<IActionResult> Search(
                    [FromQuery] string? keyword = null,
                    [FromQuery] int pageNumber = 1,
                    [FromQuery] int pageSize = 20)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Invalid pagination parameters.");

            var result = await _prodTypeService.GetAsync(keyword, pageNumber, pageSize);
            var response = new
            {
                Data = result.productTypes,
                TotalRecords = result.TotalRecords,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(result.TotalRecords / (double)pageSize)
            };

            return Ok(response);
        }

        [HttpGet("ProdType/GetById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _prodTypeService.GetByIdAsync(id);

            if (user == null)
                return NotFound($"ProductType with ID {id} not found.");

            return Ok(user);
        }
        [HttpGet("ProdType/GetByCode")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var user = await _prodTypeService.GetByCodeAsync(code);

            if (user == null)
                return NotFound($"ProductType with ID {code} not found.");

            return Ok(user);
        }

    }
}
