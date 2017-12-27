using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GaiaCore.Gaia
{
    public class MadAndroid : Faction
    {
        public MadAndroid(GaiaGame gg) : base(FactionName.MadAndroid, gg)
        {
            this.ChineseName = "疯狂机器";
            base.SetColor(5);
            IsMadAndroidAbilityUsed = false;
            Knowledge -= 2;
            StrongHold = new MadAndroidSpecialBuildings.StrongHold();
            Academy1 = new MadAndroidSpecialBuildings.Academy();
            Academy2 = new MadAndroidSpecialBuildings.Academy();
        }
        public override Terrain OGTerrain { get => Terrain.Gray; }
        public bool IsMadAndroidAbilityUsed { set; get; }

        public override void ResetNewRound()
        {
            IsMadAndroidAbilityUsed = false;
            base.ResetNewRound();
        }

        protected override int CalKnowledgeIncome()
        {
            var ret = 0;
            ret += m_TradeCenterCount - TradeCenters.Count;
            if (Academy1 == null)
            {
                ret += CallAC1Income();
            }

            ret += GameTileList.Sum(x => x.GetKnowledgeIncome());

            switch (ScienceLevel)
            {
                case 1:
                    ret += 1;
                    break;
                case 2:
                    ret += 2;
                    break;
                case 3:
                    ret += 3;
                    break;
                case 4:
                    ret += 4;
                    break;
            }
            return ret;
        }

        protected override int CalCreditIncome()
        {
            int ret = 0;
            if (ResearchLabs.Count == 2)
            {
                ret += 3;
            }
            else if (ResearchLabs.Count == 1)
            {
                ret += 7;
            }
            else if (ResearchLabs.Count == 0)
            {
                ret += 12;
            }
            ret += GameTileList.Sum(x => x.GetCreditIncome());
            switch (EconomicLevel)
            {
                case 1:
                    ret += 2;
                    break;
                case 2:
                    ret += 2;
                    break;
                case 3:
                    ret += 3;
                    break;
                case 4:
                    ret += 4;
                    break;
                case 5:
                    ret += 6;
                    break;
                default:
                    break;
            }
            return ret;
        }

        protected override int CallSHPowerTokenIncome()
        {
            return base.CallSHPowerTokenIncome() + 1;
        }

        public override bool IsIncreaseTechLevelByIndexValidate(int index, out string log, bool isIncreaseAllianceTileCost = false)
        {
            if (IsSingleAdvTechTrack && IsMadAndroidAbilityUsed == false)
            {
                var level = (int)list[index].GetValue(this);
                if ((int)list.Min(x => x.GetValue(this)) == level)
                {
                    TechTracAdv++;
                    IsMadAndroidAbilityUsed = true;
                }
                else
                {
                    if (Knowledge < 4)
                    {
                        log = "科技不足四点且无法触发能力";
                        return false;
                    }
                }
            }
            return base.IsIncreaseTechLevelByIndexValidate(index, out log, isIncreaseAllianceTileCost);
        }
    }
}
