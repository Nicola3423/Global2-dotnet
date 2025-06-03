using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Sessions_app.Models;

namespace Sessions_app.Repositories
{
    public interface IRiscoRepository 
    {
        Task<Risco> GetByIdAsync(int id);
        Task<IEnumerable<Risco>> GetAllAsync();
        Task AddAsync(Risco risco);
        Task UpdateAsync(Risco risco);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
    
}
