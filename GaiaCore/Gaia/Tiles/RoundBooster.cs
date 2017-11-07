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
                return "Action rbt1:1TF , 2C";
            }
        }

        public override int GetCreditIncome()
        {
            return 2;
        }

        public override bool InvokeGameTileAction(Faction faction)
        {
            faction.TerraFormNumber += 1;
            IsUsed = true;
            return true;
        }
        public override bool CanAction => true;
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

        public override bool InvokeGameTileAction(Faction faction)
        {
            faction.TempShip += 1;
            IsUsed = true;
            return true;
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

        public override int GetTurnEndScore(Faction faction)
        {
            return GameConstNumber.MineCount-faction.Mines.Count;
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

        public override int GetTurnEndScore(Faction faction)
        {
            return (GameConstNumber.TradeCenterCount - faction.TradeCenters.Count)*2;
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

        public override int GetTurnEndScore(Faction faction)
        {
            return (GameConstNumber.ResearchLabCount - faction.ReaserchLabs.Count) * 2;
        }
    }
    public class RBT6 : RoundBooster
    {
        public override string desc
        {
            get
            {
                return "pass-vp:SH/AC*4,4PW";
            }
        }

        public override int GetPowerIncome()
        {
            return 4;
        }
        public override int GetTurnEndScore(Faction faction)
        {
            var ret = 0;
            if (faction.Academy1 == null)
            {
                ret += 4;
            }
            if (faction.Academy2 == null)
            {
                ret += 4;
            }
            if (faction.StrongHold == null)
            {
                ret += 4;
            }

            return ret;
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
        public override int GetTurnEndScore(Faction faction)
        {
            return faction.GaiaPlanetNumber * 4;
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
