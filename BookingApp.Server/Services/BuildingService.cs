using BookingApp.Server.Models;

namespace BookingApp.Server.Services
{
    public class BuildingService
    {
        private readonly BuildingRepository _repository;

        public BuildingService(string connectionString)
        {
            _repository = new BuildingRepository(connectionString);
        }

        public Task<int> AddAsync(Building Building) => _repository.AddAsync(Building);
        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
        public Task<bool> UpdateAsync(Building Building) => _repository.UpdateAsync(Building);
        public async Task<(IEnumerable<Building> Buildings, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            return await _repository.GetAsync(keyword, pageNumber, pageSize);
        }
        public async Task<Building?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
