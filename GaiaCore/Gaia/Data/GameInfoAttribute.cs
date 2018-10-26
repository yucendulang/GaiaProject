using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia.Data
{
    public class GameInfoAttribute
    {
        /// <summary>
        /// 地图模式
        /// </summary>
        public static readonly List<PwInfo> MapSelction = new List<PwInfo>()
        {
            new PwInfo(){code = "fix2p",name = "双人固定"},
            new PwInfo(){code = "random2p",name = "双人随机"},
            new PwInfo(){code = "fix3p",name = "双人变体固定"},
            new PwInfo(){code = "random3p",name = "双人变体随机"},
            new PwInfo(){code = "fix4p",name = "四人固定"},
            new PwInfo(){code = "random4p",name = "四人部分随机"},
            new PwInfo(){code = "randomall4p",name = "四人完全随机"},

        };
        /// <summary>
        /// 连续不行动的drop时间,暂时没有1,5,10,
        /// </summary>

        public static readonly List<int> DropHour = new List<int>()
        {
            24,72,120,240
        };

    }
}
