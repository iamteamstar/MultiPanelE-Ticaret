using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiPanelE_Ticaret.Core.DTOs;
using MultiPanelE_Ticaret.Data.Context;
using MultiPanelE_Ticaret.Extensions;

namespace MultiPanelE_Ticaret.Areas.User.Controllers
{
    [Area("User")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        [HttpPost]
        public IActionResult AddToCart(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return RedirectToAction("Index");

            var cart = HttpContext.Session.GetObject<List<CartItem>>("cart") ?? new();

            var item = cart.FirstOrDefault(x => x.ProductId == id);
            if (item == null)
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = 1,
                    SellerId = product.SellerId
                });
            }
            else
            {
                item.Quantity++;
            }

            HttpContext.Session.SetObject("cart", cart);
            return RedirectToAction("Index");
        }
    }

}
