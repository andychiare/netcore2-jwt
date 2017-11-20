using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace JWT.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private IConfiguration _config;

        public TokenController(IConfiguration config)
        {
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateToken([FromBody]LoginModel login)
        {
            IActionResult response = Unauthorized();
            var user = Authenticate(login);

            if (user != null)
            {
                var tokenString = BuildToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }

        private string BuildToken(UserModel user)
        {

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
               };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserModel Authenticate(LoginModel login)
        {
            UserModel user = null;

            if (login.Username == "mario" && login.Password == "secret")
            {
                user = new UserModel { Name = "Mario Rossi", Email = "mario.rossi@domain.com" };
            }

            return user;

        }

        public class LoginModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        private class UserModel
        {
            public string Name { get; set; }
            public string Email { get; set; }
        }
    }
}
