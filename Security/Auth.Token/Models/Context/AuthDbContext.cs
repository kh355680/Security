using Microsoft.AspNet.Identity.EntityFramework;

namespace Auth.Token.Models.Context
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext() : base("DefaultConnection")
        {
            
        }
    }
}