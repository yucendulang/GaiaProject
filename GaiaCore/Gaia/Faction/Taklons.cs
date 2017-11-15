using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class Taklons : Faction
    {
        public Taklons(GaiaGame gg) :base(FactionName.Taklons,gg)
        {
            this.ChineseName = "利爪族";
            this.ColorCode = colorList[4];
            this.ColorMap = colorMapList[4];

        }
        public override Terrain OGTerrain { get => Terrain.Brown; }
    }
}
