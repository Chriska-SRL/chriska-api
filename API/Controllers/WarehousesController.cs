using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class WarehousesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
