using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GaiaDbContext.Models;
using GaiaProject.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GaiaProject.Controllers
{
    public class GameInfoController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<ApplicationUser> _userManager;


        public GameInfoController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this._userManager = userManager;

        }
        // GET: /<controller>/
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public IActionResult Index(string username)
        {
            if (username == null)
            {
                username = HttpContext.User.Identity.Name;
            }
            //this.dbContext.GameInfoModel.AsEnumerable()
            //var myfaction = from score in this.dbContext.GameFactionModel.AsEnumerable() where score.username == HttpContext.User.Identity.Name select score.gameinfo_id;
            var list = from game in this.dbContext.GameInfoModel
                from score in this.dbContext.GameFactionModel
                where score.username == username && game.Id == score.gameinfo_id
                select game;
            var result = list.ToList();
            return View(result);
        }
        /// <summary>
        /// 显示详细
        /// </summary>
        /// <returns></returns>
        public IActionResult Show()
        {
            return View();
        }
        /// <summary>
        /// 个人使用的种族信息
        /// </summary>
        /// <returns></returns>
        public IActionResult FactionList(string username)
        {
            if (username == null)
            {
                username = HttpContext.User.Identity.Name;
            }
            var gameFactionModels = this.dbContext.GameFactionModel.Where(item => item.username == username).ToList();
            return View(gameFactionModels);
        }
    }
}
