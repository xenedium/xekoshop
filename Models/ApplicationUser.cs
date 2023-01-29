using Microsoft.AspNetCore.Identity;

namespace xekoshop.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsAdmin { get; set; } = false;
    }
}