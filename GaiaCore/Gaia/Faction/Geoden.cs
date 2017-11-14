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
            this.ColorCode = colorList[3];
            if (gg != null)
            {
                IncreaseTech("tf");
            }
        }
        public override Terrain OGTerrain { get => Terrain.Orange; }

        internal override bool BuildMine(Map map, int row, int col, out string log)
        {
            if (StrongHold == null)
            {
                if (!IsPlanetTypeExist(map.HexArray[row, col].TFTerrain))
                {
                    Action action = () =>
                    {
                        Knowledge += 3;
                    };
                    ActionQueue.Enqueue(action);
                }
            }
            return base.BuildMine(map, row, col, out log);
        }
    }
}
