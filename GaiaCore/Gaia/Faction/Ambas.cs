using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class Ambas : Faction
    {
        public Ambas(GaiaGame gg) :base(FactionName.Ambas, gg)
        {

        }
        public override Terrain OGTerrain { get => Terrain.Brown; }
    }
}
