using Microsoft.EntityFrameworkCore;
using prog2.Data;
using prog2.Models;
using System;
namespace prog2.Repositories
{
    public class AdresseRepository
    {
        private readonly AppDbContext _context;

        public AdresseRepository(AppDbContext context) { _context = context; }
        public async Task<List<Adresse>> GetAllAsync()
        {
            return await _context.Adressen.Include(a => a.Ortschaft).ToListAsync();
        }

        public async Task AddAsync(Adresse adresse)
        {
            _context.Adressen.Add(adresse);
            await _context.SaveChangesAsync();
        }
    }
}

