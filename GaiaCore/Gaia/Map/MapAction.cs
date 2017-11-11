using GaiaCore.Gaia.Tiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class MapActionMgr
    {
        public List<MapAction> mapActList = new List<MapAction>()
        {
            new ACT1(),
            new ACT2(),
            new ACT3(),
            new ACT4(),
            new ACT5(),
            new ACT6(),
            new ACT7(),
            new ACT8(),
            new ACT9(),
            new ACT10()
        };
        internal void AddMapActionList(Dictionary<string, Func<Faction, bool>> actionList, Dictionary<string, Func<Faction, bool>> preList)
        {
            mapActList.ForEach(x => actionList.Add(x.GetType().Name.ToLower(), x.InvokeGameTileAction));
            mapActList.ForEach(x => preList.Add(x.GetType().Name.ToLower(), x.PredicateGameTileAction));
        }

        internal void Reset()
        {
            mapActList.ForEach(x => x.IsUsed = false);
        }
    }
    public abstract class MapAction : GameTiles
    {
        protected abstract int ResourceCost { get; }
        public override string desc => GetType().Name;
        public override bool InvokeGameTileAction(Faction faction)
        {
            return base.InvokeGameTileAction(faction);
        }
    }
    public class ACT1 : MapAction
    {
        public override string desc => "7pw→3k";
        protected override int ResourceCost => 7;
        public override bool PredicateGameTileAction(Faction faction)
        {
            return base.PredicateGameTileAction(faction) && faction.PowerToken3 >= ResourceCost;
        }
        public override bool InvokeGameTileAction(Faction faction)
        {
            faction.PowerUse(ResourceCost);
            faction.Knowledge += 3;
            return base.InvokeGameTileAction(faction);
        }
    }

    public class ACT2 : MapAction
    {
        public override string desc => "5pw→2tf";
        protected override int ResourceCost => 5;
        public override bool PredicateGameTileAction(Faction faction)
        {
            return base.PredicateGameTileAction(faction) && faction.PowerToken3 >= ResourceCost;
        }
        public override bool InvokeGameTileAction(Faction faction)
        {
            Action action = () =>
            {
                faction.PowerUse(ResourceCost);
            };
            faction.ActionQueue.Enqueue(action);
            faction.TerraFormNumber+=2;
            faction.IsUseAction2=true;
            return base.InvokeGameTileAction(faction);
        }
    }

    public class ACT3 : MapAction
    {
        public override string desc => "4pw→2o";
        protected override int ResourceCost => 4;
        public override bool PredicateGameTileAction(Faction faction)
        {
            return base.PredicateGameTileAction(faction) && faction.PowerToken3 >= ResourceCost;
        }
        public override bool InvokeGameTileAction(Faction faction)
        {
            faction.PowerUse(ResourceCost);
            faction.Ore += 2;
            return base.InvokeGameTileAction(faction);
        }
    }
    public class ACT4 : MapAction
    {
        public override string desc => "4pw→7c";
        protected override int ResourceCost => 4;
        public override bool PredicateGameTileAction(Faction faction)
        {
            return base.PredicateGameTileAction(faction) && faction.PowerToken3 >= ResourceCost;
        }
        public override bool InvokeGameTileAction(Faction faction)
        {
            faction.PowerUse(ResourceCost);
            faction.Credit += 7;
            return base.InvokeGameTileAction(faction);
        }
    }
    public class ACT5 : MapAction
    {
        public override string desc => "4pw→2k";
        protected override int ResourceCost => 4;
        public override bool PredicateGameTileAction(Faction faction)
        {
            return base.PredicateGameTileAction(faction) && faction.PowerToken3 >= ResourceCost;
        }
        public override bool InvokeGameTileAction(Faction faction)
        {
            faction.PowerUse(ResourceCost);
            faction.Knowledge += 2;
            return base.InvokeGameTileAction(faction);
        }
    }
    public class ACT6 : MapAction
    {
        public override string desc => "3pw→1tf";
        protected override int ResourceCost => 3;
        public override bool PredicateGameTileAction(Faction faction)
        {
            return base.PredicateGameTileAction(faction) && faction.PowerToken3 >= ResourceCost;
        }
        public override bool InvokeGameTileAction(Faction faction)
        {
            Action action = () =>
            {
                faction.PowerUse(ResourceCost);
            };
            faction.ActionQueue.Enqueue(action);
            faction.TerraFormNumber += 1;
            return base.InvokeGameTileAction(faction);
        }
    }
    public class ACT7 : MapAction
    {
        public override string desc => "3pw→2pwt";
        protected override int ResourceCost => 3;
        public override bool PredicateGameTileAction(Faction faction)
        {
            return base.PredicateGameTileAction(faction) && faction.PowerToken3 >= ResourceCost;
        }
        public override bool InvokeGameTileAction(Faction faction)
        {
            faction.PowerUse(ResourceCost);
            faction.PowerToken1 += 2;
            return base.InvokeGameTileAction(faction);
        }
    }
    public class ACT8 : MapAction
    {
        public override string desc => "4Q→1stt/att";
        protected override int ResourceCost => 4;
        public override bool PredicateGameTileAction(Faction faction)
        {
            faction.TechTilesGet++;
            return base.PredicateGameTileAction(faction) && faction.QICs >= ResourceCost;
        }
        public override bool InvokeGameTileAction(Faction faction)
        {
            faction.QICs -= ResourceCost;
            return base.InvokeGameTileAction(faction);
        }
    }
    public class ACT9 : MapAction
    {
        public override string desc => "3Q→score 1alt";
        protected override int ResourceCost => 3;
        public override bool PredicateGameTileAction(Faction faction)
        {
            return base.PredicateGameTileAction(faction) && faction.QICs >= ResourceCost;
        }
        public override bool InvokeGameTileAction(Faction faction)
        {
            faction.QICs -= ResourceCost;
            faction.AllianceTileReGet++;
            return base.InvokeGameTileAction(faction);
        }
    }
    public class ACT10 : MapAction
    {
        public override string desc => "2Q→3vp+1 P_type->1vp";
        protected override int ResourceCost => 2;
        public override bool PredicateGameTileAction(Faction faction)
        {
            return base.PredicateGameTileAction(faction) && faction.QICs >= ResourceCost;
        }
        public override bool InvokeGameTileAction(Faction faction)
        {
            faction.QICs -= ResourceCost;
            faction.Score += 3 + faction.GetPlanetTypeCount();
            return base.InvokeGameTileAction(faction);
        }
    }

}
