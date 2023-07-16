using Domain.Contracts;
using Domain.Contracts.DTO;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdminCRWeb.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class CategoryController : ControllerBase
    {
        

        public IConfiguration _config;
        public ICategoryService _service;
        public CategoryController(IConfiguration config, ICategoryService service)
        {
            _config = config;
            _service = service;
        }

        [HttpGet]
        [Route("GetCategories")]
        public async Task<IActionResult> GetCategories()
        {
            var response = new Response<List<CategoryDTO>>();
            try
            {
                response.Data = await _service.ListCategories();
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
