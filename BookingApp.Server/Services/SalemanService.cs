using BookingApp.Server.Models;

namespace BookingApp.Server.Services
{
    public class SalemanService
    {
        private readonly SalemanRepository _repository;

        public SalemanService(string connectionString)
        {
            _repository = new SalemanRepository(connectionString);
        }

        public Task<int> AddAsync(Saleman saleman) => _repository.AddAsync(saleman);
        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
        public Task<bool> UpdateAsync(Saleman saleman) => _repository.UpdateAsync(saleman);
        public async Task<(IEnumerable<Saleman> Salemans, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            return await _repository.GetAsync(keyword, pageNumber, pageSize);
        }
        public async Task<Saleman?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
