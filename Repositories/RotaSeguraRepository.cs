using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Sessions_app.Data;
using Sessions_app.Models;

namespace Sessions_app.Repositories
{
    public class RotaSeguraRepository : IRotaSeguraRepository
    {
        private readonly DataContext _context;

        public RotaSeguraRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<RotaSegura> GetByIdAsync(int id)
        {
            return await _context.RotaSeguras.FindAsync(id);
        }

        public async Task<IEnumerable<RotaSegura>> GetAllAsync()
        {
            return await _context.RotaSeguras.ToListAsync();
        }

        public async Task AddAsync(RotaSegura entity)
        {
            await _context.RotaSeguras.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RotaSegura entity)
        {
            _context.RotaSeguras.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var rota = await GetByIdAsync(id);
            if (rota != null)
            {
                _context.RotaSeguras.Remove(rota);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.RotaSeguras.AnyAsync(r => r.Id == id);
        }


    }
}
