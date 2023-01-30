using Microsoft.AspNetCore.Identity;

namespace xekoshop.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Cart Cart { get; set; } = new Cart();
    }
}