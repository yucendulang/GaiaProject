using GaiaCore.Gaia.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GaiaCore.Gaia
{
    public class Hive : Faction
    {
        public Hive(GaiaGame gg) : base(FactionName.Hive, gg)
        {
            this.ChineseName = "蜂人";
            this.ColorCode = colorList[1];
            this.ColorMap = colorMapList[1];
            AllianceList = new List<Tuple<int, int>>();
        }

        public List<Tuple<int, int>> AllianceList { set; get; }
        public override Terrain OGTerrain { get => Terrain.Red; }

        protected override int CalQICIncome()
        {
            return base.CalQICIncome() + 1;
        }
        public override bool BuildMine(Map map, int row, int col, out string log)
        {
            if (FactionSpecialAbility > 0)
            {
                return ExcuteSHAbility(new Tuple<int, int>(row, col), out log);
            }
            return base.BuildMine(map, row, col, out log);
        }
        public override bool BuildIntialMine(Map map, int row, int col, out string log)
        {
            log = string.Empty;
            if (!(map.HexArray[row, col].OGTerrain == OGTerrain))
            {
                log = "地形不符";
                return false;
            }
            if (!(map.HexArray[row, col].Building == null && map.HexArray[row, col].FactionBelongTo == null))
            {
                log = "该地点已经有人占领了";
                return false;
            }

            map.HexArray[row, col].Building = StrongHold;
            map.HexArray[row, col].FactionBelongTo = FactionName;
            StrongHold = null;
            AllianceList.Add(new Tuple<int, int>(row, col));
            CallSpecialSHBuild();
            return true;
        }

        public override bool FinishIntialMines()
        {
            return StrongHold == null;
        }

        public override bool ForgingAllianceCheck(List<Tuple<int, int>> list, out string log)
        {
            log = string.Empty;
            var map = GaiaGame.Map;
            if (list != null)
            {
                var SatelliteHexList = list.Where(x => GaiaGame.Map.HexArray[x.Item1, x.Item2].TFTerrain == Terrain.Empty && !GaiaGame.Map.HexArray[x.Item1, x.Item2].Satellite.Contains(FactionName))
                        .ToList();
                if (SatelliteHexList.Count != list.Count)
                {
                    log = "峰人的指令只要标记出卫星位置即可,如果不需要放置卫星请直接使用Alliance.+ATLX即可";
                    return false;
                }
                if (QICs < SatelliteHexList.Count)
                {
                    log = string.Format("QIC总数量为{0},放卫星数量为{1},QIC不够", QICs, SatelliteHexList.Count);
                    return false;
                }

                if (list.Exists(x =>
                {
                    TerrenHex terrenHex = GaiaGame.Map.HexArray[x.Item1, x.Item2];
                    return terrenHex.Satellite.Contains(FactionName);
                }))
                {
                    log = "有地块已经放置过卫星";
                    return false;
                }
                int oldAllianceCount;
                var allianceListClone = new List<Tuple<int, int>>(AllianceList);

                //allianceListClone.AddRange(list);
                do
                {
                    oldAllianceCount = allianceListClone.Count;
                    var hexlist = map.GetHexListForBuildingAndSatellite(FactionName, list);
                    var newNeighboor = hexlist.Where(x => !allianceListClone.Contains(x)).ToList().FindAll(x => allianceListClone.Exists(y => map.CalTwoHexDistance(x.Item1, x.Item2, y.Item1, y.Item2) == 1));
                    allianceListClone.AddRange(newNeighboor);
                }
                while (oldAllianceCount != allianceListClone.Count);
                var altNum = GameTileList.Count(x => x is AllianceTile) - (m_TransformLevel == 5 ? 1 : 0) + 1;
                if (allianceListClone.Sum(x => (map.GetHex(x).Building?.MagicLevel).GetValueOrDefault() + (map.GetHex(x).IsSpecialSatelliteForHive ? 1 : 0)) < m_allianceMagicLevel * altNum)
                {
                    log = string.Format("主城等级为{0},魔力等级不够{1}级", allianceListClone.Sum(x => map.GetHex(x).Building?.MagicLevel), m_allianceMagicLevel * altNum);
                    return false;
                }
            }
            else
            {
                var altNum = GameTileList.Count(x => x is AllianceTile) - (m_TransformLevel == 5 ? 1 : 0) + 1;
                if (GetMainAllianceGrade() < m_allianceMagicLevel * altNum)
                {
                    log = string.Format("主城等级为{0},魔力等级不够{1}级", GetMainAllianceGrade(), m_allianceMagicLevel * altNum);
                    return false;
                }
            }
            return true;
        }

        public override void ForgingAllianceGetTiles(List<Tuple<int, int>> list)
        {
            if (list == null)
            {
                list = new List<Tuple<int, int>>();
            }
            int oldAllianceCount;
            var allianceListClone = new List<Tuple<int, int>>(AllianceList);
            //allianceListClone.AddRange(list);
            do
            {
                oldAllianceCount = allianceListClone.Count;
                var hexlist = GaiaGame.Map.GetHexListForBuildingAndSatellite(FactionName, list);
                var newNeighboor = hexlist.Where(x => !allianceListClone.Contains(x)).ToList().FindAll(x => allianceListClone.Exists(y => GaiaGame.Map.CalTwoHexDistance(x.Item1, x.Item2, y.Item1, y.Item2) == 1));
                allianceListClone.AddRange(newNeighboor);
            }
            while (oldAllianceCount != allianceListClone.Count);

            list.AddRange(allianceListClone.Where(x => GaiaGame.Map.GetHex(x).TFTerrain != Terrain.Empty && GaiaGame.Map.GetHex(x).IsAlliance == false).ToList());
            base.ForgingAllianceGetTiles(list);
        }

        public int GetMainAllianceGrade()
        {
            int oldAllianceCount;
            do
            {
                oldAllianceCount = AllianceList.Count;
                var hexlist = GaiaGame.Map.GetHexListForBuildingAndSatellite(FactionName);
                var newNeighboor = hexlist.Where(x => !AllianceList.Contains(x)).ToList().FindAll(x => AllianceList.Exists(y => GaiaGame.Map.CalTwoHexDistance(x.Item1, x.Item2, y.Item1, y.Item2) == 1));
                AllianceList.AddRange(newNeighboor);
            }
            while (oldAllianceCount != AllianceList.Count);
            AllianceList.Where(x => GaiaGame.Map.GetHex(x).TFTerrain != Terrain.Empty && GaiaGame.Map.GetHex(x).IsAlliance == false).ToList().ForEach(x => GaiaGame.Map.GetHex(x).IsAlliance = true);
            return AllianceList.Sum(x => (GaiaGame.Map.GetHex(x).Building?.MagicLevel).GetValueOrDefault() + (GaiaGame.Map.GetHex(x).IsSpecialSatelliteForHive ? 1 : 0));
        }
        protected override void CallSpecialSHBuild()
        {
            AddGameTiles(new Hiv());
            base.CallSpecialSHBuild();
        }

        public bool ExcuteSHAbility(Tuple<int, int> pos, out string log)
        {
            log = string.Empty;
            var map = GaiaGame.Map;
            if (map.GetHex(pos) == null)
            {
                log = "出界了";
                return false;
            }
            if (map.GetHex(pos).TFTerrain != Terrain.Empty)
            {
                log = "只有空地才可以放置空间站";
                return false;
            }
            if (map.GetHex(pos).Satellite.Contains(FactionName))
            {
                log = "放置过卫星的地方不能放置空间站";
                return false;
            }
            var distanceNeed = GaiaGame.Map.CalShipDistanceNeed(pos.Item1, pos.Item2, FactionName);
            var qicship = Math.Max((distanceNeed - GetShipDistance + 1) / 2, 0);
            if (QICs * 2 < distanceNeed - GetShipDistance)
            {
                log = string.Format("空间站距离太偏远了,需要{0}个Q来加速", Math.Max((distanceNeed - GetShipDistance + 1) / 2, 0));
                return false;
            }
            ActionQueue.Enqueue(() =>
            {
                map.GetHex(pos).AddSatellite(FactionName);
                map.GetHex(pos).IsSpecialSatelliteForHive = true;
                QICs -= qicship;
            });
            FactionSpecialAbility--;
            return true;
        }
        public class Hiv : MapAction
        {
            public override string desc => "SH能力";
            protected override int ResourceCost => 0;
            public override bool CanAction => true;
            public override bool InvokeGameTileAction(Faction faction)
            {
                faction.FactionSpecialAbility++;
                faction.ActionQueue.Enqueue(() =>
                {
                    base.InvokeGameTileAction(faction);
                });
                return true;
            }
        }
    }
}
