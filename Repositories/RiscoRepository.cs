using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Sessions_app.Data;
using Sessions_app.Models;

namespace Sessions_app.Repositories
{
    public class RiscoRepository : IRiscoRepository
    {
        private readonly DataContext _context;

        public RiscoRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Risco> GetByIdAsync(int id)
        {
            return await _context.Riscos.FindAsync(id);
        }

        public async Task<IEnumerable<Risco>> GetAllAsync()
        {
            return await _context.Riscos.ToListAsync();
        }

        public async Task AddAsync(Risco entity)
        {
            await _context.Riscos.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Risco entity)
        {
            _context.Riscos.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var risco = await GetByIdAsync(id);
            if (risco != null)
            {
                _context.Riscos.Remove(risco);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Riscos.AnyAsync(r => r.Id == id);
        }
    }
}
