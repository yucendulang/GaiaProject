using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class Itar : Faction
    {
        public Itar(GaiaGame gg) :base(FactionName.Itar, gg)
        {
            m_ore += 1;
            m_powerToken1 += 2;
        }

        protected override void CallAC1Income()
        {
            Knowledge += 3;
        }
        public override Terrain OGTerrain { get => Terrain.White; }
    }
}
