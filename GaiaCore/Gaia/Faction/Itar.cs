using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class Itar : Faction
    {
        public Itar(GaiaGame gg) :base(FactionName.Itar, gg)
        {
            this.ChineseName = "伊塔星人";
            this.ColorCode = colorList[6];
            this.ColorMap = colorMapList[6];

            m_ore += 1;
            m_powerToken1 += 2;
        }

        public override void CalIncome()
        {
            base.CalIncome();
        }
        protected override int CalPowerTokenIncome(List<int> list = null)
        {
            List<int> ret;
            if (list != null)
            {
                ret = list;
            }
            else
            {
                ret = new List<int>();
            }
            ret.Add(1);
            return base.CalPowerTokenIncome(ret);
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

        protected override int CallAC1Income()
        {
            return 3;
        }
        public override Terrain OGTerrain { get => Terrain.White; }

        public void SpecialGetTechTile()
        {
            if (StrongHold != null)
            {
                throw new Exception("无SH不能执行盖亚能力");
            }
            TechTilesGet++;
            TempPowerTokenGaia = -GameConstNumber.ItarGaiaGetTechTileCost;
            Action action = () =>
            {
                PowerTokenGaia = PowerTokenGaia;
                TempPowerTokenGaia = 0;
            };
            ActionQueue.Enqueue(action);
        }
    }
}
