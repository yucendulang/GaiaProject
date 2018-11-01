using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GaiaDbContext.Models.HomeViewModels
{
    public class MatchJoinModel
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(200)]
        [Display(Name = "标题")]
        public string Name { get; set; }
        /// <summary>
        /// 比赛ID
        /// </summary>
        public int matchInfo_id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(20)]
        public string username { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(20)]
        public string userid { get; set; }
        /// <summary>
        /// 报名时间
        /// </summary>
        public DateTime? AddTime { get; set; }
        /// <summary>
        /// 最后名词
        /// </summary>
        public Int16 Rank { get; set; }
        /// <summary>
        /// 得分
        /// </summary>
        public Int16 Score { get; set; }

        public Int16 first { get; set; }

        public Int16 second { get; set; }

        public Int16 third { get; set; }

        public Int16 fourth { get; set; }

        public Int16 avgwinscore { get; set; }
        //avgscore
    }
}
