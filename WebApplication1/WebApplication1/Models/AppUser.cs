using System;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models
{
    public class AppUser : IdentityUser
    {
        public int IdToken { get; set; }
        public int IdUser { get; set; }
        
        public string? LoginToken { get; set; }
        
        public string? RefreshToken { get; set; }
        
        public DateTime RefreshTokenExpirationDate { get; set; }
        
        public virtual LoginRequest? LoginRequest { get; set; }
    }
}