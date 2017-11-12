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

        protected override void CallSpecialFreeIncome()
        {
            m_powerToken1++;
        }

        public override void PowerBurnSpecialPreview(int v)
        {
            TempPowerTokenGaia += v;
            return;
        }
        public override void PowerBurnSpecialActual(int v)
        {
            PowerTokenGaia = PowerTokenGaia;
            TempPowerTokenGaia = 0;
            return;
        }

        protected override void CallAC1Income()
        {
            Knowledge += 3;
        }
        public override Terrain OGTerrain { get => Terrain.White; }

        public void SpecialGetTechTile()
        {
            TechTilesGet++;
            TempPowerTokenGaia = -4;
            Action action = () =>
            {
                PowerTokenGaia = PowerTokenGaia;
                TempPowerTokenGaia = 0;
            };
            ActionQueue.Enqueue(action);
        }
    }
}
