using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class Terraner:Faction
    {
        public Terraner():base(FactionName.Terraner)
        {

        }
        public override Terrain OGTerrain { get => Terrain.Blue; }
    }
}
