using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GaiaDbContext.Models.HomeViewModels
{
    public class GameFactionExtendModel
    {
        [Key]
        public int Id { get; set; }

        //public int gamefaction_id { get; set; }


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
        /// 基础科技版
        /// </summary>
        public Int16 STT1 { get; set; }
        public Int16 STT2 { get; set; }
        public Int16 STT3 { get; set; }
        public Int16 STT4 { get; set; }
        public Int16 STT5 { get; set; }
        public Int16 STT6 { get; set; }
        public Int16 STT7 { get; set; }
        public Int16 STT8 { get; set; }
        public Int16 STT9 { get; set; }

        /// <summary>
        /// 高级科技版
        /// </summary>
        //public string attlist { get; set; }

        public Int16 ATT1 { get; set; }
        public Int16 ATT2 { get; set; }
        public Int16 ATT3 { get; set; }
        public Int16 ATT4 { get; set; }
        public Int16 ATT5 { get; set; }
        public Int16 ATT6 { get; set; }
        public Int16 ATT7 { get; set; }
        public Int16 ATT8 { get; set; }
        public Int16 ATT9 { get; set; }
        public Int16 ATT10 { get; set; }
        public Int16 ATT11 { get; set; }
        public Int16 ATT12 { get; set; }
        public Int16 ATT13 { get; set; }
        public Int16 ATT14 { get; set; }
        public Int16 ATT15 { get; set; }

        ///得分
        public Int16 ATT4Score { get; set; }
        public Int16 ATT5Score { get; set; }
        public Int16 ATT6Score { get; set; }
        public Int16 ATT7Score { get; set; }
        public Int16 ATT8Score { get; set; }
        public Int16 ATT9Score { get; set; }
        public Int16 ATT10Score { get; set; }
        public Int16 ATT11Score { get; set; }
        public Int16 ATT12Score { get; set; }
        public Int16 ATT13Score { get; set; }
        public Int16 ATT14Score { get; set; }
        public Int16 ATT15Score { get; set; }
    }
}
