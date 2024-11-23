using BookingApp.Server.Models;

namespace BookingApp.Server.Services
{
    public class DepartmentService
    {
        private readonly DepartmentRepository _repository;

        public DepartmentService(string connectionString)
        {
            _repository = new DepartmentRepository(connectionString);
        }

        public Task<int> AddAsync(Department department) => _repository.AddAsync(department);
        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
        public Task<bool> UpdateAsync(Department department) => _repository.UpdateAsync(department);
        public async Task<(IEnumerable<Department> Departments, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            return await _repository.GetAsync(keyword, pageNumber, pageSize);
        }
        public async Task<Department?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
