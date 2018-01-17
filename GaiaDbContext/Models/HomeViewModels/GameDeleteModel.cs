using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GaiaDbContext.Models.HomeViewModels
{
    /// <summary>
    /// 游戏删除提醒
    /// </summary>
    public class GameDeleteModel
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 游戏ID
        /// </summary>
        public int gameinfo_id { get; set; }

        [System.ComponentModel.DataAnnotations.MaxLength(20)]
        public string gameinfo_name { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(20)]
        public string username { get; set; }
        /// <summary>
        /// 种族名称
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(20)]
        public string FactionName { get; set; }

        /// <summary>
        /// 同意状态
        /// </summary>
        public int state { get; set; }

    }
}
