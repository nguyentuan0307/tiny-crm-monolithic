using Microsoft.AspNetCore.Mvc;

namespace TinyCRM.API.Controllers
{
    public class LeadController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
