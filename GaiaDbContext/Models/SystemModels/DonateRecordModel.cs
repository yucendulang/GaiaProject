using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GaiaDbContext.Models.SystemModels
{
    /// <summary>
    /// 捐赠记录
    /// </summary>
    public class DonateRecordModel
    {
        [Key]
        public int id { get; set; }


        /// <summary>
        /// 捐赠人
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(20)]
        public string donateuser { get; set; }


        /// <summary>
        /// 添加人
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(20)]
        public string username { get; set; }


        /// <summary>
        /// 收款人
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(20)]
        public string chequeuser { get; set; }

        /// <summary>
        /// 捐赠金额
        /// </summary>
        //[System.ComponentModel.DataAnnotations.MaxLength(20)]
        public decimal donateprice { get; set; }


        /// <summary>
        /// 捐赠方式
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(20)]
        public string donatetype { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(50)]
        public string name { get; set; }


        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? addtime { get; set; }

        /// <summary>
        /// 捐赠时间
        /// </summary>
        public DateTime? donatetime { get; set; }


        /// <summary>
        /// 状态
        /// </summary>
        public int state { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(50)]
        public string remark { get; set; }

        /// <summary>
        /// 新闻ID
        /// </summary>
        public int newid { get; set; }
        /// <summary>
        /// 新闻name
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(50)]
        public string newname { get; set; }

        /// <summary>
        /// 货币单位
        /// </summary>
        public string moneytype { get; set; }


    }
}
