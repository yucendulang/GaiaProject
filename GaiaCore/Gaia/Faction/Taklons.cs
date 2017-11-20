using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class Taklons : Faction
    {
        public Taklons(GaiaGame gg) :base(FactionName.Taklons,gg)
        {
            this.ChineseName = "利爪族";
            this.ColorCode = colorList[4];
            this.ColorMap = colorMapList[4];
            BigStone = 1;
        }
        /// <summary>
        /// 所谓的智慧之石 1表示在1区 2表示在2区 3表示3区
        /// </summary>
        public int BigStone { set; get; }
        public override Terrain OGTerrain { get => Terrain.Brown; }

        public override int PowerIncrease(int i)
        {
            if (m_powerToken1 > i)
            {
                BigStone = 2;
            }
            else if (m_powerToken1 * 2 + m_powerToken2 > i)
            {
                BigStone = 3;
            }
            else
            {
                BigStone = 3;
            }
            return base.PowerIncrease(i);
        }

        internal override void PowerUse(int v)
        {
            //if(BigStone)
            base.PowerUse(v);
        }
    }
}
