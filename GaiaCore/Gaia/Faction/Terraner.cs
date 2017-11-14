using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class Terraner:Faction
    {
        public Terraner(GaiaGame gg) :base(FactionName.Terraner, gg)
        {
            this.ChineseName = "人类";
            this.ColorCode = colorList[0];
            if (gg != null)
            {
                IncreaseTech("gaia");
            }

        }
        public override Terrain OGTerrain { get => Terrain.Blue; }

        internal override void GaiaPhaseIncome()
        {
            PowerToken2 += PowerTokenGaia;
            PowerTokenGaia = 0;
        }
    }
}
