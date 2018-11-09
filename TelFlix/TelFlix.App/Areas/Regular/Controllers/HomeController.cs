using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TelFlix.App.Areas.Regular.Controllers
{

    [Area("Regular")]
    [Authorize(Roles = "RegularUser")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}