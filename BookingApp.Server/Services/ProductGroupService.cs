using BookingApp.Server.Models;

namespace BookingApp.Server.Services
{
    public class ProductGroupService
    {
        private readonly ProductGroupRepository _repository;

        public ProductGroupService(string connectionString)
        {
            _repository = new ProductGroupRepository(connectionString);
        }

        public Task<int> AddAsync(ProductGroup productGroup) => _repository.AddAsync(productGroup);
        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
        public Task<bool> UpdateAsync(ProductGroup productGroup) => _repository.UpdateAsync(productGroup);
        public async Task<(IEnumerable<ProductGroup> ProductGroups, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            return await _repository.GetAsync(keyword, pageNumber, pageSize);
        }
        public async Task<ProductGroup?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
