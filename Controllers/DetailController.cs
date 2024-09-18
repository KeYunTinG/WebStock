using Microsoft.AspNetCore.Mvc;

namespace WebStock.Controllers
{
    public class DetailController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
