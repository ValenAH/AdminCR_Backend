using Domain.Contracts;
using Domain.Contracts.DTO;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdminCRWeb.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class SaleDetailsController : ControllerBase
    {
        public IConfiguration _config;
        public ISaleDetailsService _service;

        public SaleDetailsController(IConfiguration config, ISaleDetailsService service)
        {
            _config = config;
            _service = service;
        }

        [HttpPost]
        [Route("GetSaleDetails")]

        public async Task<IActionResult> ListSaleDetails(SaleDetailsDTO req)
        {
            var response = new Response<List<SaleDetailsDTO>>();
            try
            {
                response.Data = await _service.ListSaleDetails(req.SaleId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Header.Code = 500;
                response.Header.Message = ex.ToString();
                return BadRequest(response);
            }
        }

    }
}
