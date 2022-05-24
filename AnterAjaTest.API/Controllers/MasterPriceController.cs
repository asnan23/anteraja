﻿using AnterAjaTest.API.Extensions;
using AnterAjaTest.API.Models;
using AnterAjaTest.API.Services;
using AnterAjaTest.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnterAjaTest.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterPriceController : ControllerBase
    {
        private readonly IMasterPriceService _masterPriceService;
        protected ResponseDto response;
        private readonly IDistributedCache _cache;
        public MasterPriceController(IMasterPriceService masterPriceService, IDistributedCache cache)
        {
            _masterPriceService = masterPriceService;
            this.response = new ResponseDto();
            _cache = cache;
        }

        [HttpGet]
        public async Task<object> GetAll()
        {
            var result = new List<MasterPrice>();
            result = await _cache.GetRecordAsync<List<MasterPrice>>(RedisCacheKeys.MasterPrice);
            if(result is null)
            {
                result = await _masterPriceService.GetAllMasterPrices();
                if(result.Count > 0)
                {
                    await _cache.SetRecordAsync("MasterPrice", result);
                    response.DisplayMessage = "Data from Database";
                    response.Result = result;
                }
                else
                {
                    response.DisplayMessage = "No Data Found";
                    response.Result = result;
                }         
            }
            else
            {
                response.DisplayMessage = "Data from Redis";
                response.Result = result;
            }
            return response;
            
        }

        [HttpGet("{id}")]
        public async Task<object> Get(int id)
        {
            response.Result = await _masterPriceService.GetMasterPrice(id);
            response.DisplayMessage = "Success";
            return response;
        }

        [HttpPost]
        public async Task<object> AddMasterPrice([FromBody] MasterPrice Object)
        {
            await _cache.RemoveAsync(RedisCacheKeys.MasterPrice);
            response.Result = await _masterPriceService.AddMasterPrice(Object);
            response.DisplayMessage = "Master Price has been Added";
            return response;
        }

        [HttpDelete("{id}")]
        public async Task<object> DeleteMasterPrice(int id)
        {
            await _cache.RemoveAsync(RedisCacheKeys.MasterPrice);
            await _masterPriceService.DeleteMasterPrice(id);
            response.Result = true;
            response.DisplayMessage = "Master Price has been Deleted";
            return response;
        }

        [HttpPut("{id}")]
        public async Task<object> UpdateMasterPrice(int id, [FromBody] MasterPrice Object)
        {
            await _cache.RemoveAsync(RedisCacheKeys.MasterPrice);
            await _masterPriceService.UpdateMasterPrice(id, Object);
            response.Result = true;
            response.DisplayMessage = "Master Price has been Updated";
            return response;
        }
    }
}