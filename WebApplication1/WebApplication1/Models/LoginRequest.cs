using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class LoginRequest
    {
        public int IdUser { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        
        public byte[]? Salt { get; set; }
        public ICollection<AppUser>? AppUser { get; set; } 
    }
}