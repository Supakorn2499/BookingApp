using Microsoft.AspNetCore.Mvc;

namespace BookingApp.Server.Controllers
{
    public class SalemanController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
