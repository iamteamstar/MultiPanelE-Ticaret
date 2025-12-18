using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiPanelE_Ticaret.Areas.User.ViewModels;
using MultiPanelE_Ticaret.Core.DTOs;
using MultiPanelE_Ticaret.Core.Entities;
using MultiPanelE_Ticaret.Core.Enums;
using MultiPanelE_Ticaret.Data.Context;
using MultiPanelE_Ticaret.Extensions;
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
        // GET: /User/Order
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
        // GET
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST
        // POST: /User/Order/CreateOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder()
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>("cart");

            if (cart == null || !cart.Any())
                return RedirectToAction("Cart");

            var userId = _userManager.GetUserId(User);

            var order = new Order
            {
                UserId = userId!,
                SellerId = cart.First().SellerId,
                Status = OrderStatus.Created,
                CreatedAt = DateTime.UtcNow
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var item in cart)
            {
                _context.OrderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    SellerId = item.SellerId
                });
            }

            await _context.SaveChangesAsync();
            HttpContext.Session.Remove("cart");

            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout()
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>("cart");

            if (cart == null || !cart.Any())
                return RedirectToAction("Cart");

            var userId = _userManager.GetUserId(User);

            var order = new Order
            {
                UserId = userId,
                SellerId = cart.First().SellerId,
                Status = OrderStatus.Created
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var item in cart)
            {
                _context.OrderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    SellerId = item.SellerId
                });

                var product = await _context.Products.FindAsync(item.ProductId);
                if (product != null)
                    product.Stock -= item.Quantity;
            }

            await _context.SaveChangesAsync();
            HttpContext.Session.Remove("cart");

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Increase(int productId)
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>("cart") ?? new();
            var item = cart.First(x => x.ProductId == productId);
            item.Quantity++;
            HttpContext.Session.SetObject("cart", cart);
            return RedirectToAction("Cart");
        }

        [HttpPost]
        public IActionResult Decrease(int productId)
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>("cart");
            var item = cart?.FirstOrDefault(x => x.ProductId == productId);

            if (item != null)
            {
                item.Quantity--;
                if (item.Quantity <= 0)
                    cart!.Remove(item);
            }

            HttpContext.Session.SetObject("cart", cart);
            return RedirectToAction("Cart");
        }

        [HttpPost]
        public IActionResult Remove(int productId)
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>("cart") ?? new();

            var item = cart.FirstOrDefault(x => x.ProductId == productId);
            if (item != null)
                cart.Remove(item);

            HttpContext.Session.SetObject("cart", cart);
            return RedirectToAction("Cart");
        }
       


    }
}
