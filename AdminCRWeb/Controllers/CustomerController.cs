using Domain.Contracts;
using Domain.Contracts.DTO;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdminCRWeb.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class CustomerController : Controller
    {
        public IConfiguration _config;
        public ICustomerService _service;

        public CustomerController(IConfiguration config, ICustomerService service)
        {
            _config = config;
            _service = service;
        }

        [HttpGet]
        [Route("GetCustomers")]
        public async Task<IActionResult> GetProducts()
        {
            var response = new Response<List<CustomerDTO>>();
            try
            {
                response.Data = await _service.ListCustomers();
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
        [Route("GetCustomerById")]
        public async Task<IActionResult> GetCustomerById(CustomerDTO req)
        {
            var response = new Response<CustomerDTO>();
            try
            {
                response.Data = await _service.GetCustomerById(req.CustomerId);
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
        [Route("SaveCustomer")]
        public async Task<IActionResult> SaveCustomer(CustomerDTO customer)
        {
            var response = new Response<bool>();
            try
            {
                response.Data = await _service.SaveCustomer(customer);
                response.Header.Message = response.Data ? "El cliente se ha creado con éxito" : "No se guardó el cliente";
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
