using AnterAjaTest.API.AppDbContext;
using AnterAjaTest.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnterAjaTest.API.Repository
{
    public class MasterPriceRepository : IRepository<MasterPrice>
    {
        ApplicationDbContext _dbContext;
        public MasterPriceRepository(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }
        public async Task<MasterPrice> CreateAsync(MasterPrice _object)
        {
            var obj = await _dbContext.MasterPrices.AddAsync(_object);
            _dbContext.SaveChanges();
            return obj.Entity;
        }

        public async Task UpdateAsync(MasterPrice _object)
        {
            _dbContext.MasterPrices.Update(_object);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<MasterPrice> ExistAsync(string origin, string dest)
        {
            return await _dbContext.MasterPrices.FirstOrDefaultAsync(t => t.origin_code == origin && t.destination_code == dest);            
        }

        public async Task<List<MasterPrice>> GetAllAsync()
        {
            return await _dbContext.MasterPrices.ToListAsync();
        }

        public async Task<MasterPrice> GetByIdAsync(int Id)
        {
            return await _dbContext.MasterPrices.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task DeleteAsync(int id)
        {
            var data = _dbContext.MasterPrices.FirstOrDefault(x => x.Id == id);
            _dbContext.Remove(data);
            await _dbContext.SaveChangesAsync();
        }
    }
}
