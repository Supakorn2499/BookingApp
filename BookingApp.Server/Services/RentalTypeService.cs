using BookingApp.Server.Models;

namespace BookingApp.Server.Services
{
    public class RentalTypeService
    {
        private readonly RentalTypeRepository _repository;

        public RentalTypeService(string connectionString)
        {
            _repository = new RentalTypeRepository(connectionString);
        }

        public Task<int> AddAsync(RentalType RentalType) => _repository.AddAsync(RentalType);
        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
        public Task<bool> UpdateAsync(RentalType RentalType) => _repository.UpdateAsync(RentalType);
        public async Task<(IEnumerable<RentalType> RentalTypes, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            return await _repository.GetAsync(keyword, pageNumber, pageSize);
        }
        public async Task<RentalType?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}