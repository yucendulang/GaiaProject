using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GaiaDbContext.Models.HomeViewModels
{
    /// <summary>
    /// 游戏聊天记录
    /// </summary>
    public class GameChatModel
    {
        [Key]
        public int id { get; set; }

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
        /// 用户ID
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(20)]
        public string userid { get; set; }
        /// <summary>
        /// 种族名称
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(20)]
        public string factionname { get; set; }

        /// <summary>
        /// 种族中文名称
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(20)]
        public string factionchinesename { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime addtime { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string contents { get; set; }
    }
}
