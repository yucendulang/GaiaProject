using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class Xenos : Faction
    {
        public Xenos(GaiaGame gg) :base(FactionName.Xenos, gg)
        {

        }
        public override Terrain OGTerrain { get => Terrain.Yellow; }
    }
}
