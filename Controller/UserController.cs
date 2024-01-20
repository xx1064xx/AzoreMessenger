using AzoreMessanger.Data;
using AzoreMessanger.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AzoreMessanger.Controller
{
    public class UserController
    {
    }

    public class Logininfo
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

                // generiert einen zufälligen Schlüssel um den privaten schlüssel zu verschlüsseln und entschlüsseln.
                // Dies dient nur dazu, dass lokal gespeicherte private Keys nicht von anderen Usern auf dem lokalen Gerät ausgelesen werden können
                string zeichenfolge = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                Random random = new Random();
                int stringLaenge = 40;
                string keypass = new string(Enumerable.Repeat(zeichenfolge, stringLaenge)
                                                                   .Select(s => s[random.Next(s.Length)])
                                                                   .ToArray());


                string salt;
                string pwHash = HashGenerator.GenerateHash(registerInfo.Password, out salt);
                User newUser = new User()
                {
                    username = registerInfo.Username,
                    email = registerInfo.Email,
                    password = pwHash,
                    Salt = salt,
                    keyPass = keypass,
                };
                _context.Users.Add(newUser);
                _context.SaveChanges();
                return Ok(CreateToken(newUser.Id, newUser.email));
            }

            return BadRequest();
        }

        [HttpPost("login")]
        public IActionResult Login(Logininfo logininfo)
        {
            User userInDb = _context.Users.FirstOrDefault(user => user.email == logininfo.Email);

            if (userInDb != null
                && HashGenerator.VerifyHash(userInDb.password, logininfo.Password, userInDb.Salt))
            {
                return Ok(CreateToken(userInDb.Id, userInDb.email));
            }
            return Unauthorized();
        }

        [Authorize]
        [HttpGet("getCurrentUser")]
        public IActionResult getCurrentUser()
        {
            try
            {
                var userId = GetUserIdFromToken();
                var user = _context.Users.FirstOrDefault(u => u.Id == userId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        [Authorize]
        [HttpDelete("delete")]
        public IActionResult DeleteUser()
        {
            var userId = GetUserIdFromToken();

            if (userId != null)
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == userId);

                if (user != null && user.Id != 1)
                {

                    // Lösche den Benutzer
                    _context.Users.Remove(user);

                    _context.SaveChanges();
                    return Ok("Benutzer erfolgreich gelöscht");
                }

                return NotFound("Benutzer nicht gefunden");
            }

            return Unauthorized("Ungültiges Token");
        }

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            try
            {
                
                var userId = GetUserIdFromToken();

                var user = _context.Users.FirstOrDefault(u => u.Id == userId);
                if (user != null)
                {
                    return Ok("Logout erfolgreich");
                }

                return NotFound("Benutzer nicht gefunden");
            }
            catch (Exception ex)
            {
                return BadRequest("Logout fehlgeschlagen");
            }
        }


        private long? GetUserIdFromToken()
        {
            Claim subClaim = User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier);
            long userId = Convert.ToInt64(subClaim.Value);

            return userId;
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
