using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using xekoshop.Data;
using xekoshop.Models;

namespace xekoshop.Controllers
{
    [Authorize]
    public class CartLineController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartLineController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> AddToCart(int articleId, int quantity)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();
            var cart = await _context.Cart
                .Include(c => c.User)
                .Where(c => c.User.Id == user.Id)
                .Include(c => c.CartLines)
                .ThenInclude(cl => cl.Product)
                .FirstAsync();
            
            var product = await _context.Product.FindAsync(articleId);
            if (product == null) return NotFound();
            
            var cartLine = cart.CartLines.FirstOrDefault(cl => cl.Product.Id == articleId);
            if (cartLine == null)
            {
                cartLine = new CartLine
                {
                    Cart = cart,
                    Product = product,
                    Quantity = quantity
                };
                _context.CartLine.Add(cartLine);
            }
            else
            {
                cartLine.Quantity += quantity;
                _context.CartLine.Update(cartLine);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Mycart", controllerName: "Cart");
        }

        // GET: CartLine
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CartLine.Include(c => c.Cart).Include(c => c.Product);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CartLine/Details/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CartLine == null)
            {
                return NotFound();
            }

            var cartLine = await _context.CartLine
                .Include(c => c.Cart)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cartLine == null)
            {
                return NotFound();
            }

            return View(cartLine);
        }

        // GET: CartLine/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["CartId"] = new SelectList(_context.Cart, "Id", "Id");
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id");
            return View();
        }

        // POST: CartLine/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("Id,ProductId,CartId,Quantity,CreatedAt,UpdatedAt")] CartLine cartLine)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cartLine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CartId"] = new SelectList(_context.Cart, "Id", "Id", cartLine.CartId);
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id", cartLine.ProductId);
            return View(cartLine);
        }

        // GET: CartLine/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CartLine == null)
            {
                return NotFound();
            }

            var cartLine = await _context.CartLine.FindAsync(id);
            if (cartLine == null)
            {
                return NotFound();
            }
            ViewData["CartId"] = new SelectList(_context.Cart, "Id", "Id", cartLine.CartId);
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id", cartLine.ProductId);
            return View(cartLine);
        }

        // POST: CartLine/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,CartId,Quantity,CreatedAt,UpdatedAt")] CartLine cartLine)
        {
            if (id != cartLine.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cartLine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartLineExists(cartLine.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CartId"] = new SelectList(_context.Cart, "Id", "Id", cartLine.CartId);
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id", cartLine.ProductId);
            return View(cartLine);
        }

        // GET: CartLine/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CartLine == null)
            {
                return NotFound();
            }

            var cartLine = await _context.CartLine
                .Include(c => c.Cart)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cartLine == null)
            {
                return NotFound();
            }

            return View(cartLine);
        }

        // POST: CartLine/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CartLine == null)
            {
                return Problem("Entity set 'ApplicationDbContext.CartLine'  is null.");
            }
            var user = await _userManager.GetUserAsync(User);
            var cartLine = await _context.CartLine
                .Include(cl => cl.Cart)
                .Where(cl => cl.Id == id)
                .FirstOrDefaultAsync();
            if (cartLine == null) return NotFound();
            if (cartLine.Cart.UserId != user?.Id) return Forbid();
            
            _context.CartLine.Remove(cartLine);
            await _context.SaveChangesAsync();
            return RedirectToAction("MyCart", controllerName: "Cart");
        }

        private bool CartLineExists(int id)
        {
          return (_context.CartLine?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
