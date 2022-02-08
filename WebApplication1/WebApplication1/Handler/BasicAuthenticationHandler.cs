using System;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebApplication1.Models;
using static WebApplication1.PasswordCripter.PasswordCripter;


namespace WebApplication1.Handler
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {

        private readonly MainDbContext _context;
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock,
            MainDbContext context) 
            : base(options, logger, encoder, clock)
        {
            _context = context;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if(!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Authorization header was not found");
            try
            {
                var authenticationHeaderValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var bytes = Convert.FromBase64String(authenticationHeaderValue.Parameter);
                var credentials = Encoding.UTF8.GetString(bytes).Split(":");
                var login = credentials[0];
                var password = credentials[1];
                var user = _context.LoginRequest
                    .Where(x => x.Login == login);
                var userPassword = user.Select(x => x.Password);
                var userSalt = user.Select(x => x.Salt).First();
                var userAuthenticate = _context.LoginRequest.First(x => x.Login == login && x.Password == passwordCripter(password, userSalt));
                if (userAuthenticate == null)
                {
                    return AuthenticateResult.Fail("Invalid username or password" + credentials);
                }
                else
                {
                    var claims = new[] {new Claim(ClaimTypes.Name, userAuthenticate.Login)};
                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);
                    return AuthenticateResult.Success(ticket);
                }
            }
            catch (Exception e)
            {
                return AuthenticateResult.Fail("Error has occured");
            }
           
        }
    }
        
    }
