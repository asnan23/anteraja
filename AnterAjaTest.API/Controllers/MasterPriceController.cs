using AnterAjaTest.API.Extensions;
using AnterAjaTest.API.Models;
using AnterAjaTest.API.Services;
using AnterAjaTest.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
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
        private readonly ILogger<MasterPriceController> _logger;
        private readonly IConfiguration _configuration;
        public MasterPriceController(IMasterPriceService masterPriceService, IDistributedCache cache, 
            ILogger<MasterPriceController> logger, IConfiguration configuration)
        {
            _masterPriceService = masterPriceService;
            this.response = new ResponseDto();
            _cache = cache;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<object> GetAll()
        {
            try
            {
                var result = new List<MasterPrice>();
                result = await _cache.GetRecordAsync<List<MasterPrice>>(RedisCacheKeys.MasterPrice);
                if (result is null)
                {
                    result = await _masterPriceService.GetAllMasterPrices();
                    if (result.Count > 0)
                    {
                        await _cache.SetRecordAsync(RedisCacheKeys.MasterPrice, result, TimeSpan.FromSeconds(Convert.ToDouble(_configuration.GetValue<string>("AppSetting:RedisExp"))));
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

            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                response.IsSuccess = false;
                response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return response;
        }

        [HttpGet("Redis")]
        public async Task<object> GetAllRedis(string key)
        {
            try
            {
                var result = await _cache.GetRecordAsync<MasterPrice>(key);
                if (result is null)
                {
                    response.DisplayMessage = "No Data Found on Redis";
                    response.Result = result;
                }
                else
                {
                    response.DisplayMessage = "Data from Redis";
                    response.Result = result;
                }

            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                response.IsSuccess = false;
                response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return response;
        }

        [HttpGet("{id}")]
        public async Task<object> Get(int id)
        {
            try
            {
                response.Result = await _masterPriceService.GetMasterPrice(id);
                response.DisplayMessage = "Success";

            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                response.IsSuccess = false;
                response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return response;
        }

        [HttpPost]
        public async Task<object> AddMasterPrice([FromBody] MasterPrice Object)
        {
            try
            {
                await _cache.RemoveAsync(RedisCacheKeys.MasterPrice);
                if (await _masterPriceService.MasterPriceExist(Object))
                {
                    response.IsSuccess = false;
                    response.DisplayMessage = "data exists in database";
                }
                else
                {
                    var result = await _masterPriceService.AddMasterPrice(Object);
                    var key = $"{Object.origin_code},{Object.destination_code},{Object.product}";
                    await _cache.SetRecordAsync(key, result, TimeSpan.FromSeconds(Convert.ToDouble(_configuration.GetValue<string>("AppSetting:RedisExp"))));
                    response.Result = result;
                    response.DisplayMessage = "Master Price has been Added";
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                response.IsSuccess = false;
                response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return response;
        }

        [HttpDelete("{id}")]
        public async Task<object> DeleteMasterPrice(int id)
        {
            try
            {
                var price = await _masterPriceService.GetMasterPrice(id);
                var key = $"{price.origin_code},{price.destination_code},{price.product}";
                await _cache.RemoveAsync(key);
                await _cache.RemoveAsync(RedisCacheKeys.MasterPrice);
                await _masterPriceService.DeleteMasterPrice(id);
                response.Result = true;
                response.DisplayMessage = "Master Price has been Deleted";
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                response.IsSuccess = false;
                response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return response;
        }

        [HttpPut("{id}")]
        public async Task<object> UpdateMasterPrice(int id, [FromBody] MasterPrice Object)
        {
            try
            {
                await _cache.RemoveAsync(RedisCacheKeys.MasterPrice);
                await _masterPriceService.UpdateMasterPrice(id, Object);
                response.Result = true;
                response.DisplayMessage = "Master Price has been Updated";
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                response.IsSuccess = false;
                response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return response;
        }

    }
}
