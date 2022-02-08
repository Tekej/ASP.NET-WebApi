using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Models;
using static WebApplication1.PasswordCripter.PasswordCripter;
using static WebApplication1.PasswordCripter.SaltCreater;

namespace WebApplication1.Controllers
{
    [Authorize]
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MainDbContext _context;
        private readonly JWTSettings _jwtSettings;

        public AccountController(IConfiguration configuration, MainDbContext context, IOptions<JWTSettings> jwtSettings)
        {
            _configuration = configuration;
            _context = context;
            _jwtSettings = jwtSettings.Value;
        }


        [AllowAnonymous]
        [HttpPost("signUp")]
        public IActionResult SignUp(LoginRequest loginRequest)
        {
            if (loginRequest.Login.IsNullOrWhiteSpace() ||
                loginRequest.Password.IsNullOrWhiteSpace())
            {
                return Ok("You need write Login and Password");
            }
            var checker = _context.LoginRequest.Where(s => s.Login == loginRequest.Login);
            if (checker.Any()) return Ok("This login is already taken");
            var salt = saltCreater();
            var newUser = new LoginRequest
            {
                Login = loginRequest.Login,
                Salt = salt,
                Password = passwordCripter(loginRequest.Password, salt)
            };
            _context.Add(newUser);
            _context.SaveChanges();
            return Ok("Successfully sign up ");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(LoginRequest loginRequest)
        {
            if (loginRequest.Login.IsNullOrWhiteSpace() ||
                loginRequest.Password.IsNullOrWhiteSpace()) return Ok("Login and password are reqired");
            var user = _context.LoginRequest
                .Where(x => x.Login == loginRequest.Login);
            var userSalt = user.Select(x => x.Salt).First();
            var userAuthenticate = user.Where(x => x.Password == passwordCripter(loginRequest.Password, userSalt));
            if (!userAuthenticate.Any())
            {
                return Ok("Invalid username or password");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Select(x => x.Login).First()!)
                }),
                Expires = DateTime.UtcNow.AddMonths(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)    
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var userToken = tokenHandler.WriteToken(token);
            var refreshToken = Guid.NewGuid().ToString();
            var refreshTokenExpirationDate = DateTime.Now.AddDays(1);
            var oldToken = _context.AppUser.First(x => x.LoginRequest!.Login == loginRequest.Login);
            var tokens = new AppUser
            {
                IdUser = _context.LoginRequest.First(x => x.Login == loginRequest.Login).IdUser,
                LoginToken = userToken,
                RefreshToken = refreshToken,
                RefreshTokenExpirationDate = refreshTokenExpirationDate
            };
            _context.Remove(oldToken);
            _context.Add(tokens);
            _context.SaveChanges();

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken
            }); 
        }
        [AllowAnonymous]
        [HttpPost("refreshLoginToken")]
        public IActionResult RefreshLoginToken(AppUser givenRefreshToken)
        {
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
            var userLogin = _context.AppUser.Where(x => x.RefreshToken == givenRefreshToken.RefreshToken)
                .Select(x => x.LoginRequest!.Login).First();
            if (userLogin == null) return Ok("Refresh token is invalid");
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, userLogin)
                    }),
                    Expires = DateTime.UtcNow.AddMonths(6),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)    
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var userToken = tokenHandler.WriteToken(token);
                var refreshToken = Guid.NewGuid().ToString();
                var refreshTokenExpirationDate = DateTime.Now.AddDays(1); 
                var oldToken = _context.AppUser.First(x => x.RefreshToken == givenRefreshToken.RefreshToken);
                var tokens = new AppUser
                {
                    IdUser = _context.AppUser.Where(x => x.RefreshToken == givenRefreshToken.RefreshToken).Select(x => x.LoginRequest!.IdUser).First(),
                    LoginToken = userToken,
                    RefreshToken = refreshToken,
                    RefreshTokenExpirationDate = refreshTokenExpirationDate
                };
                _context.Remove(oldToken);
                _context.Add(tokens);
                _context.SaveChanges();

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken
                });
            }

        }
    }
}