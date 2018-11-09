using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TelFlix.App.Controllers
{
    public class UsersController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}