using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class Terraner : Faction
    {
        public Terraner(GaiaGame gg) : base(FactionName.Terraner, gg)
        {
            this.ChineseName = "人类";
            this.ColorCode = colorList[0];
            m_powerToken1 += 2;
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

        internal bool ConvertGaiaPowerToAnother(int rFNum, string rFKind, int rTNum, string rTKind, out string log)
        {
            log = string.Empty;
            var str = rFKind + rTKind;
            switch (str)
            {
                case "pwq":
                    if (rFNum != rTNum * 4)
                    {
                        log = "兑换比例为4：1";
                        return false;
                    }
                    if (PowerTokenGaia < rFNum)
                    {
                        log = "魔力值不够";
                        return false;
                    }
                    TempPowerTokenGaia -= rFNum;
                    TempPowerToken2 += rFNum;
                    TempQICs += rTNum;
                    Action action = () =>
                    {
                        PowerTokenGaia = PowerTokenGaia;
                        PowerToken2 = PowerToken2;
                        QICs = QICs;
                        TempPowerTokenGaia = 0;
                        TempPowerToken2 = 0;
                        TempQICs = 0;
                    };
                    ActionQueue.Enqueue(action);
                    break;
                case "pwo":
                    if (rFNum != rTNum * 3)
                    {
                        log = "兑换比例为3：1";
                        return false;
                    }
                    if (PowerTokenGaia < rFNum)
                    {
                        log = "魔力值不够";
                        return false;
                    }
                    TempPowerTokenGaia -= rFNum;
                    TempPowerToken2 += rFNum;
                    TempOre += rTNum;
                    action = () =>
                    {
                        PowerTokenGaia = PowerTokenGaia;
                        PowerToken2 = PowerToken2;
                        Ore = Ore;
                        TempPowerTokenGaia = 0;
                        TempPowerToken2 = 0;
                        TempOre = 0;
                    };
                    ActionQueue.Enqueue(action);
                    break;
                case "pwk":
                    if (rFNum != rTNum * 4)
                    {
                        log = "兑换比例为4：1";
                        return false;
                    }
                    if (PowerTokenGaia < rFNum)
                    {
                        log = "魔力值不够";
                        return false;
                    }
                    TempPowerTokenGaia -= rFNum;
                    TempPowerToken2 += rFNum;
                    TempKnowledge += rTNum;
                    action = () =>
                    {
                        PowerTokenGaia = PowerTokenGaia;
                        PowerToken2 = PowerToken2;
                        Knowledge = Knowledge;
                        TempPowerTokenGaia = 0;
                        TempPowerToken2 = 0;
                        TempKnowledge = 0;
                    };
                    ActionQueue.Enqueue(action);
                    break;
                case "pwc":
                    if (rFNum != rTNum * 1)
                    {
                        log = "兑换比例为1：1";
                        return false;
                    }
                    if (PowerTokenGaia < rFNum)
                    {
                        log = "魔力值不够";
                        return false;
                    }
                    TempPowerTokenGaia -= rFNum;
                    TempPowerToken2 += rFNum;
                    TempCredit += rTNum;
                    action = () =>
                    {
                        PowerTokenGaia = PowerTokenGaia;
                        PowerToken2 = PowerToken2;
                        Credit = Credit;
                        TempPowerTokenGaia = 0;
                        TempPowerToken2 = 0;
                        TempCredit = 0;
                    };
                    ActionQueue.Enqueue(action);
                    break;
                default:
                    log = "不支持这种转换";
                    return false;
            }
            return true;
        }
    }
}
