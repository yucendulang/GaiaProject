using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class Hive : Faction
    {
        public Hive(GaiaGame gg) :base(FactionName.Hive, gg)
        {

        }
        public override Terrain OGTerrain { get => Terrain.Red; }
    }
}
