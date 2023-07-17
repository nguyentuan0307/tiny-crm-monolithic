using Microsoft.AspNetCore.Mvc;

namespace TinyCRM.API.Controllers
{
    public class DealController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
