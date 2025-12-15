using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiPanelE_Ticaret.Core.Enums;

namespace MultiPanelE_Ticaret.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        [Area("Admin")]
        [Authorize(Roles =Roles.Admin)]
        public IActionResult Index()
        {
            return View();
        }
    }
}
