using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gaia.Model.Final;
using GaiaDbContext.Models.SystemModels;
using GaiaProject.Data;
using Microsoft.AspNetCore.Mvc;

namespace GaiaProject.Controllers
{
    public class NewsController   : BaseControlNews
    {
        private readonly ApplicationDbContext dbContext;

        public NewsController(ApplicationDbContext dbContext):base(dbContext)
        {
            this.dbContext = dbContext;
        }
        /// <summary>
        /// 攻略详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ShowInfo(int id)
        {
            //新闻信息
            NewsInfoModel singleOrDefault = this.dbContext.NewsInfoModel.SingleOrDefault(item => item.Id == id);
            return View(singleOrDefault);
            
        }
        /// <summary>
        /// 攻略列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index(int type = NewsConfig.TYPE_GL)
        {
            IQueryable<NewsInfoModel> newsInfoModels = this.dbContext.NewsInfoModel.Where(item => item.type == type).OrderBy(item=>item.Rank);
            return View(newsInfoModels);
        }

        [HttpGet]
        public IActionResult Modify(int? id, int type = NewsConfig.TYPE_GL)
        {
            NewsInfoModel newModel = base.News_Update(id);
            //判断空
            if (newModel == null)
            {
                newModel = new NewsInfoModel();
            }
            newModel.type = type;
            return View(newModel);
        }
        /// <summary>
        /// 提交更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Modify(NewsInfoModel model)
        {
            //类型
            //model.type = type;
            //状态
            model.state = 1;
            //用户
            model.username = this.User.Identity.Name;

            NewsInfoModel newModel = base.News_Update(model);
            return Redirect("/News/Index?type="+model.type.ToString());
            return View(newModel);
        }


    }
}