using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class Taklons : Faction
    {
        public Taklons():base(FactionName.Taklons)
        {

        }
        public override Terrain OGTerrain { get => Terrain.Brown; }
    }
}
