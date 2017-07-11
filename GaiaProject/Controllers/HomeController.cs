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

        public IActionResult NewGame(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = Guid.NewGuid().ToString();
            }
            GameMgr.CreateNewGame(name, out GaiaGame result);
            return View(result);
        }

        public IActionResult BackupData()
        {
            GameMgr.BackupDictionary();
            return View();
        }
        public IActionResult RestoreData(string filename)
        {
            ViewData["nameList"] = string.Join(",", GameMgr.RestoreDictionary(filename));
            return View();
        }
        public IActionResult GetAllGame()
        {
            ViewData["nameList"] = string.Join(",", GameMgr.GetAllGame());
            return View();
        }
    }
}
