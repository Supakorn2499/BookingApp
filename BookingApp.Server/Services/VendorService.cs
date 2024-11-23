using BookingApp.Server.Models;

namespace BookingApp.Server.Services
{
    public class VendorService
    {
        private readonly VendorRepository _repository;

        public VendorService(string connectionString)
        {
            _repository = new VendorRepository(connectionString);
        }

        public Task<int> AddAsync(Vendor vendor) => _repository.AddAsync(vendor);
        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
        public Task<bool> UpdateAsync(Vendor vendor) => _repository.UpdateAsync(vendor);
        public async Task<(IEnumerable<Vendor> Vendors, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            return await _repository.GetAsync(keyword, pageNumber, pageSize);
        }
        public async Task<Vendor?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
