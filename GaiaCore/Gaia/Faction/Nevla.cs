using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class Nevla : Faction
    {
        public Nevla(GaiaGame gg) :base(FactionName.Nevla, gg)
        {

        }
        public override Terrain OGTerrain { get => Terrain.White; }
    }
}
