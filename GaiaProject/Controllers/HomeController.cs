using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GaiaCore.Gaia;
using GaiaProject.Models.HomeViewModels;
using Microsoft.AspNetCore.Identity;
using GaiaProject.Models;

namespace GaiaProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public HomeController(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var task =_userManager.GetUserAsync(HttpContext.User);

            ViewData["Message"] = task.Result.UserName;
            ViewData["GameList"]=GameMgr.GetAllGame(task.Result.UserName);
            //ViewData["Message"] = @"yucenyucen@126.com";
            //ViewData["GameList"] = GameMgr.GetAllGame(@"yucenyucen@126.com");
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = string.Join("\r\n", GameMgr.GetAllBackupDataName());

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            ViewData["nameList"] = string.Join(",", GameMgr.GetAllGame());
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
            string[] username = new string[] { model.Player1, model.Player2, model.Player3, model.Player4 };
            GameMgr.CreateNewGame(model.Name, username, out GaiaGame result);
            ViewData["ReturnUrl"] = "/Home/ViewGame/" + model.Name;
            return View(model);
        }
        // GET: /Home/NewGame
        [HttpGet]
        public IActionResult NewGame()
        {
            return View();
        }

        public IActionResult ViewGame(string id)
        {
            var gg = GameMgr.GetGameByName(id);
            return View(gg);
        }
        [HttpPost]
        public IActionResult SyntaxGame(string name,string syntax)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(syntax))
            {
                return Redirect("/home/index");
            }
            GameMgr.GetGameByName(name).ProcessSyntax(syntax);

            return Redirect("/home/viewgame/"+name);
        }

        #region 管理工具
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
        #endregion
    }
}
