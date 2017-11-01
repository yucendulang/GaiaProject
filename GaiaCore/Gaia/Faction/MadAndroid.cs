using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class MadAndroid : Faction
    {
        public MadAndroid():base(FactionName.MadAndroid)
        {

        }
        public override Terrain OGTerrain { get => Terrain.Black; }
    }
}
