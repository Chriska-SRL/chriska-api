using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class SuppliersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
