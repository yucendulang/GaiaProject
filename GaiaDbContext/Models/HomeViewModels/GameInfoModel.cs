﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GaiaDbContext.Models.HomeViewModels
{
    public class GameInfoModel
    {

        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(20)]
        public string name { get; set; }
        /// <summary>
        /// 状态
        /// </summary>

        public int? GameStatus { set; get; }

        [System.ComponentModel.DataAnnotations.MaxLength(50)]
        public string STT6List { get; set; }
        [System.ComponentModel.DataAnnotations.MaxLength(30)]
        public string STT3List { get; set; }
        [System.ComponentModel.DataAnnotations.MaxLength(50)]
        public string ATTList { get; set; }
        [System.ComponentModel.DataAnnotations.MaxLength(50)]
        public string RSTList { get; set; }
        [System.ComponentModel.DataAnnotations.MaxLength(20)]
        public string FSTList { get; set; }
        [System.ComponentModel.DataAnnotations.MaxLength(50)]
        public string RBTList { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public int version { get; set; }
        /// <summary>
        /// 玩家数量
        /// </summary>
        public int UserCount { get; set; }

        /// <summary>
        /// 日志
        /// </summary>
        //[System.ComponentModel.DataAnnotations.MaxLength(4000)]
        public string loginfo { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime starttime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? endtime { get; set; }

        /// <summary>
        /// 最后得分情况
        /// </summary>
        public string scoreFaction { get; set; }


        public int IsTestGame { get; set; }

        public string userlist { get; set; }

        public string MapSelction { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 回合数量
        /// </summary>
        public int? round { get; set; }

        //[Display(Name = "随机顺位")]
        public bool IsRandomOrder { set; get; }

        //[Display(Name = "允许观看")]
        public bool IsAllowLook { set; get; }
        /// <summary>
        /// 删除状态
        /// </summary>
        public int isDelete { get; set; }

        /// <summary>
        /// 是否旋转地图
        /// </summary>
        public bool IsRotatoMap { get; set; }

        /// <summary>
        /// 保存信息状态
        /// </summary>
        public int saveState { get; set; }


        /// <summary>
        /// 游戏说明
        /// </summary>
        [Display(Name = "游戏说明")]
        public string remark { get; set; }

        /// <summary>
        /// 是否游戏大厅
        /// </summary>
        [Display(Name = "是否游戏大厅")]
        public bool isHall { get; set; }
        /// <summary>
        /// 禁止种族
        /// </summary>
        public string jinzhiFaction { get; set; }

        /// <summary>
        /// drop 时间
        /// </summary>
        [Display(Name = "drop Hours")]
        public int dropHour { get; set; }


        /// <summary>
        /// 如果时比赛，加入比赛id
        /// </summary>
        public int matchId { get; set; }
    }
}
