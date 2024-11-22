using BookingApp.Server.Models;
using BookingApp.Server.Services;
using Microsoft.AspNetCore.Mvc;


namespace BookingApp.Server.Controllers
{
    public class ProductGroupController : ControllerBase
    {
        private readonly ProductGroupService _prodGroupService;

        public ProductGroupController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DatabaseConfig");
            _prodGroupService = new ProductGroupService(connectionString);
        }


        [HttpPost("ProdGroup/Create")]
        public async Task<IActionResult> CreateUser([FromBody] ProductGroup? prodGroup)
        {
            try
            {
                if (prodGroup == null)
                    return BadRequest();

                var id = await _prodGroupService.AddAsync(prodGroup);
                return CreatedAtAction(nameof(GetUserById), new { id }, prodGroup);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpDelete("ProdGroup/Delete")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var success = await _prodGroupService.DeleteAsync(id);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPut("ProdGroup/Update")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] ProductGroup? prodGroup)
        {
            try
            {
                if (id != prodGroup.id)
                    return BadRequest();

                var success = await _prodGroupService.UpdateAsync(prodGroup);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpGet("ProdGroup/Search")]
        public async Task<IActionResult> Search(
                    [FromQuery] string? keyword = null,
                    [FromQuery] int pageNumber = 1,
                    [FromQuery] int pageSize = 20)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Invalid pagination parameters.");

            var result = await _prodGroupService.GetAsync(keyword, pageNumber, pageSize);
            var response = new
            {
                Data = result.ProductGroups,
                TotalRecords = result.TotalRecords,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(result.TotalRecords / (double)pageSize)
            };

            return Ok(response);
        }

        [HttpGet("ProdGroup/GetById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _prodGroupService.GetByIdAsync(id);

            if (user == null)
                return NotFound($"ProductGroup with ID {id} not found.");

            return Ok(user);
        }


    }
}
