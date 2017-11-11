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
        [EmailAddress]
        [Display(Name = "玩家1")]
        public string Player1 { set; get; }
        [Required]
        [EmailAddress]
        [Display(Name = "玩家2")]
        public string Player2 { set; get; }
        [EmailAddress]
        [Display(Name = "玩家3")]
        public string Player3 { set; get; }
        [EmailAddress]
        [Display(Name = "玩家4")]
        public string Player4 { set; get; }
        [Display(Name = "是否为测试局")]
        public bool IsTestGame { set; get; }
    }
}
