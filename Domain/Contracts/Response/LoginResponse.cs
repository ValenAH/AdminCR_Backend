using Domain.Contracts.DTO;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.Response
{
    public class LoginResponse
    {
        public string Token { get; set; }
    }
}
