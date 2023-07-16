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
    [ApiController]
    [Route("api/[Controller]")]
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
        [HttpGet]
        [Route("GetProducts")]
        public async Task<IActionResult> GetProducts()
        {
            var response = new Response<List<ProductDTO>>();
            try
            {
                response.Data = await _service.ListProducts();
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
        [Route("GetProductById")]
        public async Task<IActionResult> GetProductById(ProductDTO req)
        {
            var response = new Response<ProductDTO>();
            try
            {
                response.Data = await _service.GetProductById(req.Id);
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
        [Route("SaveProduct")]
        public async Task<IActionResult> SaveProduct(ProductDTO product)
        {
            var response = new Response<bool>();
            try
            {
                response.Data = await _service.SaveProduct(product);
                response.Header.Message = response.Data ? "El producto se ha creado con éxito" : "No se guardó el producto";
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
        [Route("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(ProductDTO product)
        {
            var response = new Response<bool>();
            try
            {
                response.Data = await _service.UpdateProduct(product);
                response.Header.Message = response.Data ? "El producto se ha actualizado con éxito" : "No se actualizo el producto";
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
