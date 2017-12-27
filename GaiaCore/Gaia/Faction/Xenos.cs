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
            base.SetColor(3);

            if (gg != null)
            {
                IncreaseTech("ai");
            }
        }


        protected override int CallSHPowerTokenIncome()
        {
            return 0;
        }

        protected override int CalQICIncome()
        {
            var ret = 0;
            if (StrongHold == null)
            {
                ret += 1;
            }
            ret += base.CalQICIncome();
            return ret;
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
