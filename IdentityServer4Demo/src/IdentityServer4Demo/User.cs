using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer4Demo.Models
{
    public class User
    {
        public string UserId { get; set; }
        public ICollection<Claim> Claims { get; set; }
        public bool Enabled { get; set; }
        public string Password { get; set; }
        public string Provider { get; set; }
        public string ProviderId { get; set; }
        public string Subject { get; set; }
        public string Username { get; set; }
    }
}