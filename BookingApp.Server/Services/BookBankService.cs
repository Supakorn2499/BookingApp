using BookingApp.Server.Models;

namespace BookingApp.Server.Services
{
    public class BookBankService
    {
        private readonly BookBankRepository _repository;

        public BookBankService(string connectionString)
        {
            _repository = new BookBankRepository(connectionString);
        }

        public Task<int> AddAsync(BookBank bank) => _repository.AddAsync(bank);
        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
        public Task<bool> UpdateAsync(BookBank bank) => _repository.UpdateAsync(bank);
        public async Task<(IEnumerable<BookBank> BookBanks, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            return await _repository.GetAsync(keyword, pageNumber, pageSize);
        }
        public async Task<BookBank?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
