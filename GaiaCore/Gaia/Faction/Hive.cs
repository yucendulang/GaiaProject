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

        }
        public override Terrain OGTerrain { get => Terrain.Red; }
    }
}
