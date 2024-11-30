using BookingApp.Server.Models;

namespace BookingApp.Server.Services
{
    public class ZoneService
    {
        private readonly ZoneRepository _repository;

        public ZoneService(string connectionString)
        {
            _repository = new ZoneRepository(connectionString);
        }

        public Task<int> AddAsync(Zone zone) => _repository.AddAsync(zone);
        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
        public Task<bool> UpdateAsync(Zone zone) => _repository.UpdateAsync(zone);
        public async Task<(IEnumerable<Zone> Zones, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            return await _repository.GetAsync(keyword, pageNumber, pageSize);
        }
        public async Task<Zone?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
