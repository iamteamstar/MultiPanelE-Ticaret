using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiPanelE_Ticaret.Areas.Admin.ViewModels;
using MultiPanelE_Ticaret.Core.Entities;
using MultiPanelE_Ticaret.Core.Enums;
using MultiPanelE_Ticaret.Data.Context;
using MultiPanelE_Ticaret.Services.Order;
namespace MultiPanelE_Ticaret.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Admin)]
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOrderStateService _orderStateService;
        public OrderController(
    AppDbContext context,
    UserManager<ApplicationUser> userManager,
    IOrderStateService orderStateService)
        {
            _context = context;
            _userManager = userManager;
            _orderStateService = orderStateService;
        }

        // LIST – Bekleyen Siparişler
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Courier)
                .Where(o => o.Status == OrderStatus.Created || o.Status == OrderStatus.Preparing)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return View(orders);
        }

        // ASSIGN (GET)
        public async Task<IActionResult> Assign(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();

            var couriers = await _userManager.GetUsersInRoleAsync(Roles.Courier);

            var model = new AssignCourierViewModel
            {
                OrderId = order.Id,
                Couriers = couriers.Select(c => new CourierOptionVm
                {
                    Id = c.Id,
                    Email = c.Email!,
                    IsActive = c.LockoutEnd == null || c.LockoutEnd <= DateTime.UtcNow
                }).ToList()
            };

            return View(model);
        }

        // ASSIGN (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(AssignCourierViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var order = await _context.Orders.FindAsync(model.OrderId);
            if (order == null)
                return NotFound();

            order.eCourierId = model.CourierId;
            order.Status = OrderStatus.Preparing;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ChangeStatus(int id, OrderStatus nextStatus)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();

            if (!_orderStateService.CanTransition(order.Status, nextStatus))
                return BadRequest("Geçersiz durum geçişi");

            order.Status = nextStatus;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> AssignCourier(int orderId, string courierId)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
                return NotFound();

            order.CourierId = courierId;
            order.Status = OrderStatus.OnTheWay;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
