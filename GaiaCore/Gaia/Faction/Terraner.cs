using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class Terraner:Faction
    {
        public Terraner(GaiaGame gg) :base(FactionName.Terraner, gg)
        {

        }
        public override Terrain OGTerrain { get => Terrain.Blue; }
    }
}
