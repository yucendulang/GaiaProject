using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class Gleen : Faction
    {
        public Gleen(GaiaGame gg) :base(FactionName.Gleen, gg)
        {
            this.ChineseName = "格伦星人";
            this.ColorCode = colorList[3];

        }
        public override Terrain OGTerrain { get => Terrain.Yellow; }
    }
}
