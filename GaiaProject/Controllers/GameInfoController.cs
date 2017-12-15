using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GaiaCore.Gaia;
using GaiaCore.Gaia.Game;
using GaiaDbContext.Models;
using GaiaDbContext.Models.HomeViewModels;
using GaiaProject.Data;
using GaiaProject.Models.HomeViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GaiaProject.Controllers
{
    public partial class GameInfoController : Controller
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
        /// 进行的游戏
        /// </summary>
        /// <returns></returns>
        public IActionResult Index(string username,int? status,int? isAdmin)
        {

            if (username == null)
            {
                username = HttpContext.User.Identity.Name;
            }
            if (status == null)
            {
                status = 8;
            }
            IQueryable<GameInfoModel> list;
            //this.dbContext.GameInfoModel.AsEnumerable()
            //var myfaction = from score in this.dbContext.GameFactionModel.AsEnumerable() where score.username == HttpContext.User.Identity.Name select score.gameinfo_id;
            //未结束
            if (status != 8)
            {
                ViewBag.Title = "未结束游戏";
                list = from game in this.dbContext.GameInfoModel
                    where game.GameStatus == status 
                    select game;
            }
            else
            {
                ViewBag.Title = "已结束游戏";
                //如果是管理员查看全部游戏结果
                if (isAdmin == 1)
                {
                    if (this._userManager.GetUserAsync(User).Result.groupid == 1)
                    {
                        list = from game in this.dbContext.GameInfoModel where game.GameStatus == status select game;
                    }
                    else
                    {
                        return View(null);
                    }
                }
                else
                {
                    list = from game in this.dbContext.GameInfoModel
                        from score in this.dbContext.GameFactionModel
                        where game.GameStatus == status && score.username == username && game.Id == score.gameinfo_id
                        select game;
                }

            }
            var result = list.ToList();
            return View(result);

        }
        /// <summary>
        /// 删除游戏
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DelGame(int id)
        {
            Models.Data.UserFriendController.JsonData jsonData = new Models.Data.UserFriendController.JsonData();
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                GameInfoModel gameInfoModel = this.dbContext.GameInfoModel.SingleOrDefault(item => item.Id == id);
                if (gameInfoModel != null)
                {
                    GameMgr.DeleteOneGame(gameInfoModel.name);
                    this.dbContext.GameInfoModel.Remove(gameInfoModel);
                    //删除下面的种族信息
                    this.dbContext.GameFactionModel.RemoveRange(this.dbContext.GameFactionModel.Where(item => item.gameinfo_id == gameInfoModel.Id).ToList());
                    this.dbContext.SaveChanges();
                    jsonData.info.state = 200;
                }
            }
            return new JsonResult(jsonData);
        }



        /// <summary>
        /// 使用的种族信息
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
        /// <summary>
        /// 种族统计
        /// </summary>
        /// <returns></returns>

        public IActionResult FactionStatistics()
        {
            var list = this.dbContext.GameFactionModel.GroupBy(item => item.FactionChineseName).Select(
                g=>new Models.Data.GameInfoController.StatisticsFaction ()
                {
                    ChineseName = g.Key,
                    count = g.Count(),
                    numberwin = g.Count(faction => faction.rank == 1),
                    winprobability = g.Count(faction => faction.rank == 1)* 100 / (g.Count()),
                    scoremin = g.Min(faction=>faction.scoreTotal),
                    scoremax = g.Max(faction => faction.scoreTotal),
                    scoremaxuser = g.OrderBy(faction => faction.scoreTotal).ToList()[0].username,
                    scoreavg = g.Sum(faction => faction.scoreTotal)/g.Count(),
                    
                }).ToList();
            return View(list);
        }

        public bool FinishGame(GaiaDbContext.Models.HomeViewModels.GameInfoModel gameInfoModel,GaiaGame result)
        {
            //实际上已经结束，但是数据库没有更新的
            if (result.GameStatus.stage == GaiaCore.Gaia.Stage.GAMEEND)
            {
                gameInfoModel.GameStatus = 8; //状态
                gameInfoModel.round = 7; //代表结束
                gameInfoModel.UserCount = result.Username.Length; //玩家数量
                gameInfoModel.endtime = DateTime.Now; //结束时间
                return true;
            }
            return false;
        }

        /// <summary>
        /// 从内存更新游戏
        /// </summary>
        /// <returns></returns>
        public IActionResult UpdateGame()
        {
            var list = GameMgr.GetAllGame();

            foreach (KeyValuePair<string, GaiaGame> keyValuePair in list)
            {
                var result = keyValuePair.Value;

                GaiaDbContext.Models.HomeViewModels.GameInfoModel gameInfoModel;
                gameInfoModel= this.dbContext.GameInfoModel.SingleOrDefault(item => item.name == keyValuePair.Key);
                //如果不存在
                bool isExist = true;
                if (gameInfoModel == null)
                {
                    isExist = false;
                    gameInfoModel =
                        new GaiaDbContext.Models.HomeViewModels.GameInfoModel()
                        {
                            name = keyValuePair.Key,
                            userlist = string.Join("|", result.Username),
                            UserCount = result.Username.Length,
                            MapSelction = keyValuePair.Value.MapSelection.ToString(),
                            IsTestGame = keyValuePair.Value.IsTestGame ? 1 : 0,
                            GameStatus = 0,
                            starttime = DateTime.Now,
                            endtime = DateTime.Now,
                            round = 0,

                            //username = HttpContext.User.Identity.Name,
                        };
                    this.FinishGame(gameInfoModel, result);
                }
                else
                {
                    //结束的游戏不需要更新
                    if (gameInfoModel.GameStatus == 8)
                    {
                        continue;
                    }
                    else
                    {
                        gameInfoModel.round = result.GameStatus.RoundCount;
                        this.FinishGame(gameInfoModel, result);
                    }
                }

                gameInfoModel.ATTList = string.Join("|", result.ATTList.Select(item => item.name));
                gameInfoModel.FSTList = string.Join("|", result.FSTList.Select(item => item.GetType().Name));
                gameInfoModel.RBTList = string.Join("|", result.RBTList.Select(item => item.name));
                gameInfoModel.RSTList = string.Join("|", result.RSTList.Select(item => item.GetType().Name));
                gameInfoModel.STT3List = string.Join("|",
                    result.STT3List.GroupBy(item => item.name).Select(g => g.Max(item => item.name)));
                gameInfoModel.STT6List = string.Join("|",
                    result.STT6List.GroupBy(item => item.name).Select(g => g.Max(item => item.name)));
                gameInfoModel.scoreFaction =
                    string.Join(":",
                        result.FactionList.OrderBy(item => item.Score)
                            .Select(item => string.Format("{0}{1}({2})", item.ChineseName,
                                item.Score, item.UserName))); //最后的得分情况
                gameInfoModel.loginfo = string.Join("|", result.LogEntityList.Select(item => item.Syntax));

                if (isExist)
                {
                    this.dbContext.GameInfoModel.Update(gameInfoModel);

                }
                else
                {
                    this.dbContext.GameInfoModel.Add(gameInfoModel);
                }
            }
            this.dbContext.SaveChanges();

            return Redirect("/GameInfo/Index?status=0");
        }
        /// <summary>
        /// 从数据库日志更新游戏
        /// </summary>
        /// <returns></returns>
        public string UpdateGameFromDb()
        {
            List<GameInfoModel> list = this.dbContext.GameInfoModel.Where(item => item.GameStatus != 8).ToList();
            foreach (GameInfoModel gameInfoModel in list)
            {
                //如果日志不是空的
                if (!string.IsNullOrEmpty(gameInfoModel.loginfo))
                {
                    GameMgr.CreateNewGame(gameInfoModel.name, gameInfoModel.userlist.Split('|'), out GaiaGame result, gameInfoModel.MapSelction, isTestGame: gameInfoModel.IsTestGame == 1 ? true : false);
                    GaiaGame gg = GameMgr.GetGameByName(gameInfoModel.name);
                    gg.GameName = gameInfoModel.name;
                    gg.UserActionLog = gameInfoModel.loginfo.Replace("|", "\r\n");

                    gg = GameMgr.RestoreGame(gameInfoModel.name, gg);
                    //是否应该结束
                    if (this.FinishGame(gameInfoModel, gg))
                    {
                        this.dbContext.GameInfoModel.Update(gameInfoModel);
                        //没有找到任何的种族信息
                        if (!this.dbContext.GameFactionModel.Any(item => item.gameinfo_id == gameInfoModel.Id))
                        {
                            //保存种族信息
                            GameSave.SaveFactionToDb(this.dbContext, gg, gameInfoModel);
                        }
                    }


                }
            }
            this.dbContext.SaveChanges();
            return "success";
        }
    }
}
