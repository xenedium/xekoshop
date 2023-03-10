using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using xekoshop.Models;

namespace xekoshop.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<xekoshop.Models.Product> Product { get; set; } = default!;
    public DbSet<Cart> Cart { get; set; } = default!;
    public DbSet<xekoshop.Models.CartLine> CartLine { get; set; } = default!;
}
