using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class BalTak : Faction
    {
        public BalTak(GaiaGame gg) :base(FactionName.BalTak, gg)
        {

        }
        public override Terrain OGTerrain { get => Terrain.Orange; }
    }
}
