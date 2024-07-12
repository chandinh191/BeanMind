using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class SessionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
