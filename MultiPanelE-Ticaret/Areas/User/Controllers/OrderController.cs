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
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>("cart");
            if (cart == null || !cart.Any())
                return RedirectToAction("Index", "Cart");

            var userId = _userManager.GetUserId(User);

            var order = new Order
            {
                UserId = userId,
                SellerId = cart.First().SellerId,
                CreatedAt = DateTime.Now
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            HttpContext.Session.Remove("cart");

            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            var orders = _context.Orders
                .Where(o => o.UserId == userId)
                .ToList();

            return View(orders);
        }
    }




}
