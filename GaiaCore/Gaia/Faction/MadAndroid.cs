using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class MadAndroid : Faction
    {
        public MadAndroid(GaiaGame gg) :base(FactionName.MadAndroid, gg)
        {
            this.ChineseName = "疯狂机器";
            this.ColorCode = colorList[5];

        }
        public override Terrain OGTerrain { get => Terrain.Gray; }
    }
}
