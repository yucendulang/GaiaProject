using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using GaiaCore.Util;
using GaiaCore.Gaia.Resources;

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
            var scoreList = new List<Faction>(factionList);
            if (scoreList.Count == 2)
            {
                var virtualPlayer = new VirtualPlayerFaction(FactionName.Ambas,null);
                scoreList.Add(virtualPlayer);
            }

            foreach (var item in scoreList.GroupBy(x => TargetNumber(x)).OrderByDescending(x => x.Key))
            {
                var total = item.ToList().Sum(x => scorequeue.Dequeue());
                item.ToList().ForEach(x =>
                {
                    if (TargetNumber(x) != 0)
                    {
                        x.FinalEndScore += total / item.Count();
                    }
                });
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
                return Messages.FSMaxBuildings;
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
                return  Messages.FSFederationBuildings;
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
                return Messages.FSPlanetTypes;
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
                return Messages.FSGaiaPlanets;
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
                return Messages.FSSpaceSector;
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
                return Messages.FSSatelites;
            }
        }

        public override int TargetNumber(Faction faction)
        {
            return faction.GetSatelliteCount();
        }
    }
}
