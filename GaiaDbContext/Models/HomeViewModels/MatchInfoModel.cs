using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GaiaDbContext.Models.HomeViewModels
{
    /// <summary>
    /// 比赛信息
    /// </summary>
    public class MatchInfoModel
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
        /// 内容
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(40000)]
        [Display(Name = "内容")]
        public string Contents { get; set; }
        /// <summary>
        /// 报名截止时间
        /// </summary>
        [Display(Name = "报名截止时间")]
        public DateTime? RegistrationEndTime { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [Display(Name = "开始时间")]
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [Display(Name = "结束时间")]
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 最多支持人数
        /// </summary>
        [Display(Name = "最多支持人数")]
        public int NumberMax { get; set; }

        /// <summary>
        /// 当前报名人数
        /// </summary>
        public int NumberNow { get; set; }

        [Display(Name = "人满自动创建比赛")]
        public bool IsAutoCreate { get; set; }


    }



}
