using GaiaCore.Gaia.Resources;
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
            new PwInfo(){code = "4pw to 1q",name = Messages.FourPWTo1QIC},
            new PwInfo(){code = "3pw to 1o",name = Messages.ThreePWTo1Ore},
            new PwInfo(){code = "4pw to 1k",name = Messages.FourPWTo1Knowledge},
            new PwInfo(){code = "1pw to 1c",name = Messages.OnePWTo1Credit},
            new PwInfo(){code = "1o to 1c",name = Messages.OneOreTo1Credit},
            new PwInfo(){code = "1o to 1pwt",name = Messages.OneOreTo1PWT},
            new PwInfo(){code = "1k to 1c",name = Messages.OneKnowledgeTo1Credit},
            new PwInfo(){code = "1q to 1c",name = Messages.OneQICTo1Credit},
            new PwInfo(){code = "1q to 1pwt",name = Messages.OneQICTo1PWT},
            new PwInfo(){code = "1q to 1o",name = Messages.OneQICTo1Ore},

        };

        //人类盖亚阶段
        public static readonly List<PwInfo> QuickActListTerraner = new List<PwInfo>()
        {
            new PwInfo(){code = "4pw to 1q",name = Messages.FourPWTo1QIC},
            new PwInfo(){code = "3pw to 1o",name = Messages.ThreePWTo1Ore},
            new PwInfo(){code = "4pw to 1k",name = Messages.FourPWTo1Knowledge},
            new PwInfo(){code = "1pw to 1c",name = Messages.OnePWTo1Credit},
            new PwInfo(){code = "8pw to 2q",name = Messages.EightPWTo2QIC},
            new PwInfo(){code = "6pw to 2o",name = Messages.SixPWTo2Ore},
            new PwInfo(){code = "8pw to 2k",name = Messages.EightPWTo2Knowledge},

        };


        /// <summary>
        /// 超星人
        /// </summary>
        public static readonly List<PwInfo> QuickActListNevla = new List<PwInfo>()
        {
            new PwInfo(){code = "2pw to 1q",name = Messages.TwoPWTo1QIC},
            new PwInfo(){code = "2pw to 1o,1c",name = Messages.TwoPWTo1Ore1Credit},
            new PwInfo(){code = "2pw to 1pwt,1c",name = Messages.TwoPWTo1PWT1Credit},
            new PwInfo(){code = "2pw to 1k",name = Messages.TwoPWTo1Knowledge},
            new PwInfo(){code = "1pw to 2c",name = Messages.OnePWTo2Credits},
            new PwInfo(){code = "3pw to 2o",name = Messages.ThreePWTo2Ore},


        };
        /// <summary>
        /// 炽炎
        /// </summary>
        public static readonly List<PwInfo> QuickActListBalTak = new List<PwInfo>()
        {
            new PwInfo(){code = "1Gaia to 1q",name = Messages.OneGaiaTo1QIC},
            new PwInfo(){code = "1Gaia to 1o",name = Messages.OneGaiaTo1Ore},
            new PwInfo(){code = "1Gaia to 1c",name = Messages.OneGaiaTo1Credit},
            new PwInfo(){code = "1Gaia to 1pwt",name = Messages.OneGaiaTo1PWT},



        };
        /// <summary>
        /// 利爪
        /// </summary>
        public static readonly List<PwInfo> QuickActListTaklons = new List<PwInfo>()
        {
            new PwInfo(){code = "burn 1.convert 1bs to 1o",name = Messages.Burn1Convert1BSTo1Ore},
            new PwInfo(){code = "burn 1.convert 1bs to 3c",name = Messages.Burn1Convert1BSTo3Credits},
            new PwInfo(){code = "convert 1bs to 1o",name = Messages.Convert1BSTo1Ore},
            new PwInfo(){code = "convert 1bs to 3c",name = Messages.Convert1BSTo3Credits},
            new PwInfo(){code = "convert 1bs,1pw to 1q",name = Messages.Convert1BS1PWTo1QIC},
            new PwInfo(){code = "convert 1bs,1pw to 1k",name = Messages.Convert1BS1PWTo1Knowledge},
            new PwInfo(){code = "convert 1bs to 1pwt",name = Messages.Convert1BSTo1PWT},

        };
        /// <summary>
        /// 圣禽
        /// </summary>
        public static readonly List<PwInfo> QuickActListHadsch = new List<PwInfo>()
        {
            new PwInfo(){code = "4c to 1q",name = Messages.FourCreditsTo1QIC},
            new PwInfo(){code = "4c to 1k",name = Messages.FourCreditsTo1Knowledge},
            new PwInfo(){code = "3c to 1o",name = Messages.ThreeCreditsTo1Ore},
            //new PwInfo(){code = "3c to 1pwt",name = "3点信用点兑换1个能量指示物"},


        };

    }
}
