using System;
using System.Collections.Generic;
using System.Text;
using GaiaCore.Util;

namespace GaiaCore.Gaia.Tiles
{
    public static class RBTMgr
    {
        public static List<RoundBooster> GetRandomList(int n)
        {
            var list = new List<RoundBooster>()
            {
                new RBT1(),
                new RBT2(),
                new RBT3(),
                new RBT4(),
                new RBT5(),
                new RBT6(),
                new RBT7(),
                new RBT8(),
                new RBT9(),
                new RBT10()
            };
            var result = new List<RoundBooster>();
            for (int i = 0; i < n; i++)
            {
                result.Add(list.RandomRemove());
            }
            return result;
        }
    }
    public abstract class RoundBooster:GameTiles
    {
    }

    public class RBT1 : RoundBooster
    {
        public override string desc
        {
            get
            {
                return "ACT:1TF,2C";
            }
        }
    }
    public class RBT2 : RoundBooster
    {
        public override string desc
        {
            get
            {
                return "ACT:3SHIP,2PW";
            }
        }
    }
    public class RBT3 : RoundBooster
    {
        public override string desc
        {
            get
            {
                return "pass-vp:M*1,1O";
            }
        }
    }
    public class RBT4 : RoundBooster
    {
        public override string desc
        {
            get
            {
                return "pass-vp:TC*2,1O";
            }
        }
    }
    public class RBT5 : RoundBooster
    {
        public override string desc
        {
            get
            {
                return "pass-vp:RL*2,1K";
            }
        }
    }
    public class RBT6 : RoundBooster
    {
        public override string desc
        {
            get
            {
                return "pass-vp:SH/AD*4,4PW";
            }
        }
    }
    public class RBT7 : RoundBooster
    {
        public override string desc
        {
            get
            {
                return "pass-vp:G*4,4C";
            }
        }
    }
    public class RBT8 : RoundBooster
    {
        public override string desc
        {
            get
            {
                return "2C,1Q";
            }
        }
    }
    public class RBT9 : RoundBooster
    {
        public override string desc
        {
            get
            {
                return "1O,1K";
            }
        }
    }
    public class RBT10 : RoundBooster
    {
        public override string desc
        {
            get
            {
                return "2PWT,1O";
            }
        }
    }
}
