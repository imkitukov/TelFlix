using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TelFlix.App.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Account()
        {
            return View();
        }

        public IActionResult Library()
        {
            return View();
        }

        public IActionResult WishList()
        {
            return View();
        }

        public IActionResult Reviews()
        {
            return View();
        }
    }
}