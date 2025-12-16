using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiPanelE_Ticaret.Data.Context;

namespace MultiPanelE_Ticaret.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        // /User/Product
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Where(p => p.IsActive && p.Stock > 0)
                .OrderBy(p => p.Name)
                .ToListAsync();

            return View(products);
        }
    }
}
