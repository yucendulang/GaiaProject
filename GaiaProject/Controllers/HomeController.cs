using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GaiaProject2.Gaia;

namespace GaiaProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var game = new GaiaGame();
            return View(game);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
