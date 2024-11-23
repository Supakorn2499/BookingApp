using BookingApp.Server.Models;
using BookingApp.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.Server.Controllers
{
    public class DepartmentController : ControllerBase
    {
        private readonly DepartmentService _departmentService;

        public DepartmentController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DatabaseConfig");
            _departmentService = new DepartmentService(connectionString);
        }


        [HttpPost("Department/Create")]
        public async Task<IActionResult> CreateUser([FromBody] Department? department)
        {
            try
            {
                if (department == null)
                    return BadRequest();

                var id = await _departmentService.AddAsync(department);
                return CreatedAtAction(nameof(GetUserById), new { id }, department);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpDelete("Department/Delete")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var success = await _departmentService.DeleteAsync(id);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPut("Department/Update")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] Department? department)
        {
            try
            {
                if (id != department.id)
                    return BadRequest();

                var success = await _departmentService.UpdateAsync(department);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpGet("Department/Search")]
        public async Task<IActionResult> Search(
                    [FromQuery] string? keyword = null,
                    [FromQuery] int pageNumber = 1,
                    [FromQuery] int pageSize = 20)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Invalid pagination parameters.");

            var result = await _departmentService.GetAsync(keyword, pageNumber, pageSize);
            var response = new
            {
                Data = result.Departments,
                TotalRecords = result.TotalRecords,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(result.TotalRecords / (double)pageSize)
            };

            return Ok(response);
        }

        [HttpGet("Department/GetById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _departmentService.GetByIdAsync(id);

            if (user == null)
                return NotFound($"Department with ID {id} not found.");

            return Ok(user);
        }


    }
}
