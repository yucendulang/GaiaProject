using System;
using System.Collections.Generic;
using System.Text;
using GaiaCore.Util;

namespace GaiaCore.Gaia.Tiles
{
    public static class RBTMgr
    {
        public static List<RoundBooster> GetRandomList(int n,Random random)
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
                result.Add(list.RandomRemove(random));
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

        public override int GetCreditIncome()
        {
            return 2;
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

        public override int GetPowerIncome()
        {
            return 2;
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

        public override int GetOreIncome()
        {
            return 1;
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
        public override int GetOreIncome()
        {
            return 1;
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
        public override int GetKnowledgeIncome()
        {
            return 1;
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

        public override int GetPowerIncome()
        {
            return 4;
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

        public override int GetCreditIncome()
        {
            return 4;
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

        public override int GetCreditIncome()
        {
            return 2;
        }

        public override int GetQICIncome()
        {
            return 1;
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

        public override int GetOreIncome()
        {
            return 1;
        }

        public override int GetKnowledgeIncome()
        {
            return 1;
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

        public override int GetPowerTokenIncome()
        {
            return 2;
        }

        public override int GetOreIncome()
        {
            return 1;
        }
    }
}
