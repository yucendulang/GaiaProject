using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GaiaDbContext.Models.SystemModels;
using GaiaProject.Data;
using Microsoft.AspNetCore.Mvc;

namespace GaiaProject.Controllers
{
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public NewsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        //        public IActionResult Index()
        //        {
        //            return View();
        //        }

        public IActionResult ShowInfo(int id)
        {
            //新闻信息
            //NewsInfoModel singleOrDefault = this.dbContext.NewsInfoModel.SingleOrDefault(item => item.type == id);
            //return View(singleOrDefault);
            return View(null);
            
        }
    }
}