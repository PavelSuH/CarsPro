using CarsPro.Data.Contracts;
using CarsPro.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CarsPro.Data.Services
{
    public class CarRepository : ICarRepository
    {
        private readonly ApplicationDbContext _db;
        public CarRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> Create(Car entity)
        {
            await _db.Cars.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Car entity)
        {
             _db.Cars.Remove(entity);
            return await Save();
        }

        public async Task<IList<Car>> FindAll()
        {
            var carlist = await _db.Cars.ToListAsync();
            return carlist;
        }

        public async Task<Car> FindById(int id)
        {
            var car = await _db.Cars.FindAsync(id);
            return car;
        }

        public async Task<bool> IsExists(int id)
        {
            return await _db.Cars.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Car entity)
        {
            _db.Cars.Update(entity);
            return await Save();
        }
    }
}
