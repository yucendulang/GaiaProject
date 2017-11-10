using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class HadschHalla : Faction
    {
        public HadschHalla(GaiaGame gg) :base(FactionName.Geoden,gg)
        {

        }
        public override Terrain OGTerrain { get => Terrain.Red; }
    }
}
