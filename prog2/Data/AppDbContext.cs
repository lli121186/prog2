using System;
using Microsoft.EntityFrameworkCore;
using prog2.Models;

namespace prog2.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Adresse> Adressen { get; set; }
        public DbSet<Ortschaft> Ortschaften { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=adressen.db");
        }

        internal async Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}

