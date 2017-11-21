using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class Hive : Faction
    {
        public Hive(GaiaGame gg) :base(FactionName.Hive, gg)
        {
            this.ChineseName = "蜂人";
            this.ColorCode = colorList[1];
            this.ColorMap = colorMapList[1];
        }
        public override Terrain OGTerrain { get => Terrain.Red; }

        protected override int CalQICIncome()
        {
            return base.CalQICIncome() + 1;
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
            return true;
        }

        public override bool FinishIntialMines()
        {
            return StrongHold == null;
        }
    }
}
