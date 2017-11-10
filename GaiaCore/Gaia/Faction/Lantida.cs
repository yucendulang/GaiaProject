using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class Lantida : Faction
    {
        public Lantida(GaiaGame gg) :base(FactionName.Lantida, gg)
        {

        }
        public override Terrain OGTerrain { get => Terrain.Blue; }
    }
}
