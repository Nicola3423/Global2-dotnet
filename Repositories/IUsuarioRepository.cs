using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Sessions_app.Models;

namespace Sessions_app.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario> GetByIdAsync(int id);
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task AddAsync(Usuario entity);
        Task UpdateAsync(Usuario entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
