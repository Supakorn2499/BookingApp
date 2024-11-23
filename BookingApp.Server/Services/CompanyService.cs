using BookingApp.Server.Models;

namespace BookingApp.Server.Services
{
    public class CompanyService
    {
        private readonly CompanyRepository _repository;

        public CompanyService(string connectionString)
        {
            _repository = new CompanyRepository(connectionString);
        }

        public Task<int> AddAsync(Company company) => _repository.AddAsync(company);
        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
        public Task<bool> UpdateAsync(Company company) => _repository.UpdateAsync(company);
        public async Task<(IEnumerable<Company> Companys, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            return await _repository.GetAsync(keyword, pageNumber, pageSize);
        }
        public async Task<Company?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
