using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gaia.Service;
using Microsoft.AspNetCore.Mvc;
using GaiaCore.Gaia;
using Microsoft.AspNetCore.Identity;
using GaiaCore.Gaia.User;
using ManageTool;
using GaiaDbContext.Models;
using GaiaDbContext.Models.AccountViewModels;
using GaiaProject.Data;
using GaiaDbContext.Models.HomeViewModels;
using GaiaDbContext.Models.SystemModels;
using GaiaProject.Models.HomeViewModels;
using GaiaProject.Notice;
using Microsoft.Extensions.Caching.Memory;

namespace GaiaProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext dbContext;
        private readonly IEmailSender _emailSender;
        public IMemoryCache cache;
        private readonly SignInManager<ApplicationUser> signInManager;
        public const string IndexName = "IndexName";

        public HomeController(ApplicationDbContext dbContext,UserManager<ApplicationUser> userManager, IEmailSender emailSender, IMemoryCache cache, SignInManager<ApplicationUser> signInManager)
        {
            this._userManager = userManager;
            this.dbContext = dbContext;
            this._emailSender = emailSender;
            this.cache = cache;
            this.signInManager = signInManager;
        }
        public IActionResult Index()
        {

            if (!signInManager.IsSignedIn(User))
            {
                return Redirect("/News/ShowInfo/3");
            }

            //AuthMessageSender.SendEmail("325153468@qq.com", "test", "sbs");

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
            //获取首页声明
            string remark;
            this.cache.TryGetValue(IndexName, out remark);
            if (string.IsNullOrEmpty(remark))
            {
                remark = this.dbContext.NewsInfoModel.SingleOrDefault(item => item.Id == 1)?.contents;
                this.cache.Set(IndexName, remark);
            }

            ViewBag.Cache = remark;

            return View();
        }

        public IActionResult About()
        {
            return View();
        }
        /// <summary>
        /// 种族图片
        /// </summary>
        /// <returns></returns>
        public IActionResult FactionImage()
        {
            return View();
        }
        /// <summary>
        /// 帮助页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Contact()
        {
            IQueryable<NewsInfoModel> newsInfoModels = this.dbContext.NewsInfoModel.Where(item => item.type == 2).OrderBy(item=>item.Rank);
            return View(newsInfoModels.ToList());
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

        #region 创建游戏

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
            //清除空格
            model.Name = model.Name.Trim();

            string[] username = new string[] { model.Player1, model.Player2, model.Player3, model.Player4 };
            if (this.dbContext.GameInfoModel.Any(item => item.name == model.Name) || GameMgr.GetGameByName(model.Name) != null)
            {
                ModelState.AddModelError(string.Empty, "游戏名称已经存在");
                return View(model);
            }
            //保存游戏信息
            Action<GaiaGame> saveGameInfo = (result) =>
            {
                //保存到数据库
                GaiaDbContext.Models.HomeViewModels.GameInfoModel gameInfoModel =
                    new GaiaDbContext.Models.HomeViewModels.GameInfoModel()
                    {
                        name = model.Name,

                        //UserCount = model.isHall?model.UserCount: username.Length,
                        MapSelction = model.MapSelction,
                        IsTestGame = model.IsTestGame ? 1 : 0,
                        GameStatus = 0,
                        starttime = DateTime.Now,
                        endtime = DateTime.Now,
                        username = HttpContext.User.Identity.Name,

                        IsAllowLook = model.IsAllowLook,
                        IsRandomOrder = model.IsRandomOrder,
                        IsRotatoMap = model.IsRotatoMap,
                        version = 4,

                        //游戏大厅
                        isHall = model.isHall,
                        remark = model.remark,
                        //round = model.isHall?-1:0,
                    };
                //游戏大厅
                if (model.isHall)
                {
                    gameInfoModel.round = -1;
                    gameInfoModel.UserCount = model.UserCount;
                    gameInfoModel.userlist = string.Format("|{0}|", this.User.Identity.Name);

                }
                else
                {
                    gameInfoModel.round = 0;
                    gameInfoModel.UserCount = username.Length;
                    gameInfoModel.userlist = string.Join("|", username);

                }
                gameInfoModel.jinzhiFaction = this.HttpContext.Request.Form["jinzhi"];
                //有游戏信息
                if (result != null)
                {
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
                }
                this.dbContext.GameInfoModel.Add(gameInfoModel);
                this.dbContext.SaveChanges();
            };

            //如果是大厅
            if (model.isHall)
            {
                //人数大于4
                if (model.UserCount > 4)
                {
                    return Redirect("/home/NewGameHall");
                }
                else
                {
                    saveGameInfo(null);
                    return Redirect("/Home/GameHallList/");
                }
            }
            else
            {


                //存在屏蔽玩家
                // 以及是否存在屏蔽用户
                //IQueryable<UserFriend> friendList = this.dbContext.UserFriend.AsQueryable();
                int count = username.Length;
                for (int i = 0; i < count - 1; i++)
                {
                    for (int j = i + 1; j < count; j++)
                    {
                        var j1 = j;
                        if (this.dbContext.UserFriend.Any(
                            friend => (friend.UserName == username[i] && friend.UserNameTo == username[j1] &&
                                       friend.Type == 2) ||
                                      (friend.UserNameTo == username[i] && friend.UserName == username[j1] &&
                                       friend.Type == 2)))
                        {
                            ModelState.AddModelError(string.Empty, string.Format("{0}和{1}不能同时存在", username[i], username[j1]));
                            return View(model);
                        }
                    }
                }

                //被禁止的种族
                model.jinzhiFaction = this.HttpContext.Request.Form["jinzhi"];
                //model.jinzhiFaction = jinzhiFaction;
                GaiaGame result = this.CreateGame(username, model);

                if (model.isHall || (result != null && !model.IsTestGame && username[0] != username[1]))//测试局以及自己对战的局暂时不保留数据
                {
                    saveGameInfo(result);
                }

                ViewData["ReturnUrl"] = "/Home/ViewGame/" + model.Name;
                return Redirect("/home/viewgame/" + System.Net.WebUtility.UrlEncode(model.Name));
            }

        }
        /// <summary>
        /// 创建游戏，从内存
        /// </summary>
        private GaiaGame CreateGame(string[] username, NewGameViewModel model)
        {
            //删除空白玩家
            username = username.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            //随机排序
            if (model.IsRandomOrder)
            {
                username = this.RandomSortList<string>(username).ToArray();
            }

            //用户列表
            List<UserGameModel> listUser = new List<UserGameModel>();
            //判断用户不存在
            foreach (var item in username)
            {
                var user = _userManager.FindByNameAsync(item);
                if (user.Result == null)
                {
                    ModelState.AddModelError(string.Empty, item + "用户不存在");
                    //return View("NewGame");
                }
                else
                {
                    listUser.Add(new UserGameModel()
                    {
                        username = item,
                        isTishi = true,
                        paygrade = user.Result.paygrade,
                        scoreUserStart = user.Result.scoreUser,
                    });
                }
            }
            //创建游戏
            bool create = GameMgr.CreateNewGame(username,model,out GaiaGame result);

        
            //赋值用户信息
            result.UserGameModels = listUser;

            if (!string.IsNullOrEmpty(model.jinzhiFaction))
            {
                var list = new List<Faction>()
                {
                    new Terraner(null),
                    new Lantida(null),
                    new Hive(null),
                    new HadschHalla(null),
                    new BalTak(null),
                    new Geoden(null),
                    new Gleen(null),
                    new Xenos(null),
                    new Ambas(null),
                    new Taklons(null),
                    new Firaks(null),
                    new MadAndroid(null),
                    new Itar(null),
                    new Nevla(null)
                };
                result.JinzhiFaction = new List<Faction>();
                foreach (string name in model.jinzhiFaction.Split(','))
                {
                    result.JinzhiFaction.Add(list.Find(fac => fac.FactionName.ToString() == name));
                }
            }
            return result;
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
        #endregion


        #region 游戏大厅
        /// <summary>
        /// 游戏大厅
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult NewGameHall()
        {
            var task = _userManager.GetUserAsync(HttpContext.User);
            Task[] taskarray = new Task[] { task };
            Task.WaitAll(taskarray, millisecondsTimeout: 1000);
            if (task.Result != null)
            {
                ViewData["Message"] = task.Result.UserName;
                ViewData["GameList"] = GameMgr.GetAllGameName(task.Result.UserName);
            }
            GaiaProject.Models.HomeViewModels.NewGameViewModel model =
                new GaiaProject.Models.HomeViewModels.NewGameViewModel()
                {
                    isHall = true,
                    IsAllowLook = true,
                    IsRandomOrder = true
                };
            return View("NewGame", model);
        }
        /// <summary>
        /// 游戏大厅列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GameHallList()
        {
            var task = _userManager.GetUserAsync(HttpContext.User);
            Task[] taskarray = new Task[] { task };
            Task.WaitAll(taskarray, millisecondsTimeout: 1000);
            if (task.Result != null)
            {
                ViewData["Message"] = task.Result.UserName;
                ViewData["GameList"] = GameMgr.GetAllGameName(task.Result.UserName);
            }
            var list = this.dbContext.GameInfoModel.Where(item => item.isHall == true && item.round == -1).ToList();
            return View(list);
        }
        /// <summary>
        /// 加入游戏
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> JoinGame(int id)
        {
            Models.Data.UserFriendController.JsonData jsonData = new Models.Data.UserFriendController.JsonData();

            GameInfoModel gameInfoModel = this.dbContext.GameInfoModel.SingleOrDefault(item => item.Id == id);
            if (gameInfoModel != null)
            {
                //如果包括自己
                if (gameInfoModel.userlist.Contains(string.Format("|{0}|", this.User.Identity.Name)))
                {
                    jsonData.info.state = 400;
                    jsonData.info.message = "已经加入";
                }
                else
                {
                    gameInfoModel.userlist = gameInfoModel.userlist + this.User.Identity.Name + "|";

                    //判断是否满足人数，正式开始游戏
                    string[] username = gameInfoModel.userlist.Trim('|').Split('|');
                    if (username.Length == gameInfoModel.UserCount)
                    {
                        gameInfoModel.round = 0;
                        NewGameViewModel newGameViewModel = new NewGameViewModel()
                        {
                            IsAllowLook = gameInfoModel.IsAllowLook,
                            IsRandomOrder = gameInfoModel.IsRandomOrder,
                            IsRotatoMap = gameInfoModel.IsRotatoMap,
                            IsTestGame = gameInfoModel.IsTestGame == 1,
                            MapSelction = gameInfoModel.MapSelction,
                            Name = gameInfoModel.name,
                            jinzhiFaction = gameInfoModel.jinzhiFaction,
                        };
                        //创建游戏
                        this.CreateGame(username, newGameViewModel);
                    }
                    this.dbContext.GameInfoModel.Update(gameInfoModel);
                    this.dbContext.SaveChanges();

                    jsonData.info.state = 200;
                    jsonData.info.message = "成功";
                }
            }

            return new JsonResult(jsonData);

        }
        /// <summary>
        /// 退出游戏
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ExitGame(int id)
        {
            Models.Data.UserFriendController.JsonData jsonData = new Models.Data.UserFriendController.JsonData();
            GameInfoModel gameInfoModel = this.dbContext.GameInfoModel.SingleOrDefault(item => item.Id == id);
            if (gameInfoModel != null)
            {
                string username = this.User.Identity.Name;
                //如果不包括自己
                if (!gameInfoModel.userlist.Contains(username))
                {
                    jsonData.info.state = 400;
                    jsonData.info.message = "没有加入,无法取消";
                }
                else
                {
                    if (gameInfoModel.username == username)
                    {
                        jsonData.info.state = 400;
                        jsonData.info.message = "创建人暂时无法退出";

                    }
                    else
                    {
                        gameInfoModel.userlist = gameInfoModel.userlist.Replace("|" + username + "|", "");
                        //判断结尾和开头
                        if (!gameInfoModel.userlist.StartsWith("|"))
                        {
                            gameInfoModel.userlist = "|" + gameInfoModel.userlist;
                        }
                        if (!gameInfoModel.userlist.EndsWith("|"))
                        {
                            gameInfoModel.userlist = gameInfoModel.userlist + "|";
                        }

                        this.dbContext.GameInfoModel.Update(gameInfoModel);
                        this.dbContext.SaveChanges();
                        jsonData.info.state = 200;
                        jsonData.info.message = "成功";
                    }

                }
            }

            return new JsonResult(jsonData);

        }


        /// <summary>
        /// 删除游戏
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteHallGame(int id)
        {
            Models.Data.UserFriendController.JsonData jsonData = new Models.Data.UserFriendController.JsonData();
            GameInfoModel gameInfoModel = this.dbContext.GameInfoModel.SingleOrDefault(item => item.Id == id);
            if (gameInfoModel != null)
            {
                string username = this.User.Identity.Name;

                //是自己创建的 或者管理员
                if (username == gameInfoModel.username || (_userManager.GetUserAsync(User).Result != null && _userManager.GetUserAsync(User).Result.groupid == 1))
                {
                    this.dbContext.GameInfoModel.Remove(gameInfoModel);
                    this.dbContext.SaveChanges();
                    jsonData.info.state = 200;
                }
            }

            return new JsonResult(jsonData);

        }


        #endregion


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
        /// 
        /// </summary>
        /// <param name="id">游戏ID</param>
        /// <param name="type"></param>
        /// <returns></returns>
        private GaiaGame RestoreGame(int id,out int type, int? row = null)
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
                else if (!gameInfoModel.IsAllowLook)//不允许查看
                {
                    type = 0;
                    return null;
                }
                else
                {
                    var game = GameMgr.GetGameByName(gameInfoModel.name);
                    if (game == null)//游戏不存在
                    {
                        if (string.IsNullOrEmpty(gameInfoModel.loginfo))
                        {
                            type = 0;
                            return null;
                        }
                        log = gameInfoModel.loginfo;
                    }
                    else
                    {
                        type = 1;//跳转
                        return game;
                        //return Redirect("/Home/ViewGame/" + gameInfoModel.name);
                    }
                    //log = game.UserActionLog;
                }

                GameMgr.CreateNewGame(gameInfoModel.name, gameInfoModel.userlist.Split('|'), out GaiaGame result, gameInfoModel.MapSelction, isTestGame: gameInfoModel.IsTestGame == 1 ? true : false,IsRotatoMap:gameInfoModel.IsRotatoMap,version:gameInfoModel.version);
                GaiaGame gg = GameMgr.GetGameByName(gameInfoModel.name);
                gg.GameName = gameInfoModel.name;
                gg.UserActionLog = log?.Replace("|", "\r\n");

                //赋值会重写全部数据
                gg.dbContext = this.dbContext;

                gg = GameMgr.RestoreGame(gameInfoModel.name, gg,row:row);
                gg.GameName = gameInfoModel.name;
                //从内存删除
                GameMgr.DeleteOneGame(gameInfoModel.name);
                type = 200;
                return gg;
            }
            type = 0;
            return null;
        }
        /// <summary>
        /// 还原游戏
        /// </summary>
        public IActionResult RestoreGame(int id,int? row)
        {
            GaiaGame gaiaGame = this.RestoreGame(id, out int type, row);
            if (type == 200)//从日志恢复的
            {
                ViewData["row"] = row;
                return View("ViewGame",gaiaGame);
            }
            else if (type == 1)//内存中的游戏
            {
                //var name= Encoding.
                //return Redirect("/Home/ViewGame/" + gaiaGame.GameName);
                return View("ViewGame", gaiaGame);
                //return Redirect("/Home/ViewGame/" + gaiaGame.GameName);
            }
            else
            {
                return View("Index");
            }
            //GameMgr.UndoOneStep(id);
        }
        /// <summary>
        /// 跳过回合
        /// </summary>
        public void SkipRound(string id,int round=6)
        {
            GaiaGame gaiaGame = GameMgr.GetGameByName(id);
            for (int i = gaiaGame.GameStatus.RoundCount; i < 6; i++)
            {
                for (int count = 0; count < gaiaGame.UserCount; count++)
                {
                    gaiaGame.Syntax(
                        string.Format("{1}:pass {0}", gaiaGame.RBTList[0].name,
                            gaiaGame.FactionList[gaiaGame.GameStatus.PlayerIndex].FactionName), out string log,dbContext:this.dbContext);

                }
            }
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
                GameMgr.GetGameByName(name).Syntax(syntax, out string log, task.Result.UserName,dbContext:this.dbContext);

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

        #region 接收游戏命令

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
                //游戏结束，发送邮件赋值，
                GaiaCore.Gaia.Game.DbGameSave._emailSender = this._emailSender;
                //执行命令
                GaiaGame gaiaGame = GameMgr.GetGameByName(name);
                try
                {
                    //GameMgr.WriteUserActionLog(syntax, task.Result.UserName);
                }
                catch { }
                gaiaGame.Syntax(syntax, out string log, task.Result.UserName, dbContext: this.dbContext);
                if (!string.IsNullOrEmpty(log))
                {
                    return "error:" + log;
                }
                else
                {
                    //如果是即时制游戏，进行通知
                    if (gaiaGame.IsSocket)
                    {
                        NoticeWebSocketMiddleware.GameActive(gaiaGame, HttpContext.User);
                    }
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
        public IActionResult LeechPower(string name, FactionName factionName, int power, FactionName leechFactionName, bool isLeech, bool? isPwFirst)
        {
            if (ServerStatus.IsStopSyntax == true)
            {
                return Redirect("/home/serverdown");
            }
            var task = _userManager.GetUserAsync(HttpContext.User);
            Task[] taskarray = new Task[] { task };
            Task.WaitAll(taskarray, millisecondsTimeout: 1000);
            var faction = GameMgr.GetGameByName(name).FactionList.Find(x => x.FactionName.ToString().Equals(name));
            var leech = isLeech ? "leech" : "decline";

            var syntax = string.Format("{0}:{1} {2} from {3}", factionName, leech, power, leechFactionName);
            if (isPwFirst.HasValue)
            {
                var pwFirst = isPwFirst.GetValueOrDefault() ? "pw" : "pwt";
                syntax = syntax + " " + pwFirst;
            }
            GaiaGame gaiaGame = GameMgr.GetGameByName(name);
            try
            {
                //GameMgr.WriteUserActionLog(syntax, task.Result.UserName);
            }
            catch { }
            gaiaGame.Syntax(syntax, out string log, dbContext: this.dbContext);
            //如果是即时制游戏，进行通知
            if (gaiaGame.IsSocket)
            {
                NoticeWebSocketMiddleware.GameActive(gaiaGame, HttpContext.User);
            }
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
            var u = User.Identity;
            if (!PowerUser.IsPowerUser(User.Identity.Name))
            {
                return false;
            }
            return GameMgr.UndoOneStep(id);
        }

        /// <summary>
        /// 会员退回到自己回合
        /// </summary>
        /// <param name="GameName"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ReturnMyTurn(string id)
        {
            var gg = GameMgr.GetGameByName(id);
            if (gg != null)
            {
                //当前用户
                var myUser = gg.UserGameModels.Find(user => user.username == HttpContext.User.Identity.Name.ToString());
                //不是会员
                if (myUser.paygrade == 0 || myUser.resetPayNumber == 0)
                {

                }
                else
                {
                    //-1
                    myUser.resetPayNumber--;

                    //首先要找到自己最近的操作
                    bool flag = false;
                    //种族名称
                    Faction myFaction = gg.FactionList.Find(f => f.UserName == myUser.username);

                    var syntaxList = gg.UserActionLog.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    if (myFaction != null)
                    {
                        for (int i = syntaxList.Count - 1; i > 0; i--)
                        {
                            string str = syntaxList[i];
                            var list = str.Split(':');
                            //隔开
                            if (list.Length == 2)
                            {
                                //当前种族操作
                                if (list[0] == myFaction.FactionName.ToString())
                                {
                                    if (list[1].Contains("pass turn"))
                                    {
                                        flag = true;
                                    }
                                    syntaxList.RemoveAt(i);
                                }
                                //不是当前种族
                                else
                                {
                                    //以及经过自己的主主回合，以及不是自己的回合
                                    if (flag && list[0] != myFaction.FactionName.ToString())
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        syntaxList.RemoveAt(i);
                                    }
                                }
                            }
                            else
                            {
                                //禁止回退到选种
                                if (str.Contains("setup"))
                                {
                                    break;
                                }
                                else
                                {
                                    syntaxList.RemoveAt(i);
                                }

                            }
                        }
                    }
                    gg.UserActionLog = string.Join("\r\n", syntaxList);
                    //重置游戏
                    GameMgr.RestoreGame(gg.GameName, gg, isToDict: true);
                }
            }

            return Redirect("/home/ViewGame/" + id);
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
        public async Task<JsonResult> SyntaxLog(string id, string factionName = null, int factionType = 0)
        {


            var gg = GameMgr.GetGameByName(id);
            StringBuilder stringBuilder = new StringBuilder();
            //数据库查询
            GameInfoModel singleOrDefault = this.dbContext.GameInfoModel.SingleOrDefault(item => item.name == id);
            //内存没有游戏
            if (gg == null)
            {
                //singleOrDefault = this.dbContext.GameInfoModel.SingleOrDefault(item => item.name == id);
                if (singleOrDefault != null)
                {
                    gg = this.RestoreGame(singleOrDefault.Id, out int type);
                }
            }
            if (gg != null)
            {
                List<LogEntity> logList = gg.LogEntityList;
                if (factionName != null)
                {
                    logList = logList.Where(item => item.FactionName.ToString() == factionName).ToList();
                    //只看分数变化的操作
                    if (factionType == 1)
                    {
                        logList = logList.Where(item => item.ResouceChange?.m_score != 0).ToList();
                    }
                }
                foreach (var item in logList.OrderByDescending(x => x.Row))
                {
                    stringBuilder.Append(string.Format("<tr><td>{0}</td><td class='text-right'>{1}</td><td>{2}vp</td><td class='text-right'>{3}</td><td>{4}c</td><td class='text-right'>{5}</td><td>{6}o</td><td class='text-right'>{7}</td><td>{8}q</td><td class='text-right'>{9}</td><td>{10}k</td><td class='text-right'>{11}</td><td>{12}/{13}/{14}</td><td>{15}</td>{16}</tr>", item.FactionName ?? null, @item.ResouceChange?.m_score, item.ResouceEnd?.m_score, item.ResouceChange?.m_credit, item.ResouceEnd?.m_credit, item.ResouceChange?.m_ore, item.ResouceEnd?.m_ore, item.ResouceChange?.m_QICs, item.ResouceEnd?.m_QICs, item.ResouceChange?.m_knowledge, item.ResouceEnd?.m_knowledge, item.ResouceChange?.m_powerToken2 + item.ResouceChange?.m_powerToken3 * 2, item.ResouceEnd?.m_powerToken1, item.ResouceEnd?.m_powerToken2, item.ResouceEnd?.m_powerToken3, item.Syntax,
                        singleOrDefault.GameStatus == 8 ? string.Format("<td><a href='/Home/RestoreGame/{1}/?row={0}'>转到</a></td>", item.Row, singleOrDefault?.Id) : null)
                    );
                }
            }

            return new JsonResult(new Models.Data.UserFriendController.JsonData()
            {
                data = stringBuilder.ToString(),
                info = new Models.Data.UserFriendController.Info() { state = 200 }
            });
        }

        /// <summary>
        /// drop种族
        /// </summary>
        [HttpGet]
        public async Task<JsonResult> DropFaction(string id, string factionName = null)
        {
            var gg = GameMgr.GetGameByName(id);

            UserGameModel singleOrDefault = gg.FactionList.Find(item => item.FactionName.ToString() == factionName)
                .UserGameModel;
            if (singleOrDefault != null)
            {
                singleOrDefault.dropType = 1;
                return new JsonResult(new Models.Data.UserFriendController.JsonData()
                {
                    data = "",
                    info = new Models.Data.UserFriendController.Info() { state = 200 }
                });
            }
            else
            {
                return new JsonResult(new Models.Data.UserFriendController.JsonData()
                {
                    data = "",
                    info = new Models.Data.UserFriendController.Info() { state = 100 }
                });
            }
            
        }

        #endregion




        #region 管理工具
        public IActionResult BackupData()
        {
            ServerStatus.IsStopSyntax = true;
            GameMgr.BackupDictionary();
            return View();
        }
        /// <summary>
        /// 恢复备份
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public IActionResult RestoreData(string filename)
        {
            if (!PowerUser.IsPowerUser(User.Identity.Name))
            {
                return Redirect("/home/index");
            }
            ViewData["nameList"] = string.Join(",", GameMgr.RestoreDictionary(filename));
            //恢复到未维护状态
            ServerStatus.IsStopSyntax = false;
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
