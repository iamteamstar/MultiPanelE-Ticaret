using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiPanelE_Ticaret.Areas.Seller.ViewModels;
using MultiPanelE_Ticaret.Core.Entities;
using MultiPanelE_Ticaret.Core.Enums;
using MultiPanelE_Ticaret.Data.Context;

namespace MufltiPanelE_Ticaret.Areas.Seller.Controllers
{
    [Area("Seller")]
    [Authorize(Roles = Roles.Seller)]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // LIST
        public async Task<IActionResult> Index()
        {
            var sellerId = _userManager.GetUserId(User);

            var products = await _context.Products
                .Where(p => p.SellerId == sellerId)
                .ToListAsync();

            return View(products);
        }

        [HttpGet]
        // CREATE (GET)
        public IActionResult Create()
        {
            return View();
        }

        // CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var product = new Product
            {
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock,
                SellerId = _userManager.GetUserId(User)!,
                IsActive = true
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet("Seller/Product/Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var sellerId = _userManager.GetUserId(User);

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id && p.SellerId == sellerId);

            if (product == null)
                return NotFound();

            var vm = new ProductEditViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                IsActive = product.IsActive
            };

            return View(vm);
        }

        // EDIT (POST)
        [HttpPost("Seller/Product/Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var sellerId = _userManager.GetUserId(User);

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id && p.SellerId == sellerId);

            if (product == null)
                return NotFound();

            product.Name = model.Name;
            product.Price = model.Price;
            product.Stock = model.Stock;
            product.IsActive = model.IsActive;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> ToggleStatus(int id)
        {
            var sellerId = _userManager.GetUserId(User);

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id && p.SellerId == sellerId);

            if (product == null)
                return NotFound();

            product.IsActive = !product.IsActive;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
