using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PurchasesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
