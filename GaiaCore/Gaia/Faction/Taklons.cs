using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class Taklons : Faction
    {
        public Taklons(GaiaGame gg) :base(FactionName.Taklons,gg)
        {

        }
        public override Terrain OGTerrain { get => Terrain.Brown; }
    }
}
