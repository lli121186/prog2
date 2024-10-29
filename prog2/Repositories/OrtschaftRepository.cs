using System;
namespace prog2.Repositories
{
    public class OrtschaftRepository
    {
        private readonly AppDbContext _context;

        public OrtschaftRepository(AppDbContext context) { _context = context; }
        public async Task<List<Ortschaft>> GetAllAsync()
        {
            return await _context.Ortschaften.ToListAsync();
        }

        public async Task AddAsync(Ortschaft ortschaft)
        {
            _context.Ortschaften.Add(ortschaft);
            await _context.SaveChangesAsync();
        }
    }
}

