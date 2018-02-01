using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;

namespace GaiaDbContext.Models.SystemModels
{
    /// <summary>
    /// 新闻信息
    /// </summary>
    public class NewsInfoModel
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(50)]
        public string name { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(50)]

        public string remark { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(20)]
        public string username { get; set; }

        /// <summary>
        /// 删除状态
        /// </summary>
        public int isDelete { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int state { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(4000)]
        public string contents { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? AddTime { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Rank { get; set; }
    }
}