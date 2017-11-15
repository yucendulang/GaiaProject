using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class BalTak : Faction
    {
        public BalTak(GaiaGame gg) :base(FactionName.BalTak, gg)
        {
            this.ChineseName = "炽炎族";
            this.ColorCode = colorList[3];
            this.ColorMap = colorMapList[3];


        }
        public override Terrain OGTerrain { get => Terrain.Orange; }
    }
}
