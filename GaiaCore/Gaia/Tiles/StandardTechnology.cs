using System;
using System.Collections.Generic;
using System.Text;
using GaiaCore.Util;

namespace GaiaCore.Gaia.Tiles
{
    public static class STTMgr
    {
        public static List<StandardTechnology> GetRandomList(int n, Random random)
        {
            var list = new List<StandardTechnology>()
            {
                new STT1(),
                new STT2(),
                new STT3(),
                new STT4(),
                new STT5(),
                new STT6(),
                new STT7(),
                new STT8(),
                new STT9()
            };
            var result = new List<StandardTechnology>();
            for (int i = 0; i < n; i++)
            {
                result.Add(list.RandomRemove(random));
            }
            return result;
        }

        public static List<StandardTechnology> GetOtherList(List<StandardTechnology> removelist)
        {
            var list = new List<StandardTechnology>()
            {
                new STT1(),
                new STT2(),
                new STT3(),
                new STT4(),
                new STT5(),
                new STT6(),
                new STT7(),
                new STT8(),
                new STT9()
            };
            list.RemoveAll(x => removelist.Exists(y => x.GetType() == y.GetType()));
            return list;
        }
    }


    public abstract class StandardTechnology : GameTiles
    {
        public StandardTechnology()
        {
            this.name = this.GetType().Name;
        }
    }
    /// <summary>
    /// 行动：获得4魔力
    /// </summary>
    public class STT1 : StandardTechnology
    {


        public override string desc
        {
            get
            {
                return "Act:4PW";
            }
        }

        public const int powerIncreaseConst = 4;

        public override bool CanAction => true;
        public override bool InvokeGameTileAction(Faction faction)
        {
            faction.PowerIncrease(powerIncreaseConst);
            return true;
        }
    }
    /// <summary>
    /// 每当在盖亚星球建造矿场，获得3分
    /// </summary>
    public class STT2 : StandardTechnology
    {
        public override string desc
        {
            get
            {
                return "M(G) >> 3VP";
            }
        }
    }
    /// <summary>
    /// 获得7分
    /// </summary>
    public class STT3 : StandardTechnology
    {
        public override string desc
        {
            get
            {
                return "7VP";
            }
        }
        public override bool OneTimeAction(Faction faction)
        {
            faction.Score += 7;
            return true;
        }
    }
    /// <summary>
    /// 每登录1个星球种类，为你获得1点知识
    /// </summary>
    public class STT4 : StandardTechnology
    {
        public override string desc
        {
            get
            {
                return "1 P_type->1K";
            }
        }

        public override bool OneTimeAction(Faction faction)
        {
            faction.Knowledge += faction.GetPlanetTypeCount();
            return true;
        }
    }
    public class STT5 : StandardTechnology
    {
        public override string desc
        {
            get
            {
                return "1O,1Q";
            }
        }
        public override bool OneTimeAction(Faction faction)
        {
            faction.Ore += 1;
            faction.QICs += 1;
            return true;
        }
    }
    public class STT6 : StandardTechnology
    {
        public override string desc
        {
            get
            {
                return "+4C";
            }
        }

        public override int GetCreditIncome()
        {
            return 4;
        }
    }
    public class STT7 : StandardTechnology
    {
        public override string desc
        {
            get
            {
                return "+1O,+1PW";
            }
        }
        public override int GetOreIncome()
        {
            return 1;
        }
        public override int GetPowerIncome()
        {
            return 1;
        }
    }
    public class STT8 : StandardTechnology
    {
        public override string desc
        {
            get
            {
                return "+1K,+1C";
            }
        }
        public override int GetKnowledgeIncome()
        {
            return 1;
        }
        public override int GetCreditIncome()
        {
            return 1;
        }
    }
    /// <summary>
    /// 学院要塞等级+1
    /// </summary>
    public class STT9 : StandardTechnology
    {
        public override string desc
        {
            get
            {
                return "SH/AD_PWLV+1";
            }
        }

        public override bool OneTimeAction(Faction faction)
        {
            if (faction.Academy1 != null)
            {
                faction.Academy1.MagicLevelIncrease += 1;
            }
            if (faction.Academy2 != null)
            {
                faction.Academy2.MagicLevelIncrease += 1;
            }
            if (faction.StrongHold != null)
            {
                faction.StrongHold.MagicLevelIncrease += 1;
            }

            foreach (var item in faction.GaiaGame.Map.HexArray)
            {
                if (item.Building == null || item.FactionBelongTo != faction.FactionName)
                {
                    continue;
                }
                if (item.Building is Academy)
                {
                    (item.Building as Academy).MagicLevelIncrease += 1;
                }
                if (item.Building is StrongHold)
                {
                    (item.Building as StrongHold).MagicLevelIncrease += 1;
                }
            }
            return true;
        }
    }
}
