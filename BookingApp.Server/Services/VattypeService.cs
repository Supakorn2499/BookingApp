using BookingApp.Server.Models;

namespace BookingApp.Server.Services
{
    public class VattypeService
    {
        private readonly VattypeRepository _repository;

        public VattypeService(string connectionString)
        {
            _repository = new VattypeRepository(connectionString);
        }

        public Task<int> AddAsync(Vattype vattype) => _repository.AddAsync(vattype);
        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
        public Task<bool> UpdateAsync(Vattype vattype) => _repository.UpdateAsync(vattype);
        public async Task<(IEnumerable<Vattype> Vattypes, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            return await _repository.GetAsync(keyword, pageNumber, pageSize);
        }
        public async Task<Vattype?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
