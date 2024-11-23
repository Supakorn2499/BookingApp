using BookingApp.Server.Models;

namespace BookingApp.Server.Services
{
    public class BankBranchService
    {
        private readonly BankBranchRepository _repository;

        public BankBranchService(string connectionString)
        {
            _repository = new BankBranchRepository(connectionString);
        }

        public Task<int> AddAsync(BankBranch bank) => _repository.AddAsync(bank);
        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
        public Task<bool> UpdateAsync(BankBranch bank) => _repository.UpdateAsync(bank);
        public async Task<(IEnumerable<BankBranch> BankBranchs, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            return await _repository.GetAsync(keyword, pageNumber, pageSize);
        }
        public async Task<BankBranch?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
