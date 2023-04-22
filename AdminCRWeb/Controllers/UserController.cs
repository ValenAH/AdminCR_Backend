using Domain.Contracts;
using Domain.Contracts.DTO;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdminCRWeb.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class UserController : Controller
    {
        public IConfiguration _config;
        public IUserService _service;
        public UserController(IConfiguration config, IUserService service)
        {
            _config = config;
            _service = service;
        }
        [HttpGet]
        [Route("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var response = new Response<List<UserDTO>>();
            try
            {
                response.Data = await _service.ListUsers();
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
        [Route("SaveUser")]
        public async Task<IActionResult> SaveUser(UserDTO user)
        {
            var response = new Response<bool>();
            try
            {
                response.Data = await _service.SaveUser(user);
                response.Header.Message = response.Data ? "El usuario ha sido creado con éxito" : "El usuario no se guardó";
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
