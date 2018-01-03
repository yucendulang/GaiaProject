using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GaiaDbContext.Models;
using GaiaDbContext.Models.HomeViewModels;
using GaiaProject.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GaiaProject.Controllers
{
    public class MatchInfoController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public MatchInfoController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            this.dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var list = this.dbContext.MatchInfoModel.ToList();
            return View(list);
        }
        /// <summary>
        /// 添加比赛
        /// </summary>
        /// <returns></returns>
        [HttpGet]

        public IActionResult AddMatch()
        {
            return View();
        }

        [HttpPost]

        public IActionResult AddMatch(GaiaDbContext.Models.HomeViewModels.MatchInfoModel model)
        {
            if (ModelState.IsValid)
            {
                this.dbContext.MatchInfoModel.Add(model);
                this.dbContext.SaveChanges();
                return Redirect("Index");
            }
            return View(model);
        }
        /// <summary>
        /// 删除比赛
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DelMatch(int id)
        {
            Models.Data.UserFriendController.JsonData jsonData = new Models.Data.UserFriendController.JsonData();

            MatchInfoModel matchInfoModel = this.dbContext.MatchInfoModel.SingleOrDefault(item => item.Id == id);
            if (matchInfoModel != null)
            {
                this.dbContext.MatchInfoModel.Remove(matchInfoModel);
                this.dbContext.SaveChanges();
                jsonData.info.state = 200;
            }
            return new JsonResult(jsonData);

        }


        /// <summary>
        /// 详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ShowInfo(int id)
        {
            //主要信息
            var matchInfoModel = this.dbContext.MatchInfoModel.SingleOrDefault(item => item.Id == id);
            Models.Data.UserFriendController.JsonData jsonData = new Models.Data.UserFriendController.JsonData();
            //jsonData.data = matchInfoModel;
            //查询当前报名人
            IQueryable<MatchJoinModel> matchJoinModels = this.dbContext.MatchJoinModel.Where(item => item.matchInfo_id == matchInfoModel.Id);

            jsonData.data = new
            {
                matchInfoModel = matchInfoModel,
                matchJoinModels = matchJoinModels,
            };
            jsonData.info.state = 200;
            return new JsonResult(jsonData);
        }
        /// <summary>
        /// 加入比赛
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> JoinMatch(int id)
        {
            Models.Data.UserFriendController.JsonData jsonData = new Models.Data.UserFriendController.JsonData();
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var matchInfoModel = this.dbContext.MatchInfoModel.SingleOrDefault(item => item.Id == id);
            if (matchInfoModel != null)
            {
                if (this.dbContext.MatchJoinModel.Any(
                    item => item.matchInfo_id == matchInfoModel.Id && item.username == user.UserName))
                {
                    jsonData.info.state = 0;
                    jsonData.info.message = "已经报名";

                }
                else
                {
                    //是否已经满足报名
                    if (matchInfoModel.NumberMax !=0 &&matchInfoModel.NumberNow == matchInfoModel.NumberMax)
                    {
                        jsonData.info.state = 0;
                        jsonData.info.message = "报名人员已满";
                    }
                    else if (matchInfoModel.RegistrationEndTime<DateTime.Now)
                    {
                        jsonData.info.state = 0;
                        jsonData.info.message = "报名时间截止";
                    }
                    else
                    {
                        //报名人数+1
                        matchInfoModel.NumberNow++;
                        this.dbContext.MatchInfoModel.Update(matchInfoModel);
                        //报名信息
                        MatchJoinModel matchJoinModel = new MatchJoinModel();
                        matchJoinModel.matchInfo_id = id;//id
                        matchJoinModel.Name = matchInfoModel.Name;//name

                        matchJoinModel.AddTime = DateTime.Now;//时间
                        matchJoinModel.username = user.UserName;
                        matchJoinModel.userid = user.Id;

                        matchJoinModel.Rank = 0;
                        matchJoinModel.Score = 0;

                        this.dbContext.MatchJoinModel.Add(matchJoinModel);
                        this.dbContext.SaveChanges();

                        jsonData.info.state = 200;
                    }
                }
            }
            return new JsonResult(jsonData);
        }

        /// <summary>
        /// 退出比赛
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ExitMatch(int id)
        {
            Models.Data.UserFriendController.JsonData jsonData = new Models.Data.UserFriendController.JsonData();

            //比赛信息
            MatchInfoModel matchInfoModel = this.dbContext.MatchInfoModel.SingleOrDefault(item => item.Id == id);
            if (matchInfoModel != null)
            {
                MatchJoinModel matchJoinModel =
                    this.dbContext.MatchJoinModel.SingleOrDefault(item => item.matchInfo_id == matchInfoModel.Id &&
                                                                          item.username == HttpContext.User.Identity
                                                                              .Name);
                if (matchJoinModel != null)
                {
                    //删除
                    this.dbContext.MatchJoinModel.Remove(matchJoinModel);
                    //报名人数-1
                    matchInfoModel.NumberNow--;
                    this.dbContext.MatchInfoModel.Update(matchInfoModel);

                    this.dbContext.SaveChanges();

                    jsonData.info.state = 200;
                }
                else
                {
                    jsonData.info.state = 0;
                    jsonData.info.message = "没有报名";

                }
            }

            return new JsonResult(jsonData);
        }
    }
}