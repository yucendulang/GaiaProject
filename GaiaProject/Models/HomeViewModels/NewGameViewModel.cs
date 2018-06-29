using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GaiaProject.Models.HomeViewModels
{
    public class NewGameViewModel
    {
        [StringLength(10,MinimumLength =3)]
        [Required]
        [Display(Name = "房间名称")]
        public string Name { set; get; }
        [Required]
        [Display(Name = "玩家1")]
        public string Player1 { set; get; }
        [Required]
        [Display(Name = "玩家2")]
        public string Player2 { set; get; }
        [Display(Name = "玩家3")]
        public string Player3 { set; get; }
        [Display(Name = "玩家4")]
        public string Player4 { set; get; }


        [Display(Name = "随机顺位")]
        public bool IsRandomOrder { set; get; }

        [Display(Name = "允许观看(其他玩家可以查看)")]
        public bool IsAllowLook { set; get; }

        [Display(Name = "是否为测试局")]
        public bool IsTestGame { set; get; }

        [Display(Name = "即时刷新(需要浏览器支持websocket)")]
        public bool IsSocket { set; get; }

        [Display(Name = "地图选择")]
        public string MapSelction { set; get; }

        [Display(Name = "旋转地图(随机模式下尾家旋转)")]
        public bool IsRotatoMap { get; set; }


        /// <summary>
        /// 玩家人数
        /// </summary>
        [Display(Name = "玩家人数")]        
        public int UserCount { get; set; }

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

        [Display(Name = "禁止种族")]

        public string jinzhiFaction { get; set; }
    }

}
