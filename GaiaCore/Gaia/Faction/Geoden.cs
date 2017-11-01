using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class Geoden : Faction
    {
        public Geoden():base(FactionName.Geoden)
        {

        }
        public override Terrain OGTerrain { get => Terrain.Orange; }
    }
}
