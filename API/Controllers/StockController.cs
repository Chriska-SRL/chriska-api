using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class StockController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
