using AnterAjaTest.API.Models;
using AnterAjaTest.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnterAjaTest.API.Services
{
    public interface IMasterPriceService
    {
        Task<MasterPrice> AddMasterPrice(MasterPrice masterPrice);

        Task<bool> UpdateMasterPrice(int id, MasterPrice masterPrice);

        Task<bool> DeleteMasterPrice(int id);

        Task<List<MasterPrice>> GetAllMasterPrices();

        Task<MasterPrice> GetMasterPrice(int id);
    }
}
