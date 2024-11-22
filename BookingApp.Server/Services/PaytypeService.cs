using BookingApp.Server.Models;

namespace BookingApp.Server.Services
{
    public class PaytypeService
    {
        private readonly PaytypeRepository _repository;

        public PaytypeService(string connectionString)
        {
            _repository = new PaytypeRepository(connectionString);
        }

        public Task<int> AddAsync(Paytype paytype) => _repository.AddAsync(paytype);
        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
        public Task<bool> UpdateAsync(Paytype paytype) => _repository.UpdateAsync(paytype);
        public async Task<(IEnumerable<Paytype> Paytypes, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            return await _repository.GetAsync(keyword, pageNumber, pageSize);
        }
        public async Task<Paytype?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
