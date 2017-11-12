using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class HadschHalla : Faction
    {
        public HadschHalla(GaiaGame gg) :base(FactionName.HadschHalla, gg)
        {
            this.ChineseName = "圣禽族";
            this.ColorCode = colorList[1];

        }
        public override Terrain OGTerrain { get => Terrain.Red; }
    }
}
