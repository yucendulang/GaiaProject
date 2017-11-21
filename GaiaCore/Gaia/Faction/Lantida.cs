using GaiaCore.Gaia.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GaiaCore.Gaia
{
    public class Lantida : Faction
    {
        public Lantida(GaiaGame gg) :base(FactionName.Lantida, gg)
        {
            this.ChineseName = "亚特兰斯星人";
            this.ColorCode = colorList[0]; this.ColorMap = colorMapList[0];
            PowerToken1 += 2;
            PowerToken2 -= 4;
            Credit -= 2;
        }
        public override Terrain OGTerrain { get => Terrain.Blue; }
        protected override int CallSHPowerTokenIncome()
        {
            return 0;
        }

        public override bool BuildMine(Map map, int row, int col, out string log)
        {
            if(map.HexArray[row, col].Building!=null&& map.HexArray[row, col].FactionBelongTo!=FactionName&&!(map.HexArray[row,col].Building is GaiaBuilding))
            {
                ///进入Lantida技能阶段
                log = string.Empty;
                if (map.HexArray[row, col] == null)
                {
                    log = "出界了兄弟";
                    return false;
                }
                //航海的Q
                int QSHIP = 0;
                if (Mines.Count < 1)
                {
                    log = "已经没有可用的矿场了";
                    return false;
                }
                if (!(Credit >= m_MineCreditCost && Ore >= m_MineOreCost))
                {
                    log = "资源不够";
                    return false;
                }

                var distanceNeed = map.CalShipDistanceNeed(row, col, FactionName);

                if (QICs * 2 < distanceNeed - GetShipDistance)
                {
                    log = string.Format("建筑距离太偏远了,需要{0}个Q来加速", (distanceNeed - GetShipDistance + 1) / 2);
                    return false;
                }
                QSHIP = Math.Max((distanceNeed - GetShipDistance + 1) / 2, 0);
                //扣资源建建筑
                Action queue = () =>
                {
                    Ore -= m_MineOreCost;
                    Credit -= m_MineCreditCost;
                    map.HexArray[row, col].SpecialBuilding = Mines.First();
                    Mines.RemoveAt(0);
                    GaiaGame.SetLeechPowerQueue(FactionName, row, col);
                    TriggerRST(typeof(RST1));
                    TriggerRST(typeof(ATT4));
                    QICs -= QSHIP;
                };
                ActionQueue.Enqueue(queue);
                TempShip = 0;
                return true;
            }
            return base.BuildMine(map, row, col, out log);
        }

        public override int GetSpaceSectorCount()
        {
            var hexList = GaiaGame.Map.GetHexList();
            var q =
                    from h in hexList
                    where (h.FactionBelongTo == FactionName && !(h.Building is GaiaBuilding)) || h.SpecialBuilding != null
                    group h by h.SpaceSectorName into g
                    select g;
            return q.Count();
        }

        public override int GetAllianceBuilding()
        {
            var hexList = GaiaGame.Map.GetHexList();
            var q =
                from h in hexList
                where (h.FactionBelongTo == FactionName && h.IsAlliance == true && !(h.Building is GaiaBuilding)) ||
                (h.SpecialBuilding != null && h.IsSpecialBuildingAlliance == true)
                select h;
            return q.Count();
        }
    }
}
