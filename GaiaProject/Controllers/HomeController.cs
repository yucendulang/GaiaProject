using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GaiaCore.Gaia;
using GaiaProject.Models.HomeViewModels;

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
            ViewData["Message"] =string.Join("\r\n", GameMgr.GetAllBackupDataName());

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
        // POST: /Home/NewGame
        [HttpPost]
        public IActionResult NewGame(NewGameViewModel model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                model.Name = Guid.NewGuid().ToString();
            }
            GameMgr.CreateNewGame(model.Name, out GaiaGame result);
            return View(model);
        }
        // GET: /Home/NewGame
        [HttpGet]
        public IActionResult NewGame()
        {
            return View();
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
