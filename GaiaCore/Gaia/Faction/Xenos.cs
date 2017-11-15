using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class Xenos : Faction
    {
        public Xenos(GaiaGame gg) : base(FactionName.Xenos, gg)
        {
            this.ChineseName = "异空族";
            this.ColorCode = colorList[3];
            this.ColorMap = colorMapList[3];

            if (gg != null)
            {
                IncreaseTech("ai");
            }
        }


        protected override void CallSHIncome()
        {
            PowerIncrease(4);
            m_QICs += 1;
        }
        public override Terrain OGTerrain { get => Terrain.Yellow; }
        protected override void CallSpecialSHBuild()
        {
            m_allianceMagicLevel -= 1;
        }

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
