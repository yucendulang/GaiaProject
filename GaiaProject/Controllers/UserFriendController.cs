using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GaiaProject.Data;
using GaiaProject.Models;
using GaiaProject.Models.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GaiaProject.Controllers
{
    public class UserFriendController : Controller
    {
        // GET: /<controller>/

        private readonly ApplicationDbContext dbContext;

        public UserFriendController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> AddFriend(UserFriend model)
        {
            var result = await dbContext.UserFriend.AddAsync(model);
            await dbContext.SaveChangesAsync();
            return View(null);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var list = dbContext.UserFriend.ToList();
            return View(list);
        }
    }
}
