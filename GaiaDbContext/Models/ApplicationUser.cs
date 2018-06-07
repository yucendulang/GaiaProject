using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GaiaDbContext.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        //好友
        //public List<ApplicationUser> Friends { get; set; }

        //public string testName { get; set; }

        /// <summary>
        /// 分组ID，1=管理员
        /// </summary>
        public int? groupid { get; set; }

        /// <summary>
        /// 付费等级
        /// </summary>
        public int paygrade { get; set; }


        /// <summary>
        /// 用户积分
        /// </summary>
        public int scoreUser { get; set; }
    }
}
