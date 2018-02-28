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
using Microsoft.EntityFrameworkCore.Query.Expressions;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GaiaProject.Controllers
{
    public partial class GameInfoController : Controller
    {
        public class FactionListInfo
        {
            /// <summary>
            /// 种族信息
            /// </summary>
            public List<GameFactionModel> ListGameFaction;
            /// <summary>
            /// 种族统计
            /// </summary>
            public List<Models.Data.GameInfoController.StatisticsFaction> ListStatisticsFaction;

        }

        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public GameInfoController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this._userManager = userManager;

        }

        #region 查看和删除游戏
        // GET: /<controller>/
        /// <summary>
        /// 进行的游戏
        /// </summary>
        /// <returns></returns>
        public IActionResult Index(string username, int? isAdmin, GameInfoModel gameInfoModel, string joinname = null, int? scoremin = null, int pageindex = 1)
        {

            if (username == null)
            {
                username = HttpContext.User.Identity.Name;
            }
            IQueryable<GameInfoModel> list;

            ViewBag.Title = "游戏列表";
            if (isAdmin == null)//自己的游戏
            {
                list = from game in this.dbContext.GameInfoModel
                    from score in this.dbContext.GameFactionModel
                    where score.username == username && game.Id == score.gameinfo_id
                    select game;
            }
            else if (isAdmin == 3)//管理游戏
            {
                list = this.dbContext.GameInfoModel.Where(item => item.username == username && item.GameStatus == 0);
            }
            else
            {
                //如果是管理员查看全部游戏结果
                if (isAdmin == 1)
                {
                    if (this._userManager.GetUserAsync(User).Result.groupid == 1)
                    {
                        list = from game in this.dbContext.GameInfoModel select game;
                    }
                    else
                    {
                        return View(null);
                    }

                }
                //查看其他人的游戏
                else if (isAdmin == 2)
                {
                    list = from game in this.dbContext.GameInfoModel
                        //from score in this.dbContext.GameFactionModel
                        where game.IsAllowLook
                        select game;
                }
                else
                {
                    list = from game in this.dbContext.GameInfoModel
                        from score in this.dbContext.GameFactionModel
                        where score.username == username && game.Id == score.gameinfo_id
                        select game;
                }
                //名称
                if (gameInfoModel.name != null)
                {
                    list = list.Where(item => item.name == gameInfoModel.name);
                }
                //回合
                if (gameInfoModel.round != null)
                {
                    list = list.Where(item => item.round == gameInfoModel.round);
                }
                //创建人
                list = gameInfoModel.username == null
                    ? list
                    : list.Where(item => item.username == gameInfoModel.username);
                //参加人
                if (joinname != null)
                {
                    list = from game in list
                        from score in this.dbContext.GameFactionModel
                        where score.username == joinname && game.Id == score.gameinfo_id
                        select game;
                }
                //最低分
                if (scoremin != null)
                {
                    list = from game in list
                        from score in this.dbContext.GameFactionModel
                        where score.scoreTotal <= scoremin && game.Id == score.gameinfo_id
                        select game;
                }
            }



            //状态
            list = gameInfoModel.GameStatus == null
                ? list
                : list.Where(item => item.GameStatus == gameInfoModel.GameStatus);

            //pageindex = pageindex == 0 ? 1 : pageindex;
            list = list.OrderByDescending(item => item.starttime).Skip(30 * (pageindex - 1)).Take(30);

            var result = list.ToList();
            return View(result);

        }
        /// <summary>
        /// 申请删除列表
        /// </summary>
        /// <returns></returns>
        public IActionResult ApplyDeleteList()
        {
            var result = this.dbContext.GameDeleteModel.Where(item => item.username == User.Identity.Name).ToList();
            return View(result);
        }

        /// <summary>
        /// 提交删除游戏申请
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
                    GaiaGame gaiaGame = GameMgr.GetGameByName(gameInfoModel.name);
                    foreach (string username in gaiaGame.Username)
                    {
                        //如果不是自己
                        if (username != user.UserName)
                        {
                            this.dbContext.GameDeleteModel.Add(new GameDeleteModel()
                            {
                                gameinfo_id = gameInfoModel.Id,
                                gameinfo_name = gameInfoModel.name,
                                username = username,
                                state = 0,
                            });
                        }
                    }
                    gameInfoModel.isDelete = 1;
                    this.dbContext.SaveChanges();
                    jsonData.info.state = 200;
                }
            }
            return new JsonResult(jsonData);
        }
        /// <summary>
        /// 同意删除游戏
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]

        public async Task<JsonResult> AgreeDelGame(int id,int type)
        {
            Models.Data.UserFriendController.JsonData jsonData = new Models.Data.UserFriendController.JsonData();
            GameDeleteModel gameDeleteModel = this.dbContext.GameDeleteModel.SingleOrDefault(
                item => item.Id == id && item.username == User.Identity.Name);
            if (gameDeleteModel != null)
            {
                GameInfoModel gameInfoModel = this.dbContext.GameInfoModel.SingleOrDefault(item => item.Id == gameDeleteModel.gameinfo_id);
                //申请删除状态
                if (gameInfoModel.isDelete == 1)
                {
                    {
                        //同意删除
                        if (type == 1)
                        {
                            //删除同意记录
                            this.dbContext.GameDeleteModel.Remove(gameDeleteModel);
                            this.dbContext.SaveChanges();
                            //如果是最后一个同意的，则删除游戏
                            if (!this.dbContext.GameDeleteModel.Any(item => item.gameinfo_id == gameDeleteModel.gameinfo_id))
                            {
                                this.DeleteDbGame(gameDeleteModel.gameinfo_id);
                                jsonData.info.message = "全部玩家同意删除，已删除游戏";
                            }
                            else
                            {
                                jsonData.info.message = "继续等待其他玩家处理删除申请";
                            }
                        }
                        else//不同意删除
                        {
                            //移除全部的申请
                            this.dbContext.GameDeleteModel.RemoveRange(this.dbContext.GameDeleteModel.Where(item => item.gameinfo_id == gameDeleteModel.gameinfo_id));
                            //申请删除游戏恢复
                            GameInfoModel infoModel = this.dbContext.GameInfoModel.SingleOrDefault(item => item.Id == gameDeleteModel.gameinfo_id);
                            if (infoModel != null)
                            {
                                infoModel.isDelete = 0;
                                this.dbContext.GameInfoModel.Update(infoModel);
                                jsonData.info.message = "游戏已经恢复正常状态";
                            }
                        }
                        this.dbContext.SaveChanges();
                    }
                    jsonData.info.state = 200;
                }
            }

            return new JsonResult(jsonData);
        }

        /// <summary>
        /// 数据库删除游戏
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool  DeleteDbGame(int id)
        {
            var user =  _userManager.GetUserAsync(HttpContext.User);
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
                    return true;
                }
            }
            return false;
        }
        [HttpPost]
        public async Task<JsonResult> DeleteDbGame(int id,bool isdel=false)
        {
            Models.Data.UserFriendController.JsonData jsonData = new Models.Data.UserFriendController.JsonData();

            bool flag = DeleteDbGame(id);
            if (flag)
            {
                jsonData.info.state = 200;
            }
            return new JsonResult(jsonData);
        }


        #endregion


        #region 种族玩家统计
        /// <summary>
        /// 个人使用的种族信息
        /// </summary>
        /// <returns></returns>
        public IActionResult FactionList(GameFactionModel gameFactionModel, int? type, int? usercount,int orderType=1, int pageindex = 1)
        {
            //gameFactionModel.UserCount
            if (gameFactionModel.username == null)
            {
                gameFactionModel.username = HttpContext.User.Identity.Name;
            }
            IQueryable<GameFactionModel> gameFactionModels;
            if (type == 1)//全部
            {
                gameFactionModels = this.dbContext.GameFactionModel.AsQueryable();

            }
            else if (type == 2)//高分
            {
                gameFactionModels = this.dbContext.GameFactionModel.AsQueryable();
            }
            else//自己的
            {
                gameFactionModels = this.dbContext.GameFactionModel.Where(item => item.username == gameFactionModel.username);
            }

            //种族
            if (gameFactionModel.FactionName != null)
            {
                gameFactionModels = gameFactionModels.Where(item => item.FactionName == gameFactionModel.FactionName);
            }

            //人数
            usercount = usercount ?? 4;
            if (usercount > 0)
            {
                gameFactionModels = gameFactionModels.Where(item => item.UserCount == usercount);
            }
            List<Models.Data.GameInfoController.StatisticsFaction> list = null;

            //不是高分统计
            if (type != 2)
            {
                if (gameFactionModels.Any())
                {
                    //种族统计和平均分
                    list = this.GetFactionStatistics(gameFactionModels, usercount, HttpContext.User.Identity.Name);
                    //全部的平均分
                    Models.Data.GameInfoController.StatisticsFaction allavg = gameFactionModels
                        .GroupBy(item => item.username).Select(
                            g => new Models.Data.GameInfoController.StatisticsFaction()
                            {
                                ChineseName = "全部",
                                count = g.Count(),
                                numberwin = g.Count(faction => faction.rank == 1),
                                winprobability = g.Count(faction => faction.rank == 1) * 100 / (g.Count()),
                                scoremin = g.Min(faction => faction.scoreTotal),
                                scoremax = g.Max(faction => faction.scoreTotal),
                                scoremaxuser = g.OrderByDescending(faction => faction.scoreTotal).ToList()[0].username,
                                scoreavg = g.Sum(faction => faction.scoreTotal) / g.Count(),
                                OccurrenceRate = 100,

                            })?.ToList()[0];
                    list.Add(allavg);
                }

            }

            //list = list.OrderByDescending(item => item.starttime).Skip(30 * (pageindex - 1)).Take(30);


            if (type == null)
            {
                //时间
                if (orderType == 1)
                {
                    gameFactionModels = gameFactionModels.OrderByDescending(item => item.Id).Skip(30 * (pageindex - 1)).Take(30);

                }
                else if (orderType==2)//总分
                {
                    gameFactionModels = gameFactionModels.OrderByDescending(item => item.scoreTotal).Skip(30 * (pageindex - 1)).Take(30);

                }
                else if (orderType == 3)//裸分
                {
                    gameFactionModels = gameFactionModels.OrderByDescending(item => item.scoreLuo).Skip(30 * (pageindex - 1)).Take(30);

                }
            }
            else
            {
                //前30条
                if (type != 1)
                {
                    gameFactionModels = gameFactionModels.OrderByDescending(item => item.scoreTotal).Skip(30 * (pageindex - 1)).Take(30);
                }
                else
                {
                    gameFactionModels = gameFactionModels.OrderByDescending(item => item.scoreTotal);
                }
            }

            //赋值model
            FactionListInfo factionListInfo = new FactionListInfo();
            factionListInfo.ListGameFaction = gameFactionModels.ToList();
            factionListInfo.ListStatisticsFaction = list ?? new List<Models.Data.GameInfoController.StatisticsFaction>();
            //gameFactionModels = gameFactionModels.OrderByDescending(item => item.scoreTotal);
            return View(factionListInfo);
        }
        /// <summary>
        /// 获取种族统计
        /// </summary>
        /// <param name="usercount"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        private List<Models.Data.GameInfoController.StatisticsFaction> GetFactionStatistics(IQueryable<GameFactionModel> query, int? usercount, string username, int? orderType = null)
        {
            //IQueryable<GameFactionModel> query;
            if (query == null)
            {
                if (!string.IsNullOrEmpty(username))
                {
                    query = this.dbContext.GameFactionModel.Where(item => item.username == username);
                }
                else
                {
                    query = this.dbContext.GameFactionModel.AsQueryable();
                }
                usercount = usercount ?? 4;
                if (usercount > 0)
                {
                    //query = query.Where(item => this.dbContext.GameInfoModel.Where(game => game.GameStatus == 8 && game.UserCount == usercount).Select(game=>game.Id).Contains(item.gameinfo_id) );
                    query = query.Where(item => item.UserCount == usercount);
                }
            }
            //总场次
            int total = query.Count();
            //如果不是玩家的出场率
            if (string.IsNullOrEmpty(username))
            {
                if (usercount > 0)
                {
                    total = total / (int)usercount;
                }
                else
                {
                    total = this.dbContext.GameInfoModel.Count(item => item.GameStatus == 8);
                }
            }

            var list = query.GroupBy(item => item.FactionChineseName).Select(
                g => new Models.Data.GameInfoController.StatisticsFaction()
                {
                    ChineseName = g.Key,
                    count = g.Count(),
                    numberwin = g.Count(faction => faction.rank == 1),
                    winprobability = g.Count(faction => faction.rank == 1) * 100 / (g.Count()),
                    scoremin = g.Min(faction => faction.scoreTotal),
                    scoremax = g.Max(faction => faction.scoreTotal),
                    scoremaxuser = g.OrderByDescending(faction => faction.scoreTotal).ToList()[0].username,
                    scoreavg = g.Sum(faction => faction.scoreTotal) / g.Count(),
                    OccurrenceRate = g.Count() * 100 / total,
                });
            if (orderType != null)
            {
                switch (orderType)
                {
                    case 1:
                        list = list.OrderByDescending(item => item.count);
                        break;
                    case 2:
                        list = list.OrderByDescending(item => item.scoreavg);
                        break;
                    case 3:
                        list = list.OrderByDescending(item => item.numberwin);
                        break;
                    case 4:
                        list = list.OrderByDescending(item => item.winprobability);
                        break;
                }
            }
            else
            {
                list = list.OrderByDescending(item => item.count);
            }
            return list.ToList();
        }
        /// <summary>
        /// 种族统计
        /// </summary>
        /// <returns></returns>

        public IActionResult FactionStatistics(int? usercount, string username, int? orderType)
        {
            var list = this.GetFactionStatistics(null, usercount, username, orderType);
            return View(list);
        }

        public IActionResult FactionStatisticsChart(int? usercount, string username, int? orderType)
        {
            var list = this.GetFactionStatistics(null, usercount, username, orderType);
            return View(list);
        }

        /// <summary>
        /// 玩家的平均分
        /// </summary>
        public IActionResult UserScoreAvg(GameFactionModel gameFactionModel, int? orderType, int usercount = 4)
        {
            IQueryable<GameFactionModel> query;
            //90一下的不纳入统计
            query = this.dbContext.GameFactionModel.Where(item => item.scoreTotal > 90);
            //人数
            if (usercount > 0)
            {
                query = query.Where(item => item.UserCount == usercount);
            }
            //查询种族
            if (gameFactionModel.FactionName != null)
            {
                query = query.Where(item => item.FactionName == gameFactionModel.FactionName);
            }


            var list = query.GroupBy(item => item.username).Select(
                g => new Models.Data.GameInfoController.StatisticsFaction()
                {
                    ChineseName = g.Key,
                    count = g.Count(),
                    numberwin = g.Count(faction => faction.rank == 1),
                    winprobability = g.Count(faction => faction.rank == 1) * 100 / (g.Count()),
                    scoremin = g.Min(faction => faction.scoreTotal),
                    scoremax = g.Max(faction => faction.scoreTotal),
                    scoremaxuser = g.OrderByDescending(faction => faction.scoreTotal).ToList()[0].username,
                    scoreavg = g.Sum(faction => faction.scoreTotal) / g.Count(),

                });
            //场次要大于3场
            list = list.Where(item => item.count > 2);
            //排序方式
            if (orderType != null)
            {
                switch (orderType)
                {
                    case 1:
                        list = list.OrderByDescending(item => item.count);
                        break;
                    case 2:
                        list = list.OrderByDescending(item => item.scoreavg);
                        break;
                    case 3:
                        list = list.OrderByDescending(item => item.numberwin);
                        break;
                    case 4:
                        list = list.OrderByDescending(item => item.winprobability);
                        break;
                }
            }
            else
            {
                list = list.OrderByDescending(item => item.count);
            }
            return View(list.ToList());
        }

        #endregion


        #region 科技版统计

//        public class TTModel
//        {
//            public string name { get; set; }
//            public int count { get; set; }
//
//
//        }

        public IActionResult TTStatistics()
        {
            //储存
            GameFactionExtendModel gameFactionExtendModel =new GameFactionExtendModel();

            //高级科技
            gameFactionExtendModel.ATT1 = (short) this.dbContext.GameFactionExtendModel.Count(item => item.ATT1 > 0);
            gameFactionExtendModel.ATT2 = (short)this.dbContext.GameFactionExtendModel.Count(item => item.ATT2 > 0);
            gameFactionExtendModel.ATT3 = (short)this.dbContext.GameFactionExtendModel.Count(item => item.ATT3 > 0);
            gameFactionExtendModel.ATT4 = (short)this.dbContext.GameFactionExtendModel.Count(item => item.ATT4 > 0);
            gameFactionExtendModel.ATT5 = (short)this.dbContext.GameFactionExtendModel.Count(item => item.ATT5 > 0);
            gameFactionExtendModel.ATT6 = (short)this.dbContext.GameFactionExtendModel.Count(item => item.ATT6 > 0);
            gameFactionExtendModel.ATT7 = (short)this.dbContext.GameFactionExtendModel.Count(item => item.ATT7 > 0);
            gameFactionExtendModel.ATT8 = (short)this.dbContext.GameFactionExtendModel.Count(item => item.ATT8 > 0);
            gameFactionExtendModel.ATT9 = (short)this.dbContext.GameFactionExtendModel.Count(item => item.ATT9 > 0);
            gameFactionExtendModel.ATT10 = (short)this.dbContext.GameFactionExtendModel.Count(item => item.ATT10 > 0);
            gameFactionExtendModel.ATT11 = (short)this.dbContext.GameFactionExtendModel.Count(item => item.ATT11 > 0);
            gameFactionExtendModel.ATT12 = (short)this.dbContext.GameFactionExtendModel.Count(item => item.ATT12 > 0);
            gameFactionExtendModel.ATT13 = (short) this.dbContext.GameFactionExtendModel.Count(item => item.ATT13 > 0);
            gameFactionExtendModel.ATT14 = (short)this.dbContext.GameFactionExtendModel.Count(item => item.ATT14 > 0);
            gameFactionExtendModel.ATT15 = (short)this.dbContext.GameFactionExtendModel.Count(item => item.ATT15 > 0);
            //低级科技
            gameFactionExtendModel.STT1 = (short)this.dbContext.GameFactionExtendModel.Count(item => item.STT1 > 0);
            gameFactionExtendModel.STT2 = (short)this.dbContext.GameFactionExtendModel.Count(item => item.STT2 > 0);
            gameFactionExtendModel.STT3 = (short)this.dbContext.GameFactionExtendModel.Count(item => item.STT3 > 0);
            gameFactionExtendModel.STT4 = (short)this.dbContext.GameFactionExtendModel.Count(item => item.STT4 > 0);
            gameFactionExtendModel.STT5 = (short)this.dbContext.GameFactionExtendModel.Count(item => item.STT5 > 0);
            gameFactionExtendModel.STT6 = (short)this.dbContext.GameFactionExtendModel.Count(item => item.STT6 > 0);
            gameFactionExtendModel.STT7 = (short)this.dbContext.GameFactionExtendModel.Count(item => item.STT7 > 0);
            gameFactionExtendModel.STT8 = (short)this.dbContext.GameFactionExtendModel.Count(item => item.STT8 > 0);
            gameFactionExtendModel.STT9 = (short)this.dbContext.GameFactionExtendModel.Count(item => item.STT9 > 0);

            //IQueryable<TTModel> ttModels = this.dbContext.GameFactionExtendModel.Where(item => item.ATT1 > 0).Select(item=>item);
            //                (g => new TTModel
            //            {
            //                count = g.Sum(i => i.ATT1),
            //                name = "ATT1"
            //            });
            return View(gameFactionExtendModel);
        }

        #endregion

        #region 操作内存和数据库游戏


        public bool FinishGame(GaiaDbContext.Models.HomeViewModels.GameInfoModel gameInfoModel, GaiaGame result)
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
                var listG = this.dbContext.GameInfoModel.Where(item => item.name == keyValuePair.Key).OrderByDescending(item => item.starttime).ToList();
                if (listG.Count > 0)
                {
                    gameInfoModel = listG[0];
                }
                else
                {
                    continue;
                }
                //如果不存在
                bool isExist = true;
                if (gameInfoModel == null)
                {
                    //测试游戏跳过
                    if (result.IsTestGame)
                    {
                        continue;
                    }
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
                        //如果游戏结束
                        if (this.FinishGame(gameInfoModel, result))
                        {
                            //并且没有找到任何的种族信息
                            if (!this.dbContext.GameFactionModel.Any(item => item.gameinfo_id == gameInfoModel.Id))
                            {
                                //保存种族信息
                                DbGameSave.SaveFactionToDb(this.dbContext, result, gameInfoModel);
                            }
                        }
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
                        result.FactionList.OrderByDescending(item => item.Score)
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
        public string UpdatenNoFromDb()
        {
            return UpdateFromDb(item => item.GameStatus != 8);
        }

        /// <summary>
        /// 检测游戏结束但没有种族信息的
        /// </summary>
        /// <returns></returns>
        public string UpdatenFinishFromDb()
        {
            
            return UpdateFromDb(item => item.GameStatus == 8 && item.saveState==0);
        }

        public string UpdateFromDb(Func<GameInfoModel, bool> func)
        {
            List<GameInfoModel> list = this.dbContext.GameInfoModel.Where(func).ToList();//item => item.GameStatus != 8
            foreach (GameInfoModel gameInfoModel in list)
            {
                //如果日志不是空的
                if (!string.IsNullOrEmpty(gameInfoModel.loginfo))
                {
                    GameMgr.CreateNewGame(gameInfoModel.name, gameInfoModel.userlist.Split('|'), out GaiaGame result, gameInfoModel.MapSelction, isTestGame: gameInfoModel.IsTestGame == 1 ? true : false);
                    GaiaGame gg = GameMgr.GetGameByName(gameInfoModel.name);
                    //gg.dbContext = this.dbContext;
                    gg.GameName = gameInfoModel.name;

                    if (gameInfoModel.saveState == 0)
                    {
                        gg.dbContext = this.dbContext;
                        gg.IsSaveToDb = true;

                        gameInfoModel.saveState = 1;
                    }


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
                            DbGameSave.SaveFactionToDb(this.dbContext, gg, gameInfoModel);
                        }
                        else
                        {
                            //总局计分问题，需要重新计算
#if  DEBUG
                            DbGameSave.SaveFactionToDb(this.dbContext, gg, gameInfoModel);
#endif
                        }
                    }

                    GameMgr.DeleteOneGame(gameInfoModel.name);
                }
            }
            this.dbContext.SaveChanges();
            return "success";
        }
        /// <summary>
        /// 删除游戏未结束，但是内存没有的游戏
        /// </summary>
        /// <returns></returns>

        public string DeleteGameInvalid()
        {
            List<GameInfoModel> list = this.dbContext.GameInfoModel.Where(item => item.GameStatus != 8).ToList();//item => item.GameStatus != 8
            foreach (GameInfoModel gameInfoModel in list)
            {
                if (gameInfoModel.GameStatus != 8)
                {
                    //内存没有
                    if (GameMgr.GetGameByName(gameInfoModel.name) == null)
                    {
#if  !DEBUG
                        this.DelGame(id: gameInfoModel.Id);
#endif
                    }
                }
            }
            return "success";
        }

        #endregion

    }
}
