using BookingApp.Server.Models;

namespace BookingApp.Server.Services
{
    public class FloorService
    {
        private readonly FloorRepository _repository;

        public FloorService(string connectionString)
        {
            _repository = new FloorRepository(connectionString);
        }

        public Task<int> AddAsync(Floor Floor) => _repository.AddAsync(Floor);
        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
        public Task<bool> UpdateAsync(Floor Floor) => _repository.UpdateAsync(Floor);
        public async Task<(IEnumerable<Floor> Floors, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            return await _repository.GetAsync(keyword, pageNumber, pageSize);
        }
        public async Task<Floor?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}