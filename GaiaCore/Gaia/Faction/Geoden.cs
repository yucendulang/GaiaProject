using System;
using System.Collections.Generic;
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

        internal override bool BuildMine(Map map, int row, int col, out string log)
        {
            GetThreeKnowledge(row, col);
            return base.BuildMine(map, row, col, out log);
        }

        private void GetThreeKnowledge(int row, int col)
        {
            if (StrongHold == null)
            {
                if (!IsPlanetTypeExist(GaiaGame.Map.HexArray[row, col].TFTerrain))
                {
                    Action action = () =>
                    {
                        Knowledge += 3;
                    };
                    ActionQueue.Enqueue(action);
                }
            }
        }

        internal override bool BuildBlackPlanet(int row, int col, out string log)
        {
            GetThreeKnowledge(row, col);
            return BuildBlackPlanet(row, col, out log);
        }
    }
}
