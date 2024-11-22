using BookingApp.Server.Models;

namespace BookingApp.Server.Services
{
    public class ProductService
    {
        private readonly ProductRepository _repository;

        public ProductService(string connectionString)
        {
            _repository = new ProductRepository(connectionString);
        }

        public Task<int> AddProductAsync(Product product) => _repository.AddProductAsync(product);

        public Task<bool> DeleteProductAsync(int id) => _repository.DeleteProductAsync(id);

        public Task<bool> UpdateProductAsync(Product product) => _repository.UpdateProductAsync(product);

        public async Task<(IEnumerable<Product> Products, int TotalRecords)> GetProductsAsync(string? keyword, int pageNumber, int pageSize)
        {
            return await _repository.GetProductsAsync(keyword, pageNumber, pageSize);
        }
        
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _repository.GetProductByIdAsync(id);
        }


    }

}
