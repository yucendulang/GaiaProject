using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class Itar : Faction
    {
        public Itar(GaiaGame gg) :base(FactionName.Itar, gg)
        {

        }
        public override Terrain OGTerrain { get => Terrain.White; }
    }
}
