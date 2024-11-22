using BookingApp.Server.Models;
using BookingApp.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;

namespace BookingApp.Server.Controllers
{
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DatabaseConfig");
            _productService = new ProductService(connectionString);
        }

        [HttpPost("Product/Create")]
        public async Task<IActionResult> CreateProduct([FromBody] Product? product)
        {
            try
            {
                var id = await _productService.AddProductAsync(product);
                return CreatedAtAction(nameof(GetProductById), new { id }, product);
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
           
        }

        [HttpDelete("Product/Delete")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var success = await _productService.DeleteProductAsync(id);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPut("Product/Update")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product? product)
        {
            try
            {
                if (id != product.id)
                    return BadRequest();

                var success = await _productService.UpdateProductAsync(product);
                return success ? StatusCode(200, "success") : NotFound();
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpGet("Product/Search")]
        public async Task<IActionResult> Search(
                    [FromQuery] string? keyword = null,
                    [FromQuery] int pageNumber = 1,
                    [FromQuery] int pageSize = 20)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Invalid pagination parameters.");

            var result = await _productService.GetProductsAsync(keyword, pageNumber, pageSize);
            var response = new
            {
                Data = result.Products,
                TotalRecords = result.TotalRecords,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(result.TotalRecords / (double)pageSize)
            };

            return Ok(response);
        }

        [HttpGet("Product/GetById")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
                return NotFound($"Product with ID {id} not found.");

            return Ok(product);
        }


    }


}
