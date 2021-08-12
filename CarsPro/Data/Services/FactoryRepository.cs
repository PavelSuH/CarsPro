using CarsPro.Data.Contracts;
using CarsPro.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsPro.Data.Services
{
    public class FactoryRepository : IFactoryRepository
    {
        private ApplicationDbContext _db;
        public FactoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> Create(Factory entity)
        {
            await _db.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Factory entity)
        {
             _db.Remove(entity);
            return await Save();
        }

        public async Task<IList<Factory>> FindAll()
        {
            return await _db.Factories.ToListAsync();
        }

        public async Task<Factory> FindById(int id)
        {
            return await _db.Factories.FindAsync(id);
        }

        public async Task<bool> IsExists(int id)
        {
            return await _db.Factories.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Factory entity)
        {
            _db.Update(entity);
            return await Save();
        }

        

       
    }
}
