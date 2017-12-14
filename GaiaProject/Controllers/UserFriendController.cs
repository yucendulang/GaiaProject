using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GaiaDbContext.Models;
using GaiaDbContext.Models.AccountViewModels;
using GaiaProject.Data;
using GaiaProject.Models;
using GaiaProject.Models.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GaiaProject.Controllers
{
    public partial class UserFriendController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext dbContext;

        public UserFriendController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this._userManager = userManager;
        }
        /// <summary>
        /// 添加好友
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> AddFriend(UserFriend model)
        {
            JsonData jsonData=new JsonData();
            var user = await _userManager.GetUserAsync(HttpContext.User);           
            if (user != null)
            {
                var userTo = await _userManager.FindByNameAsync(model.UserNameTo);
                //找不对对象用户
                if (userTo == null)
                {
                    jsonData.info.state = 0;
                    jsonData.info.message = "找不到用户"+model.UserNameTo;
                }
                else
                {
                    model.UserId = user.Id;
                    model.UserName = user.UserName;
                    var list = dbContext.UserFriend.Where(item => item.UserId == model.UserId && item.UserNameTo == model.UserNameTo).ToList();
                    //已经存在
                    if (list.Count > 0)
                    {
                        jsonData.info.state = 0;
                        jsonData.info.message = "已经是好友了";
                    }
                    else
                    {
                        var result = await dbContext.UserFriend.AddAsync(model);
                        jsonData.info.state = 200;
                        await dbContext.SaveChangesAsync();
                    }
                }


            }
            return new JsonResult(jsonData);
        }


        /// <summary>
        /// 删除好友
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DelFriend(UserFriend model)
        {
            JsonData jsonData = new JsonData();
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                var userTo = await _userManager.FindByNameAsync(model.UserNameTo);
                //找不对对象用户
                if (userTo == null)
                {
                    jsonData.info.state = 0;
                    jsonData.info.message = "找不到用户" + model.UserNameTo;
                }
                else
                {
                   var uf = dbContext.UserFriend.SingleOrDefault(
                        item => item.UserId == user.Id && item.UserNameTo == model.UserNameTo);
                    if (uf != null)
                    {
                        dbContext.UserFriend.Remove(uf);
                        jsonData.info.state = 200;
                        await dbContext.SaveChangesAsync();
                    }
                }


            }
            return new JsonResult(jsonData);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var user =  _userManager.GetUserAsync(HttpContext.User);

            List<UserFriend> list = dbContext.UserFriend.Where(item => item.UserId == user.Result.Id).ToList();
            //List<UserFriend> list = dbContext.UserFriend.FindAsync(item => item.UserId == user.Result.Id).ToList();
            return View(list);
        }

        /// <summary>
        /// 全部用户
        /// </summary>
        /// <returns></returns>
        public IActionResult UserList()
        {
            List<ApplicationUser> list = this.dbContext.Users.OrderByDescending(item=>item.groupid).ToList();
            return View(list);
        }
    }
}
