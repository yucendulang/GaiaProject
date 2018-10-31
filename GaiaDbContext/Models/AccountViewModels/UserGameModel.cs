using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GaiaDbContext.Models.AccountViewModels
{
    [JsonObject(MemberSerialization.OptIn)]
    public class UserGameModel
    {
        public UserGameModel()
        {
            this.isTishi = true;
            this.resetNumber = 5;
            this.resetPayNumber = 5;
        }
        /// <summary>
        /// 用户名
        /// </summary>
        [JsonProperty]
        public string username { get; set; }
        /// <summary>
        /// 备忘
        /// </summary>
        [JsonProperty]
        public string remark { get; set; }
        /// <summary>
        /// 是否提示信息
        /// </summary>
        [JsonProperty]
        public bool isTishi { get; set; }
        /// <summary>
        /// 自动刷新
        /// </summary>
        [JsonProperty]

        public bool isSocket { get; set; }

        /// <summary>
        /// 重置阐述
        /// </summary>

        [JsonProperty]

        public int resetNumber { get; set; }

        /// <summary>
        /// 会员强制退回操作
        /// </summary>

        [JsonProperty]

        public int resetPayNumber { get; set; }

        /// <summary>
        /// 付费等级
        /// </summary>
        [JsonProperty]
        public int? paygrade { get; set; }


        /// <summary>
        /// 用户加入时积分
        /// </summary>
        public int scoreUserStart { get; set; }

        /// <summary>
        /// 是否退出
        /// 0=正常，不正常
        /// </summary>
        [JsonProperty]
        public int dropType { get; set; }

    }
}
