using Microsoft.AspNetCore.Identity;

namespace BlackBox.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty;  
        public string EUID { get; set; } = string.Empty;
    }
}
 