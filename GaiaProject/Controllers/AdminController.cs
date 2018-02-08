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
            newModel.Rank = model.Rank;
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



        #region 众凑
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public IActionResult DonateIndex()
        {
            List<DonateRecordModel> donateRecordModels = this.dbContext.DonateRecordModel.ToList();
            return View(donateRecordModels);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]       
        public IActionResult DonateUpdate(int? id)
        {
            DonateRecordModel model = null;
            if (id > 0)
            {
                model = this.dbContext.DonateRecordModel.SingleOrDefault(item => item.id == id);
            }
            //List<DonateRecordModel> donateRecordModels = this.dbContext.DonateRecordModel.ToList();
            return View(model);
        }

        [HttpPost]
        public IActionResult DonateUpdate(DonateRecordModel model)
        {
            DonateRecordModel newModel;
            //编辑
            if (model.id > 0)
            {
                newModel = this.dbContext.DonateRecordModel.SingleOrDefault(item => item.id == model.id);
            }
            else//添加
            {
                newModel = new DonateRecordModel();
                newModel.addtime = DateTime.Now;
            }
            //赋值
            newModel.donateuser = model.donateuser;
            newModel.donateprice = model.donateprice;
            newModel.donatetime = model.donatetime;
            newModel.donatetype = model.donatetype;
            newModel.chequeuser = model.chequeuser;
            newModel.newid = model.newid;
            //newModel.name = model.name;
            //保存
            if (model.id > 0)
            {
                this.dbContext.DonateRecordModel.Update(newModel);
            }
            else
            {
                this.dbContext.DonateRecordModel.Add(newModel);
            }
            this.dbContext.SaveChanges();
            return Redirect("/Admin/DonateIndex");
        }
        #endregion

        /// <summary>
        /// 删除塑胶
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DelData(int id,string type)
        {
            Models.Data.UserFriendController.JsonData jsonData = new Models.Data.UserFriendController.JsonData();

            jsonData.info.state = 200;
            switch (type)
            {
                case "donate":
                    var donateRecordModel = this.dbContext.DonateRecordModel.SingleOrDefault(item => item.id == id);
                    if (donateRecordModel != null)
                    {
                        this.dbContext.DonateRecordModel.Remove(donateRecordModel);
                        this.dbContext.SaveChanges();
                    }
                    break;
            }
            return new JsonResult(jsonData);

        }

    }
}