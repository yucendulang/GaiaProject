using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class Lantida : Faction
    {
        public Lantida(GaiaGame gg) :base(FactionName.Lantida, gg)
        {
            this.ChineseName = "亚特兰斯星人";
            this.ColorCode = colorList[0];
        }
        public override Terrain OGTerrain { get => Terrain.Blue; }
    }
}
