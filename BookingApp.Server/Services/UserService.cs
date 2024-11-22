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

        public Task<int> AddUserAsync(User user) => _repository.AddUserAsync(user);
        public Task<bool> DeleteUserAsync(int id) => _repository.DeleteUserAsync(id);
        public Task<bool> UpdateUserAsync(User user) => _repository.UpdateUserAsync(user);
        public async Task<(IEnumerable<User> Users, int TotalRecords)> GetUsersAsync(string? keyword, int pageNumber, int pageSize)
        {
            return await _repository.GetUsersAsync(keyword, pageNumber, pageSize);
        }
        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _repository.GetUserByIdAsync(id);
        }
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _repository.GetUserByUsernameAsync(username);
        }
        public Task<bool> ChangePasswordAsync(User user) => _repository.ChangePasswordAsync(user);
        public async Task<User?> LoginAsync(string username , string password )
        {
            return await _repository.LoginAsync(username, password);
        }

    }
}
