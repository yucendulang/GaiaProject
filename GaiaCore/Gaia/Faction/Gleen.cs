using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class Gleen : Faction
    {
        public Gleen(GaiaGame gg) :base(FactionName.Geoden,gg)
        {

        }
        public override Terrain OGTerrain { get => Terrain.Yellow; }
    }
}
