using BookingApp.Server.Models;

namespace BookingApp.Server.Services
{
    public class BankService
    {

        private readonly BankRepository _repository;

        public BankService(string connectionString)
        {
            _repository = new BankRepository(connectionString);
        }

        public Task<int> AddAsync(Bank bank) => _repository.AddAsync(bank);
        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
        public Task<bool> UpdateAsync(Bank bank) => _repository.UpdateAsync(bank);
        public async Task<(IEnumerable<Bank> Banks, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            return await _repository.GetAsync(keyword, pageNumber, pageSize);
        }
        public async Task<Bank?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
