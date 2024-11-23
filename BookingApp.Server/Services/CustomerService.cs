using BookingApp.Server.Models;

namespace BookingApp.Server.Services
{
    public class CustomerService
    {
        private readonly CustomerRepository _repository;

        public CustomerService(string connectionString)
        {
            _repository = new CustomerRepository(connectionString);
        }

        public Task<int> AddAsync(Customer customer) => _repository.AddAsync(customer);
        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
        public Task<bool> UpdateAsync(Customer customer) => _repository.UpdateAsync(customer);
        public async Task<(IEnumerable<Customer> Customers, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            return await _repository.GetAsync(keyword, pageNumber, pageSize);
        }
        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
