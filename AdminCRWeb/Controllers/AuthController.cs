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
    public class AuthController : ControllerBase
    {
        private IConfiguration _config;
        private IUserService _service;

        public AuthController(IConfiguration config, IUserService service)
        {
            _config = config;
            _service = service;
        }
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
                var jwt = _config.GetSection("Jwt").Get<Jwt>();
                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.AddHours(1).ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.Name),
                new Claim("IdUser", user.IdUser.ToString()),
                new Claim("IdRole", user.IdRole.ToString()),
            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
                var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                response.Data = new LoginResponse();
                var token = new JwtSecurityToken(
                        jwt.Issuer,
                        jwt.Audience,
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(60),
                        signingCredentials: singIn
                    );
                response.Data.Token = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(response);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.ToString());
            }
            
        }
    }
}
