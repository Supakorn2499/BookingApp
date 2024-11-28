using BookingApp.Server.Models;

namespace BookingApp.Server.Services
{
    public class ProductTypeService
    {
        private readonly ProductTypeRepository _repository;

        public ProductTypeService(string connectionString)
        {
            _repository = new ProductTypeRepository(connectionString);
        }

        public Task<int> AddAsync(ProductType productType) => _repository.AddAsync(productType);
        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
        public Task<bool> UpdateAsync(ProductType productType) => _repository.UpdateAsync(productType);
        public async Task<(IEnumerable<ProductType> productTypes, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            return await _repository.GetAsync(keyword, pageNumber, pageSize);
        }
        public async Task<ProductType?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
        public async Task<ProductType?> GetByCodeAsync(string code)
        {
            return await _repository.GetByCodeAsync(code);
        }
    }
}
