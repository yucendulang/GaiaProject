using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GaiaCore.Gaia
{
    public class Geoden : Faction
    {
        public Geoden(GaiaGame gg) : base(FactionName.Geoden, gg)
        {
            this.ChineseName = "晶矿星人";
            this.ColorCode = colorList[2];
            this.ColorMap = colorMapList[2];

            if (gg != null)
            {
                IncreaseTech("tf");
            }
        }
        public override Terrain OGTerrain { get => Terrain.Orange; }

        public override bool BuildMine(Map map, int row, int col, out string log)
        {
            GetThreeKnowledge(row, col);
            return base.BuildMine(map, row, col, out log);
        }

        private void GetThreeKnowledge(int row, int col)
        {
            if (StrongHold == null)
            {
                var hexList = GaiaGame.Map.GetHexList().ToList();
                var hex = GaiaGame.Map.GetHex(row, col);
                if (!hexList.Exists(x => x.FactionBelongTo == FactionName && x.TFTerrain == hex.TFTerrain)){
                    TempKnowledge += 3;

                    Action action = () =>
                    {
                        Knowledge = Knowledge;
                    };
                    ActionQueue.Enqueue(action);
                }
            }
        }

        public override bool BuildBlackPlanet(int row, int col, out string log)
        {
            GetThreeKnowledge(row, col);
            return base.BuildBlackPlanet(row, col, out log);
        }
    }
}
