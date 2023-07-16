using Domain.Contracts;
using Domain.Contracts.DTO;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdminCRWeb.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class PaymentMethodController : ControllerBase
    {
        public IConfiguration _config;
        public IPaymentMethodService _service;

        public PaymentMethodController(IConfiguration config, IPaymentMethodService service)
        {
            _config = config;
            _service = service;
        }

        [HttpGet]
        [Route("GetPaymentMethods")]
        public async Task<IActionResult> ListPaymentMethods()
        {
            var response = new Response<List<PaymentMethodDTO>>();
            try
            {
                response.Data = await _service.ListPaymentMethods();
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
