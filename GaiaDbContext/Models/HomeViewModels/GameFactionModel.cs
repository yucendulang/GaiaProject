using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GaiaDbContext.Models.HomeViewModels
{
    /// <summary>
    /// 种族信息
    /// </summary>
    public class GameFactionModel
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
        /// 用户ID
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(20)]
        public string userid { get; set; }
        /// <summary>
        /// 种族名称
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(20)]
        public string FactionName { get; set; }

        /// <summary>
        /// 种族中文名称
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(20)]
        public string FactionChineseName { get; set; }

        /// <summary>
        /// 排名
        /// </summary>
        public int rank { get; set; }
        /// <summary>
        /// 总得分
        /// </summary>
        public int scoreTotal { get; set; }


        /// <summary>
        /// fst1得分
        /// </summary>
        public int scoreFst1 { get; set; }
        /// <summary>
        /// fst1数量
        /// </summary>
        public int numberFst1 { get; set; }
        /// <summary>
        /// fst2得分
        /// </summary>
        public int scoreFst2 { get; set; }
        /// <summary>
        ///  fst2数量
        /// </summary>
        public int numberFst2 { get; set; }


        /// <summary>
        /// 科技得分
        /// </summary>

        public int scoreKj { get; set; }
        /// <summary>
        /// 科技等级
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(20)]
        public string kjPostion { get; set; }
        /// <summary>
        /// 回合得分
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(20)]
        public string scoreRound { get; set; }
        /// <summary>
        /// 魔力扣分
        /// </summary>
        public int scorePw { get; set; }

        /// <summary>
        /// 建筑数量
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(20)]
        public string numberBuild { get; set; }

        /// <summary>
        /// 玩家数量
        /// </summary>
        public int? UserCount { get; set; }

        public int? scoreLuo { get; set; }

    }
}
