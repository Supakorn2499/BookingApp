using BookingApp.Server.Models;

namespace BookingApp.Server.Services
{
    public class RentalSpaceService
    {
        private readonly RentalSpaceRepository _repository;

        public RentalSpaceService(string connectionString)
        {
            _repository = new RentalSpaceRepository(connectionString);
        }

        public Task<int> AddAsync(RentalSpace RentalSpace) => _repository.AddAsync(RentalSpace);
        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
        public Task<bool> UpdateAsync(RentalSpace RentalSpace) => _repository.UpdateAsync(RentalSpace);
        public async Task<(IEnumerable<RentalSpace> RentalSpaces, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            return await _repository.GetAsync(keyword, pageNumber, pageSize);
        }
        public async Task<RentalSpace?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}