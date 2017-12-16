using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GaiaCore.Gaia;
using GaiaProject.Models.HomeViewModels;
using Microsoft.AspNetCore.Identity;
using GaiaProject.Models;
using GaiaCore.Gaia.User;
using ManageTool;
using GaiaDbContext.Models;
using GaiaProject.Data;
using GaiaDbContext.Models.HomeViewModels;

namespace GaiaProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext dbContext;

        public HomeController(ApplicationDbContext dbContext,UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            this.dbContext = dbContext;
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
                ViewData["ServerStartTime"] = ServerStatus.ServerStartTime;
            }
#if DEBUG
            //ViewData["Message"] = @"yucenyucen@126.com";
            //ViewData["GameList"] = GameMgr.GetAllGame(@"yucenyucen@126.com");
#endif

            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult ServerDown()
        {
            return View();
        }
        /// <summary>
        /// 随机排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ListT"></param>
        /// <returns></returns>
        public List<T> RandomSortList<T>(T[] ListT)
        {
            Random random = new Random();
            List<T> newList = new List<T>();
            foreach (T item in ListT)
            {
                //空的就跳过
                if (item == null || string.IsNullOrEmpty(item.ToString()))
                {
                    continue;
                }
                newList.Insert(random.Next(newList.Count + 1), item);
            }
            return newList;
        }

        // POST: /Home/NewGame
        [HttpPost]
        public IActionResult NewGame(NewGameViewModel model)
        {
            if (ServerStatus.IsStopSyntax == true)
            {
                return Redirect("/home/serverdown");
            }
            if (string.IsNullOrEmpty(model.Name))
            {
                model.Name = Guid.NewGuid().ToString();
            }
            string[] username = new string[] { model.Player1, model.Player2, model.Player3, model.Player4 };
            if (this.dbContext.GameInfoModel.Any(item => item.name == model.Name))
            {
                ModelState.AddModelError(string.Empty,  "游戏名称已经存在");
                return View(model);

            }
            //随机排序
            username = this.RandomSortList<string>(username).ToArray();
            foreach (var item in username.Where(x=>!string.IsNullOrEmpty(x)))
            {
                var user=_userManager.FindByNameAsync(item);
                if (user.Result==null)
                {
                    ModelState.AddModelError(string.Empty, item + "用户不存在");
                    return View(model);
                }
            }
            //创建
            bool create = GameMgr.CreateNewGame(model.Name, username, out GaiaGame result,model.MapSelction, isTestGame: model.IsTestGame);
            if (create)
            {
                //保存到数据库
                GaiaDbContext.Models.HomeViewModels.GameInfoModel gameInfoModel =
                    new GaiaDbContext.Models.HomeViewModels.GameInfoModel()
                    {
                        name = model.Name,
                        userlist = string.Join("|", username),
                        UserCount = username.Length,
                        MapSelction = model.MapSelction,
                        IsTestGame = model.IsTestGame?1:0,
                        GameStatus = 0,
                        starttime = DateTime.Now,
                        endtime = DateTime.Now,
                        username = HttpContext.User.Identity.Name,
                    };
                //配置信息
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
                                "", item.UserName))); //最后的得分情况
                this.dbContext.GameInfoModel.Add(gameInfoModel);
                this.dbContext.SaveChanges();

            }

            ViewData["ReturnUrl"] = "/Home/ViewGame/" + model.Name;
            return Redirect("/home/viewgame/" + System.Net.WebUtility.UrlEncode(model.Name));
        }
        // GET: /Home/NewGame
        [HttpGet]
        public IActionResult NewGame()
        {
            var task = _userManager.GetUserAsync(HttpContext.User);
            Task[] taskarray = new Task[] { task };
            Task.WaitAll(taskarray, millisecondsTimeout: 1000);
            if (task.Result != null)
            {
                ViewData["Message"] = task.Result.UserName;
                ViewData["GameList"] = GameMgr.GetAllGameName(task.Result.UserName);
            }
            return View();
        }

        public IActionResult ViewGame(string id)
        {
            ViewData["Title"] = "viewgame";
            var gg = GameMgr.GetGameByName(id);
            if (gg == null)
            {
                return Redirect("/home/index");
            }
            ViewData["gameid"] = id;
            return View(gg);
        }
        /// <summary>
        /// 还原游戏
        /// </summary>
        public IActionResult RestoreGame(int id)
        {
            //游戏结果
            GameInfoModel gameInfoModel = this.dbContext.GameInfoModel.SingleOrDefault(item => item.Id == id);
            if (gameInfoModel != null)
            {
                string log;
                //游戏已经结束
                if (gameInfoModel.GameStatus == 8)
                {
                    log = gameInfoModel.loginfo;
                }
                else
                {
                    var game = GameMgr.GetGameByName(gameInfoModel.name);
                    if (game == null)//游戏不存在
                    {
                        if (string.IsNullOrEmpty(gameInfoModel.loginfo))
                        {
                            return View("Index");
                        }
                        log = gameInfoModel.loginfo;
                    }
                    else
                    {
                        return Redirect("/Home/ViewGame/"+ gameInfoModel.name);
                    }
                    //log = game.UserActionLog;
                }

                GameMgr.CreateNewGame(gameInfoModel.name, gameInfoModel.userlist.Split('|'), out GaiaGame result, gameInfoModel.MapSelction, isTestGame: gameInfoModel.IsTestGame == 1 ? true : false);
                GaiaGame gg = GameMgr.GetGameByName(gameInfoModel.name);
                gg.GameName = gameInfoModel.name;
                gg.UserActionLog = log.Replace("|", "\r\n");

                gg = GameMgr.RestoreGame(gameInfoModel.name, gg);
                //从内存删除
                GameMgr.DeleteOneGame(gameInfoModel.name);

                return View("ViewGame", gg);

                //return Redirect("/home/View/" + gameInfoModel.name);
            }
            return null;
            //GameMgr.UndoOneStep(id);
        }

        [HttpPost]
        public IActionResult ViewGame(string name, string syntax, string factionName)
        {
            ViewData["Title"] = "viewgame";
            var task = _userManager.GetUserAsync(HttpContext.User);
            Task[] taskarray = new Task[] { task };
            Task.WaitAll(taskarray, millisecondsTimeout: 1000);
            if (task.Result != null)
            {

                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(syntax))
                {
                    return Redirect("/home/index");
                }
                if (GameMgr.GetGameByName(name) == null)
                {
                    Redirect("/home/index");
                }

                if (!string.IsNullOrEmpty(factionName))
                {
                    syntax = string.Format("{0}:{1}", factionName, syntax);
                }
                GameMgr.GetGameByName(name).Syntax(syntax, out string log, task.Result.UserName);

                if (!string.IsNullOrEmpty(log))
                {
                    ModelState.AddModelError(string.Empty, log);
                }
                return View(GameMgr.GetGameByName(name));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "没有获取到用户名");
                return View(GameMgr.GetGameByName(name));
            }
        }



        [HttpPost]
        public string Syntax(string name, string syntax, string factionName)
        {
            if (ServerStatus.IsStopSyntax == true)
            {
                return "error:" + "服务器维护中";
            }
            var task = _userManager.GetUserAsync(HttpContext.User);
            Task[] taskarray = new Task[] { task };
            Task.WaitAll(taskarray, millisecondsTimeout: 1000);
            if (task.Result != null)
            {

                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(syntax))
                {
                    return "error:空语句";
                }

                if (!string.IsNullOrEmpty(factionName))
                {
                    syntax = string.Format("{0}:{1}", factionName, syntax);
                }
                GameMgr.GetGameByName(name).Syntax(syntax, out string log, task.Result.UserName,dbContext);

                if (!string.IsNullOrEmpty(log))
                {
                    return "error:" + log;
                }
                else
                {
                    return "ok";
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "没有获取到用户名");
                return "error:未登入用户";
            }
        }

        [HttpPost]
        public IActionResult LeechPower(string name, FactionName factionName, int power, FactionName leechFactionName, bool isLeech,bool? isPwFirst)
        {
            if (ServerStatus.IsStopSyntax == true)
            {
                return Redirect("/home/serverdown");
            }
            var faction = GameMgr.GetGameByName(name).FactionList.Find(x => x.FactionName.ToString().Equals(name));
            var leech = isLeech ? "leech" : "decline";

            var syntax = string.Format("{0}:{1} {2} from {3}", factionName, leech, power, leechFactionName);
            if (isPwFirst.HasValue)
            {
                var pwFirst = isPwFirst.GetValueOrDefault() ? "pw" : "pwt";
                syntax = syntax + " " + pwFirst;
            }
            GameMgr.GetGameByName(name).Syntax(syntax, out string log);
            return Redirect("/home/viewgame/" + System.Net.WebUtility.UrlEncode(name));
        }

        public string GetLastestActionLog()
        {
            return GameMgr.GetLastestBackupData();
        }
        [HttpPost]
        public string GetNextGame(string name)
        {
            return GameMgr.GetNextGame(name);
        }

        /// <summary>
        /// 回退一步
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool UndoOneStep(string id)
        {
            if (!PowerUser.IsPowerUser(User.Identity.Name))
            {
                return false;
            }
            return GameMgr.UndoOneStep(id);
        }

        public bool ReportBug(string id)
        {
            if (!PowerUser.IsPowerUser(User.Identity.Name))
            {
                return false;
            }
            return GameMgr.ReportBug(id);
        }

        public bool RedoOneStep(string id)
        {
            if (!PowerUser.IsPowerUser(User.Identity.Name))
            {
                return false;
            }
            return GameMgr.RedoOneStep(id);
        }

        public bool DeleteOneGame(string id)
        {
            if (!PowerUser.IsPowerUser(User.Identity.Name))
            {
                return false;
            }
            return GameMgr.DeleteOneGame(id);
        }

        /// <summary>
        /// 获取操作日志
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> SyntaxLog(string id)
        {


            var gg = GameMgr.GetGameByName(id);
            StringBuilder stringBuilder=new StringBuilder();

            foreach(var item in gg.LogEntityList.OrderByDescending(x => x.Row))
            {
                stringBuilder.Append(string.Format("<tr><td>{0}</td><td class='text-right'>{1}</td><td>{2}vp</td><td class='text-right'>{3}</td><td>{4}c</td><td class='text-right'>{5}</td><td>{6}o</td><td class='text-right'>{7}</td><td>{8}q</td><td class='text-right'>{9}</td><td>{10}k</td><td class='text-right'>{11}</td><td>{12}/{13}/{14}</td><td>{15}</td></tr>", item.FactionName ?? null, @item.ResouceChange?.m_score, item.ResouceEnd?.m_score, item.ResouceChange?.m_credit, item.ResouceEnd?.m_credit, item.ResouceChange?.m_ore, item.ResouceEnd?.m_ore, item.ResouceChange?.m_QICs, item.ResouceEnd?.m_QICs, item.ResouceChange?.m_knowledge, item.ResouceEnd?.m_knowledge, item.ResouceChange?.m_powerToken2 + item.ResouceChange?.m_powerToken3 * 2, item.ResouceEnd?.m_powerToken1,item.ResouceEnd?.m_powerToken2,item.ResouceEnd?.m_powerToken3, item.Syntax));
            }

            return new JsonResult(new Models.Data.UserFriendController.JsonData()
            {
                data = stringBuilder.ToString(),info=new Models.Data.UserFriendController.Info() { state = 200}
            });
        }
        #region 管理工具
        public IActionResult BackupData()
        {
            ServerStatus.IsStopSyntax = true;
            GameMgr.BackupDictionary();
            return View();
        }
        public IActionResult RestoreData(string filename)
        {
            if (!PowerUser.IsPowerUser(User.Identity.Name))
            {
                return Redirect("/home/index");
            }
            ViewData["nameList"] = string.Join(",", GameMgr.RestoreDictionary(filename));
            return Redirect("/home/viewgame/" + "test01");
            //return View();
        }
        public IActionResult GetAllGame()
        {
            ViewData["GameList"] = GameMgr.GetAllGame();
            return View();
        }

        public IActionResult DeleteAllGame()
        {
            if (!PowerUser.IsPowerUser(User.Identity.Name))
            {
                return Redirect("/home/index");
            }
            var task = _userManager.GetUserAsync(HttpContext.User);
            Task[] taskarray = new Task[] { task };
            Task.WaitAll(taskarray, millisecondsTimeout: 1000);
            if ("yucenyucen@126.com".Equals(task.Result.UserName)){
                GameMgr.DeleteAllGame();
            }
            return Redirect("/home/index");
        }

        public IActionResult RestoreDataFromServer()
        {
            Task<IEnumerable<string>> task = GameMgr.RestoreDictionaryFromServerAsync();
            task.Wait();
            var ret = task.Result;
            ViewData["nameList"] = string.Join(",", ret);
            return Redirect("/home/getallgame");
        }
        [HttpGet]
        public IActionResult RestoreDataFromServerOneGame(string id)
        {
            Task<IEnumerable<string>> task = GameMgr.RestoreDictionaryFromServerAsync(id);
            task.Wait();
            return Redirect("/home/viewgame/" + System.Net.WebUtility.UrlEncode(id));
        }
        #endregion
    }
}
