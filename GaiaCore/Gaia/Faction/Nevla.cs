using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class Nevla : Faction
    {
        public Nevla(GaiaGame gg) :base(FactionName.Nevla, gg)
        {
            this.ChineseName = "超星人";
            this.ColorCode = colorList[6];
            this.ColorMap = colorMapList[6];


        }
        public override Terrain OGTerrain { get => Terrain.White; }
    }
}
