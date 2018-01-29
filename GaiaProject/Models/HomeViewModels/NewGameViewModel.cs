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
        [Display(Name = "游戏名称")]
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
    }

}
