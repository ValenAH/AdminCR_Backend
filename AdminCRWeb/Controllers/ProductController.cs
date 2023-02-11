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
    public class ProductController : ControllerBase
    {
        private IConfiguration _config;
        private IProductService _service;

        public ProductController(IConfiguration config, IProductService service)
        {
            _config = config;
            _service = service;
        }
        [HttpPost]
        [Route("Products")]

        public async Task<IActionResult> Product(ProductRequest req)
        {
            try
            {
                var product = await _service.GetProductById(req.IdProduct);
                var response = new Response<ProductResponse>();

                return Ok(response);
            }
            catch(Exception ex)
            {

            }
        }
    }
}
