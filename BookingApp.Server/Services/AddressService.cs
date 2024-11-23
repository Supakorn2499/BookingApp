using BookingApp.Server.Models;

namespace BookingApp.Server.Services
{
    public class AddressService
    {
        private readonly AddressRepository _repository;

        public AddressService(string connectionString)
        {
            _repository = new AddressRepository(connectionString);
        }

        public Task<int> AddAsync(Address address) => _repository.AddAsync(address);
        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
        public Task<bool> UpdateAsync(Address address) => _repository.UpdateAsync(address);
        public async Task<Address?> GetByIdAsync(int addresstype, int refid)
        {
            return await _repository.GetByIdAsync(addresstype, refid);
        }
    }
}
