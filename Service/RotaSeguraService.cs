using Sessions_app.Models;
using Sessions_app.Repositories;

namespace Sessions_app.Service
{
    public class RotaSeguraService
    {
        private readonly IRotaSeguraRepository _repository;

        public RotaSeguraService(IRotaSeguraRepository repository)
        {
            _repository = repository;
        }

        public async Task<RotaSegura> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<RotaSegura>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task AddAsync(RotaSegura entity)
        {
            await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(RotaSegura entity)
        {
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
