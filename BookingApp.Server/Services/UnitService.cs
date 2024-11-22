using BookingApp.Server.Models;

namespace BookingApp.Server.Services
{
    public class UnitService
    {
        private readonly UnitRepository _repository;

        public UnitService(string connectionString)
        {
            _repository = new UnitRepository(connectionString);
        }

        public Task<int> AddAsync(Unit unit) => _repository.AddAsync(unit);
        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
        public Task<bool> UpdateAsync(Unit unit) => _repository.UpdateAsync(unit);
        public async Task<(IEnumerable<Unit> Units, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            return await _repository.GetAsync(keyword, pageNumber, pageSize);
        }
        public async Task<Unit?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
