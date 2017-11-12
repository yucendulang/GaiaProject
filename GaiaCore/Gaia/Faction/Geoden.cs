using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class Geoden : Faction
    {
        public Geoden(GaiaGame gg) :base(FactionName.Geoden,gg)
        {
            this.ChineseName = "晶矿星人";

        }
        public override Terrain OGTerrain { get => Terrain.Orange; }
    }
}
