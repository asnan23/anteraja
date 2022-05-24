using AnterAjaTest.API.Extensions;
using AnterAjaTest.API.Models;
using AnterAjaTest.API.Repository;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnterAjaTest.API.Services
{
    public class MasterPriceService : IMasterPriceService
    {
        private readonly IRepository<MasterPrice> _masterPrice;
        private readonly IDistributedCache _cache;
        public MasterPriceService(IRepository<MasterPrice> masterPrice, IDistributedCache cache)
        {
            _masterPrice = masterPrice;
            _cache = cache;
        }
        public async Task<MasterPrice> AddMasterPrice(MasterPrice masterPrice)
        {
            return await _masterPrice.CreateAsync(masterPrice);
        }

        public async Task<bool> UpdateMasterPrice(int id, MasterPrice masterPrice)
        {
            var data = await _masterPrice.GetByIdAsync(id);

            if (data != null)
            {
                data.origin_code = masterPrice.origin_code;
                data.destination_code = masterPrice.destination_code;
                data.price = masterPrice.price;
                data.product = masterPrice.product;

                await _masterPrice.UpdateAsync(data);
                return true;
            }
            else
                return false;
        }

        public async Task<bool> DeleteMasterPrice(int id)
        {
            await _masterPrice.DeleteAsync(id);
            return true;
        }

        public async Task<List<MasterPrice>> GetAllMasterPrices()
        {
            
            return await _masterPrice.GetAllAsync();
        }

        public async Task<MasterPrice> GetMasterPrice(int id)
        {
            return await _masterPrice.GetByIdAsync(id);
        }
    }
}
