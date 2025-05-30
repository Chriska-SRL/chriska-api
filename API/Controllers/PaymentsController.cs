using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PaymentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
