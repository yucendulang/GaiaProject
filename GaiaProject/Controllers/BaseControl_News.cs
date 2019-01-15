using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GaiaDbContext.Models.SystemModels;
using GaiaProject.Data;
using Microsoft.AspNetCore.Mvc;

namespace GaiaProject.Controllers
{
    public class BaseControlNews : Controller
    {
        private readonly ApplicationDbContext dbContext;
        public BaseControlNews(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        /// <summary>
        /// 编辑更新
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected NewsInfoModel News_Update(int? id)
        {
            NewsInfoModel newModel = null;
            if (id > 0)
            {
                newModel = this.dbContext.NewsInfoModel.SingleOrDefault(item => item.Id == id);
            }
            return newModel;
        }

        /// <summary>
        /// 提交更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected NewsInfoModel News_Update(NewsInfoModel model)
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
            newModel.username = model.username;
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
            return newModel;
        }
    }
}
