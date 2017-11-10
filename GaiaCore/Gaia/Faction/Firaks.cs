using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class Firaks : Faction
    {
        public Firaks(GaiaGame gg) :base(FactionName.Firaks, gg)
        {

        }
        public override Terrain OGTerrain { get => Terrain.Gray; }
    }
}
