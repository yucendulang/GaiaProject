using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace GaiaCore.Gaia.Tiles
{

    public abstract class FinalScoring : GameTiles
    {
        public abstract bool InvokeGameTileAction(List<Faction> factionList);
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
        public override bool InvokeGameTileAction(List<Faction> factionList)
        {
            var scorequeue = new Queue<int>();
            scorequeue.Enqueue(18);
            scorequeue.Enqueue(12);
            scorequeue.Enqueue(6);
            scorequeue.Enqueue(0);
            foreach (var item in factionList.GroupBy(x => x.GetRemainBuildCount()).OrderBy(x => x.Key))
            {
                var total = item.ToList().Sum(x => scorequeue.Dequeue());
                item.ToList().ForEach(x => x.Score += total / item.Count());
            }
            return true;
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

        public override bool InvokeGameTileAction(List<Faction> factionList)
        {
            var scorequeue = new Queue<int>();
            scorequeue.Enqueue(18);
            scorequeue.Enqueue(12);
            scorequeue.Enqueue(6);
            scorequeue.Enqueue(0);
            foreach (var item in factionList.GroupBy(x => x.GetPlanetTypeCount()).OrderByDescending(x => x.Key))
            {
                var total = item.ToList().Sum(x => scorequeue.Dequeue());
                item.ToList().ForEach(x => x.Score += total / item.Count());
            }
            return true;
        }
    }
}
