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
            new PwInfo(){code = "4pw to 1q",name = "4pw to 1q"},
            new PwInfo(){code = "3pw to 1o",name = "3pw to 1o"},
            new PwInfo(){code = "4pw to 1k",name = "4pw to 1k"},
            new PwInfo(){code = "1pw to 1c",name = "1pw to 1c"},
            new PwInfo(){code = "1o to 1c",name = "1o to 1c"},
            new PwInfo(){code = "1o to 1pwt",name = "1o to 1pwt"},
            new PwInfo(){code = "1k to 1c",name = "1k to 1c"},
            new PwInfo(){code = "1q to 1c",name = "1q to 1c"},
            new PwInfo(){code = "1q to 1pwt",name = "1q to 1pwt"},

        };

    }
}
