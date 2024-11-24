using BookingApp.Server.Models;

namespace BookingApp.Server.Services
{
    public class SubDistrictService
    {
        private readonly SubDistrictRepository _repository;

        public SubDistrictService(string connectionString)
        {
            _repository = new SubDistrictRepository(connectionString);
        }

        public Task<int> AddAsync(SubDistrict district) => _repository.AddAsync(district);
        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
        public Task<bool> UpdateAsync(SubDistrict district) => _repository.UpdateAsync(district);
        public async Task<(IEnumerable<SubDistrict> Districts, int TotalRecords)> GetAsync(string provinceCode,string districtCode, string? keyword, int pageNumber, int pageSize)
        {
            return await _repository.GetAsync(provinceCode, districtCode, keyword, pageNumber, pageSize);
        }
        public async Task<SubDistrict?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
