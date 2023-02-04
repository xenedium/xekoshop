using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using xekoshop.Data;
using xekoshop.Models;

namespace xekoshop.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Cart
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Cart.Include(c => c.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Cart/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var cart = await _context.Cart
                .Include(c => c.User)
                .Include(c => c.CartLines)
                .ThenInclude(cl => cl.Product)
                .FirstAsync(m => m.Id == id);

            return View(cart);
        }

        // GET: Mycart
        [Authorize]
        public async Task<IActionResult> Mycart()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();
            var cart = await _context.Cart
                .Include(c => c.User)
                .Where(c => c.User.Id == user.Id)
                .Include(c => c.CartLines)
                .ThenInclude(cl => cl.Product)
                .FirstAsync();
            return View(cart);
        }
        private bool CartExists(int id)
        {
          return (_context.Cart?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}