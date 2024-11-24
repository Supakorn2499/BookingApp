using BookingApp.Server.Models;

namespace BookingApp.Server.Services
{
    public class DistrictService
    {
        private readonly DisrtictRepository _repository;

        public DistrictService(string connectionString)
        {
            _repository = new DisrtictRepository(connectionString);
        }

        public Task<int> AddAsync(District district) => _repository.AddAsync(district);
        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
        public Task<bool> UpdateAsync(District district) => _repository.UpdateAsync(district);
        public async Task<(IEnumerable<District> Districts, int TotalRecords)> GetAsync(string provinceCode, string? keyword, int pageNumber, int pageSize)
        {
            return await _repository.GetAsync(provinceCode, keyword, pageNumber, pageSize);
        }
        public async Task<District?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
