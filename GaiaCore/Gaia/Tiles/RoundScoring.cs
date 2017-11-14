using System;
using System.Collections.Generic;
using System.Text;
using GaiaCore.Util;

namespace GaiaCore.Gaia.Tiles
{
    public static class RSTMgr
    {
        public static List<RoundScoring> GetRandomList(int n,Random random)
        {
            var list = new List<RoundScoring>()
            {
                new RST1(),
                new RST2(),
                new RST3(),
                new RST4(),
                new RST5(),
                new RST6(),
                new RST7(),
            };
            var result = new List<RoundScoring>();
            for (int i = 0; i < n; i++)
            {
                result.Add(list.RandomRemove(random));
            }
            return result;
        }
    }


    public abstract class RoundScoring:GameTiles
    {
        public RoundScoring()
        {
            this.typename = "rst";
        }
    }
    public class RST1 : RoundScoring
    {
        public override string desc
        {
            get
            {
                return "M->2VP";
            }
        }
        public override int GetTriggerScore => 2;
    }
    public class RST2 : RoundScoring
    {
        public override string desc
        {
            get
            {
                return "TC->3VP";
            }
        }

        public override int GetTriggerScore => 3;
    }
    public class RST3 : RoundScoring
    {
        public override string desc
        {
            get
            {
                return "SH/AD->5VP";
            }
        }

        public override int GetTriggerScore => 5;
    }
    public class RST4 : RoundScoring
    {
        public override string desc
        {
            get
            {
                return "M(G)->4VP";
            }
        }

        public override int GetTriggerScore => 4;
    }
    public class RST5 : RoundScoring
    {
        public override string desc
        {
            get
            {
                return "AL->5VP";
            }
        }

        public override int GetTriggerScore => 5;
    }
    public class RST6 : RoundScoring
    {
        public override string desc
        {
            get
            {
                return "RA->2VP";
            }
        }
        public override int GetTriggerScore => 2;
    }
    public class RST7 : RoundScoring
    {
        public override string desc
        {
            get
            {
                return "TF->2VP";
            }
        }

        public override int GetTriggerScore => 2;
    }
}
