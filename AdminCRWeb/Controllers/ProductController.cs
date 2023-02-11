using Domain.Contracts;
using Domain.Contracts.DTO;
using Domain.Contracts.Request;
using Domain.Contracts.Response;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AdminCRWeb.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    //[Authorize]
    public class ProductController : ControllerBase
    {
        public IConfiguration _config;
        public IProductService _service;

        public ProductController(IConfiguration config, IProductService service)
        {
            _config = config;
            _service = service;
        }
        [HttpPost]
        [Route("Products")]

        public async Task<IActionResult> GetProduct(ProductRequest req)
        {
            var response = new Response<ProductResponse>();
            try
            {
                var product = await _service.GetProductById(req.IdProduct);


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
