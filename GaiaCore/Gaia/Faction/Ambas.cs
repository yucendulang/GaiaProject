using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class Ambas : Faction
    {
        public Ambas(GaiaGame gg) :base(FactionName.Ambas, gg)
        {
            this.ChineseName = "大使星人";
            this.ColorCode = colorList[4];

        }
        public override Terrain OGTerrain { get => Terrain.Brown; }
    }
}
