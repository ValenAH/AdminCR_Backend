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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private IConfiguration _config;
        private IUserService _service;

        public AuthController(IConfiguration config, IUserService service)
        {
            _config = config;
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginRequest req)
        {
            try
            {
                var user = await _service.GetUserByCredentials(req);
                var response = new Response<LoginResponse>();
                if (user == null)
                {
                    response.Header.Code = 200;
                    response.Header.Message = "Usuario o contraseña incorrecta";
                    return BadRequest(response);
                }
                response.Data = new LoginResponse();
                response.Data.Token = GenerateToken(user);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

        }

        private string GenerateToken(UserDTO user)
        {
            var jwt = _config.GetSection("Jwt").Get<Jwt>();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.AddHours(1).ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.Name),
            new Claim("Id", user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.IdRole.ToString()),
            };

            var token = new JwtSecurityToken(
                        jwt.Issuer,
                        jwt.Audience,
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(60),
                        signingCredentials: credentials
                    );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        [HttpGet]
        [Route("AuthRoute")]
        [Authorize(Roles ="1")]
        public IActionResult AuthRoute()
        {
            return Ok(true);
        }
    }
}
