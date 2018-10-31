using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GaiaDbContext.Models.HomeViewModels
{
    public class MatchGameModel
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
        public string gameid { get; set; }
       


    }
}
