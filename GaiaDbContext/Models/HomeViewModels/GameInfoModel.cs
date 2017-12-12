using System;
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

        public int GameStatus { set; get; }

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

    }
}
