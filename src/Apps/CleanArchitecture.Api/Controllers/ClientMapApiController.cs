using BkashSNS.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System;
using BkashSNS.Application.Common.Interfaces;
using BkashSNS.Infrastructure.Services.Helper;

namespace BkashSNS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientMapApiController : ControllerBase
    {
        private readonly IClientMappingService _clientMappingService;

        public ClientMapApiController(IClientMappingService clientMappingService)
        {
            _clientMappingService = clientMappingService;

        }


        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll(string searchText = "")
        {
            try
            {
                if (StaticData.IsAuthorized(Request.Headers["Authorization"]) == false)
                {
                    return Unauthorized();
                }

                var data = await _clientMappingService.GetAll(searchText);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

        }


        [HttpGet("[action]")]
        public async Task<IActionResult> FindByMerchantWallet(string merchantWallet)
        {
            try
            {
                if (StaticData.IsAuthorized(Request.Headers["Authorization"]) == false)
                {
                    return Unauthorized();
                }

                var data = await _clientMappingService.FindByMerchantWallet(merchantWallet);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromBody] Client_Mapping model)
        {
            try
            {
                if (StaticData.IsAuthorized(Request.Headers["Authorization"]) == false)
                {
                    return Unauthorized();
                }
                model.id = Guid.NewGuid().ToString();
                model.create_date = DateTime.Now;
                var data = await _clientMappingService.Create(model);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Update([FromBody] Client_Mapping model)
        {
            try
            {
                if (StaticData.IsAuthorized(Request.Headers["Authorization"]) == false)
                {
                    return Unauthorized();
                }

                var data = await _clientMappingService.Update(model);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Remove([FromBody] Client_Mapping model)
        {
            try
            {
                if (StaticData.IsAuthorized(Request.Headers["Authorization"]) == false)
                {
                    return Unauthorized();
                }

                await _clientMappingService.Delete(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

        }


    }
}
