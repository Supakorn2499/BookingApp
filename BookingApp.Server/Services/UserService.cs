using BookingApp.Server.Models;

namespace BookingApp.Server.Services
{
    public class UserService
    {
        private readonly UserRepository _repository;

        public UserService(string connectionString)
        {
            _repository = new UserRepository(connectionString);
        }

        public Task<int> AddAsync(User user) => _repository.AddAsync(user);
        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
        public Task<bool> UpdateAsync(User user) => _repository.UpdateAsync(user);
        public async Task<(IEnumerable<User> Users, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            return await _repository.GetAsync(keyword, pageNumber, pageSize);
        }
        public async Task<User?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _repository.GetByUsernameAsync(username);
        }
        public Task<bool> ChangePasswordAsync(User user) => _repository.ChangePasswordAsync(user);
        public async Task<User?> LoginAsync(string username , string password )
        {
            return await _repository.LoginAsync(username, password);
        }

    }
}
