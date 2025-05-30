using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ReturnsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
