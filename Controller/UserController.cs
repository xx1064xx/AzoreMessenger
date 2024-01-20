using AzoreMessanger.Data;
using AzoreMessanger.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AzoreMessanger.Controller
{
    public class UserController
    {
    }

    public class Loginingo
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterInfo
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserToken
    {
        public string Email { get; set; }
        public string JWT { get; set; }
        public DateTime ExpiresAt { get; set; }
    }

    // Api configurations

    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : ControllerBase
    {
        private readonly MessengerAppContext _context;
        public UsersController(MessengerAppContext context) { _context = context; }


        [HttpPost("register")]

        public IActionResult Register(RegisterInfo registerInfo)
        {
            User userInDb = _context.Users.FirstOrDefault(user => user.email == registerInfo.Email);
            if(userInDb == null)
            {
                string salt;
                string pwHash = HashGenerator.GenerateHash(registerInfo.Password, out salt);
                User newUser = new User()
                {
                    username = registerInfo.Username,
                    email = registerInfo.Email,
                    password = pwHash,
                    Salt = salt,
                };
                _context.Users.Add(newUser);
                _context.SaveChanges();
                return Ok(CreateToken(newUser.Id, newUser.email));
            }

            return BadRequest();
        }

        private UserToken CreateToken(long userId, string email)
        {
            var expires = DateTime.UtcNow.AddDays(5);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, $"{userId}"),
                    new Claim(JwtRegisteredClaimNames.Email, email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = expires,
                Issuer = JwtConfiguration.ValidIssuer,
                Audience = JwtConfiguration.ValidAudience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(JwtConfiguration.IssuerSigningKey)),
                        SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            return new UserToken { Email = email, ExpiresAt = expires, JWT = jwtToken };
        }

    }
}
