using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using GaiaCore.Util;

namespace GaiaCore.Gaia.Tiles
{
    public static class FSTMgr
    {
        public static List<FinalScoring> GetRandomList(int n, Random random)
        {
            var list = new List<FinalScoring>()
            {
                new FST1(),
                new FST2(),
                new FST3(),
                new FST4(),
                new FST5(),
                new FST6(),
            };
            var result = new List<FinalScoring>();
            for (int i = 0; i < n; i++)
            {
                result.Add(list.RandomRemove(random));
            }
            return result;
        }
    }
    public abstract class FinalScoring : GameTiles
    {
        public virtual bool InvokeGameTileAction(List<Faction> factionList)
        {
            var scorequeue = new Queue<int>();
            scorequeue.Enqueue(18);
            scorequeue.Enqueue(12);
            scorequeue.Enqueue(6);
            scorequeue.Enqueue(0);
            foreach (var item in factionList.GroupBy(x => TargetNumber(x)).OrderByDescending(x => x.Key))
            {
                var total = item.ToList().Sum(x => scorequeue.Dequeue());
                item.ToList().ForEach(x => x.FinalEndScore += total / item.Count());
            }
            return true;
        }
        public abstract int TargetNumber(Faction faction);
    }

    public class FST1 : FinalScoring
    {
        public override string desc
        {
            get
            {
                return "自己建筑数量最多";
            }
        }

        public override int TargetNumber(Faction faction)
        {
            return faction.GetBuildCount();
        }
    }

    public class FST2 : FinalScoring
    {
        public override string desc
        {
            get
            {
                return "自己所有联盟内建筑数量最多";
            }
        }

        public override int TargetNumber(Faction faction)
        {
            return faction.GetAllianceBuilding();
        }
    }

    public class FST3 : FinalScoring
    {
        public override string desc
        {
            get
            {
                return "自己建筑所处星球种类最多";
            }
        }

        public override int TargetNumber(Faction faction)
        {
            return faction.GetPlanetTypeCount();
        }
    }

    public class FST4 : FinalScoring
    {
        public override string desc
        {
            get
            {
                return "自己建筑所处盖亚星球数量最多";
            }
        }

        public override int TargetNumber(Faction faction)
        {
            return faction.GaiaPlanetNumber;
        }
    }

    public class FST5 : FinalScoring
    {
        public override string desc
        {
            get
            {
                return "自己建筑所处星域数量最多";
            }
        }

        public override int TargetNumber(Faction faction)
        {
            return faction.GetSpaceSectorCount();
        }
    }

    public class FST6 : FinalScoring
    {
        public override string desc
        {
            get
            {
                return "自己卫星数量最多";
            }
        }

        public override int TargetNumber(Faction faction)
        {
            return faction.GetSatelliteCount();
        }
    }
}
