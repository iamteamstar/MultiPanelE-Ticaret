using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiPanelE_Ticaret.Areas.User.ViewModels;
using MultiPanelE_Ticaret.Core.Entities;
using MultiPanelE_Ticaret.Core.Enums;
using MultiPanelE_Ticaret.Data.Context;
using MultiPanelE_Ticaret.Core.DTOs;
namespace MultiPanelE_Ticaret.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = Roles.User)]
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ✅ /User/Order
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return View(orders);
        }

        // ✅ /User/Order/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // ✅ /User/Order/Create (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrderViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = _userManager.GetUserId(User);

            var order = new Order
            {
                UserId = userId!,
                Status = OrderStatus.Created,
                CreatedAt = DateTime.UtcNow
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout()
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>("cart");

            if (cart == null || !cart.Any())
                return RedirectToAction("Index");

            var userId = _userManager.GetUserId(User)!;
            var sellerId = cart.First().SellerId;

            using var tx = await _context.Database.BeginTransactionAsync();

            var order = new Order
            {
                UserId = userId,
                SellerId = sellerId,
                Status = OrderStatus.Created
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var item in cart)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null || product.Stock < item.Quantity)
                    throw new Exception("Stok yetersiz");

                product.Stock -= item.Quantity;

                _context.OrderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = product.Id,
                    SellerId = sellerId,
                    Quantity = item.Quantity,
                    Price = product.Price
                });
            }

            await _context.SaveChangesAsync();
            await tx.CommitAsync();

            HttpContext.Session.Remove("cart");

            return RedirectToAction("Success");
        }

    }
}
