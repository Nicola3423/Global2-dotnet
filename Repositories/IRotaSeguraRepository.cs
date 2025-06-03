using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Sessions_app.Models;

namespace Sessions_app.Repositories
{
    public interface IRotaSeguraRepository
    {
        Task<RotaSegura> GetByIdAsync(int id);
        Task<IEnumerable<RotaSegura>> GetAllAsync();
        Task AddAsync(RotaSegura rotaSegura);
        Task UpdateAsync(RotaSegura rotaSegura);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
