using Sessions_app.Models;
using Sessions_app.Repositories;

namespace Sessions_app.Service
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioService(
            IUsuarioRepository repository)
        {
            _repository = repository;
        }

        public async Task<Usuario> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task AddAsync(Usuario entity)
        {
            await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(Usuario entity)
        {
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
