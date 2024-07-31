using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class ParentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
