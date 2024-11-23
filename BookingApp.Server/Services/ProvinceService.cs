using BookingApp.Server.Models;

namespace BookingApp.Server.Services
{
    public class ProvinceService
    {
        private readonly ProvinceRepository _repository;

        public ProvinceService(string connectionString)
        {
            _repository = new ProvinceRepository(connectionString);
        }

        public Task<int> AddAsync(Province province) => _repository.AddAsync(province);
        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
        public Task<bool> UpdateAsync(Province province) => _repository.UpdateAsync(province);
        public async Task<(IEnumerable<Province> Provinces, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            return await _repository.GetAsync(keyword, pageNumber, pageSize);
        }
        public async Task<Province?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
