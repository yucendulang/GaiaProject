﻿using System;
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



            var task = _userManager.GetUserAsync(HttpContext.User);
            Task[] taskarray = new Task[] { task };
            Task.WaitAll(taskarray, millisecondsTimeout: 1000);
            if (task.Result != null)
            {
                ViewData["Message"] = task.Result.UserName;
                ViewData["GameList"] = GameMgr.GetAllGame(task.Result.UserName);
            }
#if DEBUG
            ViewData["Message"] = @"yucenyucen@126.com";
            ViewData["GameList"] = GameMgr.GetAllGame(@"yucenyucen@126.com");
#endif

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
        public IActionResult ViewGame(string name, string syntax, string factionName)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(syntax))
            {
                return View(GameMgr.GetGameByName(name));
            }

            if (!string.IsNullOrEmpty(factionName))
            {
                syntax = string.Format("{0}:{1}", factionName, syntax);
            }
            GameMgr.GetGameByName(name).Syntax(syntax, out string log);

            if (!string.IsNullOrEmpty(log))
            {
                ModelState.AddModelError(string.Empty, log);
            }
            return View(GameMgr.GetGameByName(name));
        }

        [HttpPost]
        public IActionResult LeechPower(string name,FactionName factionName, int power, FactionName leechFactionName, bool isLeech)
        {
            var faction = GameMgr.GetGameByName(name).FactionList.Find(x => x.FactionName.ToString().Equals(name));
            var leech = isLeech ? "leech" : "decline";
            var syntax = string.Format("{0}:{1} {2} from {3}", factionName, leech, power, leechFactionName);
            GameMgr.GetGameByName(name).Syntax(syntax, out string log);
            return Redirect("/home/viewgame/" + name);
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
            return Redirect("/home/viewgame/" + "test01");
            //return View();
        }
        public IActionResult GetAllGame()
        {
            ViewData["nameList"] = string.Join(",", GameMgr.GetAllGame());
            return View();
        }
        #endregion
    }
}
