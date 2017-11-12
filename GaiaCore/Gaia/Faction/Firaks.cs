using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class Firaks : Faction
    {
        public Firaks(GaiaGame gg) :base(FactionName.Firaks, gg)
        {
            this.ChineseName = "章鱼人";
            this.ColorCode = colorList[5];

        }
        public override Terrain OGTerrain { get => Terrain.Gray; }
    }
}
