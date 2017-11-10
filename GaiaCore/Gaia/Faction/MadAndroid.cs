using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class MadAndroid : Faction
    {
        public MadAndroid(GaiaGame gg) :base(FactionName.MadAndroid, gg)
        {

        }
        public override Terrain OGTerrain { get => Terrain.Black; }
    }
}
