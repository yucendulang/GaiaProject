using System;
using System.Collections.Generic;
using System.Text;
using GaiaCore.Util;
using System.Linq;

namespace GaiaCore.Gaia.Tiles
{
    public static class ATTMgr
    {
        /// <summary>
        /// 获取随机N个ATT板块
        /// </summary>
        /// <param name="i"></param>
        public static List<AdavanceTechnology> GetRandomList(int n, Random random)
        {
            var list = new List<AdavanceTechnology>()
            {
                new ATT1(),
                new ATT2(),
                new ATT3(),
                new ATT4(),
                new ATT5(),
                new ATT6(),
                new ATT7(),
                new ATT8(),
                new ATT9(),
                new ATT10(),
                new ATT11(),
                new ATT12(),
                new ATT13(),
                new ATT14(),
                new ATT15(),
            };

            var result = new List<AdavanceTechnology>();
            for (int i = 0; i < n; i++)
            {
                result.Add(list.RandomRemove(random));
            }
            return result;
        }
    }
    public abstract class AdavanceTechnology : GameTiles
    {
        public AdavanceTechnology()
        {
            isPicked = false;

            this.name = this.GetType().Name;
            this.typename = "att";
            this.showRank = 3;

        }
        public virtual bool isPicked { set; get; }
    }

    public class ATT1 : AdavanceTechnology
    {
        public override string desc
        {
            get
            {
                return "Act:3O";
            }
        }

        public override bool InvokeGameTileAction(Faction faction)
        {
            faction.Ore += 3;

            return base.InvokeGameTileAction(faction);
        }

        public override bool CanAction => true;
    }
    public class ATT2 : AdavanceTechnology
    {
        public override string desc
        {
            get
            {
                return "Act:3K";
            }
        }

        public override bool InvokeGameTileAction(Faction faction)
        {
            faction.Knowledge += 3;

            return base.InvokeGameTileAction(faction);
        }

        public override bool CanAction => true;
    }
    public class ATT3 : AdavanceTechnology
    {
        public override string desc
        {
            get
            {
                return "Act:1Q,5C";
            }
        }

        public override bool InvokeGameTileAction(Faction faction)
        {
            faction.QICs += 1;
            faction.Credit += 5;
            return base.InvokeGameTileAction(faction);
        }
        public override bool CanAction => true;
    }
    public class ATT4 : AdavanceTechnology
    {
        public override string desc
        {
            get
            {
                return "M>>3VP";
            }
        }

        public override int GetTriggerScore => 3;


    }
    public class ATT5 : AdavanceTechnology
    {
        public override string desc
        {
            get
            {
                return "TC>>3VP";
            }
        }
        public override int GetTriggerScore => 3;

    }
    public class ATT6 : AdavanceTechnology
    {
        public override string desc
        {
            get
            {
                return "RA>>2VP";
            }
        }

        public override int GetTriggerScore => 2;
    }
    public class ATT7 : AdavanceTechnology
    {
        public override string desc
        {
            get
            {
                return "pass-vp:AL*3";
            }
        }

        public override int GetTurnEndScore(Faction faction)
        {
            return faction.GameTileList.Where(x => x is AllianceTile).Count() * 3;
        }
    }
    public class ATT8 : AdavanceTechnology
    {
        public override string desc
        {
            get
            {
                return "pass-vp:P_type*1";
            }
        }

        public override int GetTurnEndScore(Faction faction)
        {
            return faction.GetPlanetTypeCount();
        }
    }
    public class ATT9 : AdavanceTechnology
    {
        public override string desc
        {
            get
            {
                return "pass-vp:RL*3";
            }
        }
        public override int GetTurnEndScore(Faction faction)
        {
            return (GameConstNumber.ResearchLabCount - faction.ResearchLabs.Count) * 3;
        }

    }
    public class ATT10 : AdavanceTechnology
    {
        public override string desc
        {
            get
            {
                return "1SC->1O";
            }
        }

        public override bool OneTimeAction(Faction faction)
        {
            faction.Ore += faction.GetSpaceSectorCount();
            return true;
        }
    }
    public class ATT11 : AdavanceTechnology
    {
        public override string desc
        {
            get
            {
                return "1M->2VP";
            }
        }
        public override bool OneTimeAction(Faction faction)
        {
            faction.Score += (GameConstNumber.MineCount - faction.Mines.Count) * 2;
            return true;
        }
    }
    public class ATT12 : AdavanceTechnology
    {
        public override string desc
        {
            get
            {
                return "1TC->4VP";
            }
        }

        public override bool OneTimeAction(Faction faction)
        {
            faction.Score += (GameConstNumber.TradeCenterCount - faction.TradeCenters.Count) * 4;
            return true;
        }
    }
    public class ATT13 : AdavanceTechnology
    {
        public override string desc
        {
            get
            {
                return "1G->2VP";
            }
        }

        public override bool OneTimeAction(Faction faction)
        {
            faction.Score += faction.GaiaPlanetNumber * 2;
            return true;
        }
    }
    public class ATT14 : AdavanceTechnology
    {
        public override string desc
        {
            get
            {
                return "1SC->2VP";
            }
        }
        public override bool OneTimeAction(Faction faction)
        {
            faction.Score += faction.GetSpaceSectorCount() * 2;
            return true;
        }

    }
    public class ATT15 : AdavanceTechnology
    {
        public override string desc
        {
            get
            {
                return "1AL->5VP";
            }
        }

        public override bool OneTimeAction(Faction faction)
        {
            faction.Score += faction.GameTileList.Where(x => x is AllianceTile).Count() * 3;
            return true;
        }
    }
}
