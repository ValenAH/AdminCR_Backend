using Domain.Contracts;
using Domain.Contracts.DTO;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdminCRWeb.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class IdentificationTypeController : ControllerBase
    {
        public IConfiguration _config;
        public IIdentificationTypeService _service;
        public IdentificationTypeController(IConfiguration config, IIdentificationTypeService service)
        {
            _config = config;
            _service = service;
        }

        [HttpGet]
        [Route("GetIdentificationType")]
        public async Task<IActionResult> GetIdentificationType()
        {
            var response = new Response<List<IdentificationTypeDTO>>();
            try
            {
                response.Data = await _service.ListIdentificationType();
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
