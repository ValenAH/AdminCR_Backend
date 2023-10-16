using Domain.Contracts;
using Domain.Contracts.DTO;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdminCRWeb.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class PaymentController : ControllerBase
    {
        IConfiguration _config;
        IPaymentService _service;

        public PaymentController(IConfiguration config, IPaymentService service)
        {
            _config = config;
            _service = service;
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ListPaymentBySale(int id)
        {
            var response = new Response<List<PaymentDTO>>();
            try
            {
                response.Data = await _service.ListPaymentBySale(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Header.Code = 500;
                response.Header.Message = ex.ToString();
                return BadRequest(response);
            }
        }

        [HttpPost]
        [Route("SavePayment")]
        public async Task<IActionResult> SavePayment(List<PaymentDTO> req)
        {
            var response = new Response<bool>();
            try
            {
                response.Data = await _service.SavePayment(req);
                return Ok(response);
            }catch (Exception ex)
            {
                response.Header.Code = 500;
                response.Header.Message = ex.ToString();
                return BadRequest(response);
            }
        }
        
    }
}
