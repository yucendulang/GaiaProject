using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia.Data
{
    public class PwInfo
    {
        public string code { get; set; }
        public string name { get; set; }

    }

    public class PwInfoList
    {
        public static readonly List<PwInfo> QuickActList = new List<PwInfo>()
        {
            new PwInfo(){code = "4pw to 1q",name = "4点能量兑换1个量子方块"},
            new PwInfo(){code = "3pw to 1o",name = "3点能量兑换1个矿石"},
            new PwInfo(){code = "4pw to 1k",name = "4点能量兑换1点知识"},
            new PwInfo(){code = "1pw to 1c",name = "1点能量兑换1点信用点"},
            new PwInfo(){code = "1o to 1c",name = "1个矿石兑换1点信用点"},
            new PwInfo(){code = "1o to 1pwt",name = "1个矿石兑换1个能量指示物"},
            new PwInfo(){code = "1k to 1c",name = "1点知识兑换1点信用点"},
            new PwInfo(){code = "1q to 1c",name = "1个量子方块兑换1点信用点"},
            new PwInfo(){code = "1q to 1pwt",name = "1个量子方块兑换1个能量指示物"},

        };
        /// <summary>
        /// 超星人
        /// </summary>
        public static readonly List<PwInfo> QuickActListNevla = new List<PwInfo>()
        {
            new PwInfo(){code = "2pw to 1q",name = "2点能量兑换1个量子方块"},
            new PwInfo(){code = "2pw to 1o+1c",name = "2点能量兑换1个矿石和1点信用点"},
            new PwInfo(){code = "2pw to 1pwt+1c",name = "2点能量兑换1个能量指示物和1点信用点"},
            new PwInfo(){code = "2pw to 1k",name = "2点能量兑换1点知识"},
            new PwInfo(){code = "1pw to 2c",name = "1点能量兑换2点信用点"},
            new PwInfo(){code = "1o to 1c",name = "1个矿石兑换1点信用点"},
            new PwInfo(){code = "1o to 1pwt",name = "1个矿石兑换1个能量指示物"},
            new PwInfo(){code = "1q to 1o",name = "1个量子方块兑换1个矿石"},
            new PwInfo(){code = "1k to 1c",name = "1点知识兑换1点信用点"},
            new PwInfo(){code = "1q to 1c",name = "1个量子方块兑换1点信用点"},
            new PwInfo(){code = "1q to 1pwt",name = "1个量子方块兑换1个能量指示物"},


        };
        /// <summary>
        /// 炽炎
        /// </summary>
        public static readonly List<PwInfo> QuickActListBalTak = new List<PwInfo>()
        {
            new PwInfo(){code = "4pw to 1q",name = "4点能量兑换1个量子方块"},
            new PwInfo(){code = "3pw to 1o",name = "3点能量兑换1个矿石"},
            new PwInfo(){code = "4pw to 1k",name = "4点能量兑换1点知识"},
            new PwInfo(){code = "1pw to 1c",name = "1点能量兑换1点信用点"},
            new PwInfo(){code = "1o to 1c",name = "1个矿石兑换1点信用点"},
            new PwInfo(){code = "1o to 1pwt",name = "1个矿石兑换1个能量指示物"},
            new PwInfo(){code = "1k to 1c",name = "1点知识兑换1点信用点"},
            new PwInfo(){code = "1q to 1c",name = "1个量子方块兑换1点信用点"},
            new PwInfo(){code = "1q to 1pwt",name = "1个量子方块兑换1个能量指示物"},
            new PwInfo(){code = "1Gaia to 1q",name = "1个盖亚单元兑换1个量子方块"},
            new PwInfo(){code = "1Gaia to 1o",name = "1个盖亚单元兑换1个矿石"},
            new PwInfo(){code = "1Gaia to 1c",name = "1个盖亚单元兑换1点信用点"},
            new PwInfo(){code = "1Gaia to 1pwt",name = "1个盖亚单元兑换1个能量指示物"},



        };
        /// <summary>
        /// 利爪
        /// </summary>
        public static readonly List<PwInfo> QuickActListTaklons = new List<PwInfo>()
        {
            new PwInfo(){code = "4pw to 1q",name = "4点能量兑换1个量子方块"},
            new PwInfo(){code = "3pw to 1o",name = "3点能量兑换1个矿石"},
            new PwInfo(){code = "4pw to 1k",name = "4点能量兑换1点知识"},
            new PwInfo(){code = "1pw to 1c",name = "1点能量兑换1点信用点"},
            new PwInfo(){code = "1o to 1c",name = "1个矿石兑换1点信用点"},
            new PwInfo(){code = "1o to 1pwt",name = "1个矿石兑换1个能量指示物"},
            new PwInfo(){code = "1k to 1c",name = "1点知识兑换1点信用点"},
            new PwInfo(){code = "1q to 1c",name = "1个量子方块兑换1点信用点"},
            new PwInfo(){code = "1q to 1pwt",name = "1个量子方块兑换1个能量指示物"},
            new PwInfo(){code = "1bs to 1o",name = "1个智慧石兑换1个矿石"},
            new PwInfo(){code = "1bs to 3c",name = "1个智慧石兑换3点信用点"},
            new PwInfo(){code = "1bs+1pw to 1q",name = "1个智慧石和1点能量兑换1个量子方块"},
            new PwInfo(){code = "1bs+1pw to 1k",name = "1个智慧石和1点能量兑换1点知识"},
            new PwInfo(){code = "1bs to 1pwt",name = "1个智慧石兑换1个能量指示物"},


        };
        /// <summary>
        /// 圣禽
        /// </summary>
        public static readonly List<PwInfo> QuickActListHadsch = new List<PwInfo>()
        {
            new PwInfo(){code = "4pw to 1q",name = "4点能量兑换1个量子方块"},
            new PwInfo(){code = "3pw to 1o",name = "3点能量兑换1个矿石"},
            new PwInfo(){code = "4pw to 1k",name = "4点能量兑换1点知识"},
            new PwInfo(){code = "1pw to 1c",name = "1点能量兑换1点信用点"},
            new PwInfo(){code = "1o to 1c",name = "1个矿石兑换1点信用点"},
            new PwInfo(){code = "1o to 1pwt",name = "1个矿石兑换1个能量指示物"},
            new PwInfo(){code = "1k to 1c",name = "1点知识兑换1点信用点"},
            new PwInfo(){code = "1q to 1c",name = "1个量子方块兑换1点信用点"},
            new PwInfo(){code = "1q to 1pwt",name = "1个量子方块兑换1个能量指示物"},
            new PwInfo(){code = "4c to 1q",name = "4点信用点兑换1个量子方块"},
            new PwInfo(){code = "4c to 1k",name = "4点信用点兑换1点知识"},
            new PwInfo(){code = "3c to 1o",name = "3点信用点兑换1个矿石"},
            new PwInfo(){code = "3c to 1pwt",name = "3点信用点兑换1个能量指示物"},


        };

    }
}
