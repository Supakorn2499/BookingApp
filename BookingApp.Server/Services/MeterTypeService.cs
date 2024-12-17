using BookingApp.Server.Models;

namespace BookingApp.Server.Services
{
    public class MeterTypeService
    {
        private readonly MeterTypeRepository _repository;

        public MeterTypeService(string connectionString)
        {
            _repository = new MeterTypeRepository(connectionString);
        }

        public Task<int> AddAsync(MeterType MeterType) => _repository.AddAsync(MeterType);
        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
        public Task<bool> UpdateAsync(MeterType MeterType) => _repository.UpdateAsync(MeterType);
        public async Task<(IEnumerable<MeterType> MeterTypes, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            return await _repository.GetAsync(keyword, pageNumber, pageSize);
        }
        public async Task<MeterType?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
