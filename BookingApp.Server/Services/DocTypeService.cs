using BookingApp.Server.Models;

namespace BookingApp.Server.Services
{
    public class DocTypeService
    {
        private readonly DocTypeRepository _repository;

        public DocTypeService(string connectionString)
        {
            _repository = new DocTypeRepository(connectionString);
        }

        public Task<int> AddAsync(DocType doctype) => _repository.AddAsync(doctype);
        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
        public Task<bool> UpdateAsync(DocType doctype) => _repository.UpdateAsync(doctype);
        public async Task<(IEnumerable<DocType> DocTypes, int TotalRecords)> GetAsync(string? keyword, int pageNumber, int pageSize)
        {
            return await _repository.GetAsync(keyword, pageNumber, pageSize);
        }
        public async Task<DocType?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
