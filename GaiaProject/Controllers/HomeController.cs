using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GaiaCore.Gaia;

namespace GaiaProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Message"] = "Index";
            return View();
        }

        public IActionResult Game()
        {
            var game = new GaiaGame();
            game.ProcessSyntax("Default Game", out string log);
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

        public IActionResult NewGame()
        {
            GameMgr.CreateNewGame(System.Guid.NewGuid().ToString(), out GaiaGame result);
            GameMgr.BakeDictionary();
            return View(result);
        }

        public IActionResult BackData()
        {
            GameMgr.BakeDictionary();
            return View();
        }
    }
}
