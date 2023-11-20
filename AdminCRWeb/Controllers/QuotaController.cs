using Domain.Contracts;
using Domain.Contracts.DTO;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdminCRWeb.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class QuotaController : ControllerBase
    {
        public IConfiguration _config;
        public IQuotaService _service;

        public QuotaController(IConfiguration config, IQuotaService service)
        {
            _config = config;
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetQuotas()
        {
            var response = new Response<List<QuotaDTO>>();
            try
            {
                response.Data = await _service.ListQuotas();
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Header.Code = 500;
                response.Header.Message = ex.ToString();
                return BadRequest(response);
            }
        }
        [HttpPost]
        [Route("SaveQuota")]
        public async Task<IActionResult> SaveQuota(QuotaDTO quota)
        {
            var response = new Response<bool>();
            try
            {
                response.Data = await _service.SaveQuota(quota);
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Header.Code = 500;
                response.Header.Message = ex.ToString();
                return BadRequest(response);
            }
        }
    }
}
