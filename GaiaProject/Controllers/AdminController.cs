using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GaiaDbContext.Models.SystemModels;
using GaiaProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace GaiaProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private IMemoryCache cache;

        public AdminController(ApplicationDbContext dbContext, IMemoryCache cache)
        {
            this.dbContext = dbContext;
            this.cache = cache;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region 新闻

        /// <summary>
        /// 新闻列表
        /// </summary>
        /// <returns></returns>

        public IActionResult NewsIndex()
        {
            List<NewsInfoModel> newsInfoModels = this.dbContext.NewsInfoModel.ToList();
            return View(newsInfoModels);
        }
        /// <summary>
        /// 更新新闻
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult NewsUpdate(int? id)
        {
            NewsInfoModel newModel = null;
            if (id > 0)
            {
                newModel = this.dbContext.NewsInfoModel.SingleOrDefault(item => item.Id == id);
            }
            return View(newModel);
        }
        /// <summary>
        /// 提交更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult NewsUpdate(NewsInfoModel model)
        {
            NewsInfoModel newModel;
            //编辑
            if (model.Id > 0)
            {
                newModel = this.dbContext.NewsInfoModel.SingleOrDefault(item => item.Id == model.Id);
            }
            else//添加
            {
                newModel = new NewsInfoModel();
                newModel.AddTime = DateTime.Now;
            }
            //赋值
            newModel.name = model.name;
            newModel.contents = model.contents;
            newModel.type = model.type;
            newModel.state = model.state;
            //newModel.name = model.name;


            //保存
            if (model.Id > 0)
            {
                this.dbContext.NewsInfoModel.Update(newModel);
            }
            else
            {
                this.dbContext.NewsInfoModel.Add(newModel);
            }
            this.dbContext.SaveChanges();
            return Redirect("/Admin/NewsIndex");
            return View(newModel);
        }


        public IActionResult NewsIndexUpdate()
        {
            this.cache.Remove(HomeController.IndexName);
            return Redirect("/Admin/NewsIndex");
        }
        #endregion


    }
}