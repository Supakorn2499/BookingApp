using BookingApp.Server.Models;

namespace BookingApp.Server.Services
{
    public class SaleTeamService
    {
        private readonly SaleTeamRepository _repository;

        public SaleTeamService(string connectionString)
        {
            _repository = new SaleTeamRepository(connectionString);
        }

        public Task<int> AddAsync(SaleTeam saleTeam) => _repository.AddAsync(saleTeam);
        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
        public Task<bool> UpdateAsync(SaleTeam saleTeam) => _repository.UpdateAsync(saleTeam);
        public async Task<(IEnumerable<SaleTeam> SaleTeams, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            return await _repository.GetAsync(keyword, pageNumber, pageSize);
        }
        public async Task<SaleTeam?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
