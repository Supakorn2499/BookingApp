using BookingApp.Server.Models;
using BookingApp.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.Server.Controllers
{
    public class DocTypeController : ControllerBase
    {
        private readonly DocTypeService _doctypeService;

        public DocTypeController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DatabaseConfig");
            _doctypeService = new DocTypeService(connectionString);
        }


        [HttpPost("DocType/Create")]
        public async Task<IActionResult> CreateUser([FromBody] DocType? doctype)
        {
            try
            {
                if (doctype == null)
                    return BadRequest();

                var id = await _doctypeService.AddAsync(doctype);
                return CreatedAtAction(nameof(GetUserById), new { id }, doctype);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpDelete("DocType/Delete")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var success = await _doctypeService.DeleteAsync(id);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPut("DocType/Update")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] DocType? doctype)
        {
            try
            {
                if (id != doctype.id)
                    return BadRequest();

                var success = await _doctypeService.UpdateAsync(doctype);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpGet("DocType/Search")]
        public async Task<IActionResult> Search(
                    [FromQuery] string? keyword = null,
                    [FromQuery] int pageNumber = 1,
                    [FromQuery] int pageSize = 20)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Invalid pagination parameters.");

            var result = await _doctypeService.GetAsync(keyword, pageNumber, pageSize);
            var response = new
            {
                Data = result.DocTypes,
                TotalRecords = result.TotalRecords,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(result.TotalRecords / (double)pageSize)
            };

            return Ok(response);
        }

        [HttpGet("DocType/GetById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _doctypeService.GetByIdAsync(id);

            if (user == null)
                return NotFound($"DocType with ID {id} not found.");

            return Ok(user);
        }


    }
}
