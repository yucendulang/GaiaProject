using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class Xenos : Faction
    {
        public Xenos(GaiaGame gg) : base(FactionName.Xenos, gg)
        {
            IncreaseTech("ai");
            m_allianceMagicLevel -= 1;
        }


        protected override void CallSHIncome()
        {
            PowerIncrease(4);
            m_QICs += 1;
        }
        public override Terrain OGTerrain { get => Terrain.Yellow; }

        public override bool FinishIntialMines()
        {
            if (GameConstNumber.MineCount - Mines.Count == 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
