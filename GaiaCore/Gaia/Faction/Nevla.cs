using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GaiaCore.Gaia
{
    public class Nevla : Faction
    {
        public Nevla(GaiaGame gg) : base(FactionName.Nevla, gg)
        {
            this.ChineseName = "超星人";
            this.ColorCode = colorList[6];
            this.ColorMap = colorMapList[6];
            Knowledge -= 1;
            IsStrongBuild = false;
            if (gg != null)
            {
                IncreaseTech("sci");
            }
        }
        public bool IsStrongBuild { set; get; }
        public override Terrain OGTerrain { get => Terrain.White; }

        protected override int CalKnowledgeIncome()
        {
            return base.CalKnowledgeIncome() - m_ReaserchLabCount + ResearchLabs.Count;
        }

        internal override void ResetUnfinishAction()
        {
            if (StrongHold == null)
            {
                IsStrongBuild = true;
            }
            base.ResetUnfinishAction();
        }

        protected override int CalPowerIncome(List<int> list = null)
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
            for (int i = 0; i < m_ReaserchLabCount - ResearchLabs.Count; i++)
            {
                ret.Add(2);
            }
            base.CalPowerIncome(ret);
            return ret.Sum();
        }

        public override int PowerTokenRepresent
        {
            get
            {
                if (IsStrongBuild)
                {
                    return base.PowerTokenRepresent * 2;
                }
                else
                {
                    return base.PowerTokenRepresent;
                }
            }
        }

        internal override void PowerUse(int v)
        {
            if (IsStrongBuild)
            {
                PowerToken3 -= (v + 1) / 2;
                PowerToken1 += (v + 1) / 2;
            }
            else
            {
                PowerToken3 -= v;
                PowerToken1 += v;
            }
        }

        internal override bool ConvertOneResourceToAnother(int rFNum, string rFKind, int rTNum, string rTKind, out string log, int? rTNum2 = null, string rTKind2 = null)
        {
            log = string.Empty;
            var str = rFKind + rTKind;
            switch (str)
            {
                case "pwspk":
                    if (rFNum != rTNum * 1)
                    {
                        log = "超星人特殊自由行动兑换比例为1：1";
                        return false;
                    }
                    if (PowerToken3 < rFNum)
                    {
                        log = "魔力值不够";
                        return false;
                    }
                    TempPowerToken3 -= rFNum;
                    TempPowerTokenGaia += rFNum;
                    TempKnowledge += rTNum;
                    Action action = () =>
                    {
                        PowerToken3 = PowerToken3;
                        PowerTokenGaia = PowerTokenGaia;
                        Knowledge = Knowledge;
                        TempPowerToken3 = 0;
                        TempPowerTokenGaia = 0;
                        TempKnowledge = 0;
                    };
                    ActionQueue.Enqueue(action);
                    return true;
                default:
                    break;
            }
            if (IsStrongBuild)
            {
                str = rFKind + rTKind + rTKind2;
                switch (str)
                {
                    case "pwq":
                        if (rFNum != rTNum * 2)
                        {
                            log = "兑换比例为2：1";
                            return false;
                        }
                        if (PowerToken3 < rFNum)
                        {
                            log = "魔力值不够";
                            return false;
                        }
                        TempPowerToken3 -= rFNum;
                        TempPowerToken1 += rFNum;
                        TempQICs += rTNum;
                        Action action = () =>
                        {
                            PowerToken3 = PowerToken3;
                            PowerToken1 = PowerToken1;
                            QICs = QICs;
                            TempPowerToken3 = 0;
                            TempPowerToken1 = 0;
                            TempQICs = 0;
                        };
                        ActionQueue.Enqueue(action);
                        return true;
                    case "pwoc":
                        if (!(rFNum * 2 == rTNum * 3 + rTNum2))
                        {
                            log = "pw兑换o与c的兑换比例为2:1:1";
                            return false;
                        }
                        if (PowerToken3 < rFNum)
                        {
                            log = "魔力值不够";
                            return false;
                        }
                        TempPowerToken3 -= rFNum;
                        TempPowerToken1 += rFNum;
                        TempOre += rTNum;
                        TempCredit += rTNum2.GetValueOrDefault();
                        action = () =>
                        {
                            PowerToken3 = PowerToken3;
                            PowerToken1 = PowerToken1;
                            Ore = Ore;
                            Credit = Credit;
                            TempPowerToken3 = 0;
                            TempPowerToken1 = 0;
                            TempOre = 0;
                            TempCredit = 0;
                        };
                        ActionQueue.Enqueue(action);
                        return true;
                    case "pwco":
                        if (!(rFNum * 2 == rTNum + rTNum2 * 3))
                        {
                            log = "pw兑换c与o的兑换比例为2:1:1";
                            return false;
                        }
                        if (PowerToken3 < rFNum)
                        {
                            log = "魔力值不够";
                            return false;
                        }
                        TempPowerToken3 -= rFNum;
                        TempPowerToken1 += rFNum;
                        TempCredit += rTNum;
                        TempOre += rTNum2.GetValueOrDefault();
                        action = () =>
                        {
                            PowerToken3 = PowerToken3;
                            PowerToken1 = PowerToken1;
                            Ore = Ore;
                            Credit = Credit;
                            TempPowerToken3 = 0;
                            TempPowerToken1 = 0;
                            TempOre = 0;
                            TempCredit = 0;
                        };
                        ActionQueue.Enqueue(action);
                        return true;
                    case "pwpwtc":
                        if (!(rFNum * 2 == rTNum * 3 + rTNum2))
                        {
                            log = "pw兑换pwt和C的兑换比例为2:1：1";
                            return false;
                        }
                        if (PowerToken3 < rFNum)
                        {
                            log = "魔力值不够";
                            return false;
                        }
                        TempPowerToken3 -= rFNum;
                        TempPowerToken1 += rFNum;
                        TempPowerToken1 += rTNum;
                        TempCredit += rTNum2.GetValueOrDefault();
                        action = () =>
                        {
                            PowerToken3 = PowerToken3;
                            PowerToken1 = PowerToken1;
                            Credit = Credit;
                            TempPowerToken3 = 0;
                            TempPowerToken1 = 0;
                            TempCredit = 0;
                        };
                        ActionQueue.Enqueue(action);
                        return true;
                    case "pwcpwt":
                        if (!(rFNum * 2 == rTNum + rTNum2 * 3))
                        {
                            log = "pw兑换c和pwt的兑换比例为2:1：1";
                            return false;
                        }
                        if (PowerToken3 < rFNum)
                        {
                            log = "魔力值不够";
                            return false;
                        }
                        TempPowerToken3 -= rFNum;
                        TempPowerToken1 += rFNum;
                        TempCredit += rTNum;
                        TempPowerToken1 += rTNum2.GetValueOrDefault();
                        action = () =>
                        {
                            PowerToken3 = PowerToken3;
                            PowerToken1 = PowerToken1;
                            Credit = Credit;
                            TempPowerToken3 = 0;
                            TempPowerToken1 = 0;
                            TempCredit = 0;
                        };
                        ActionQueue.Enqueue(action);
                        return true;
                    case "pwc":
                        if (rFNum * 2 != rTNum)
                        {
                            log = "兑换比例为1：2";
                            return false;
                        }
                        if (PowerToken3 < rFNum)
                        {
                            log = "魔力值不够";
                            return false;
                        }
                        TempPowerToken3 -= rFNum;
                        TempPowerToken1 += rFNum;
                        TempCredit += rTNum;
                        action = () =>
                        {
                            PowerToken3 = PowerToken3;
                            PowerToken1 = PowerToken1;
                            Credit = Credit;
                            TempPowerToken3 = 0;
                            TempPowerToken1 = 0;
                            TempCredit = 0;
                        };
                        ActionQueue.Enqueue(action);
                        return true;
                    case "pwk":
                        if (rFNum != rTNum * 2)
                        {
                            log = "兑换比例为2：1";
                            return false;
                        }
                        if (PowerToken3 < rFNum)
                        {
                            log = "魔力值不够";
                            return false;
                        }
                        TempPowerToken3 -= rFNum;
                        TempPowerToken1 += rFNum;
                        TempKnowledge += rTNum;
                        action = () =>
                        {
                            PowerToken3 = PowerToken3;
                            PowerToken1 = PowerToken1;
                            Knowledge = Knowledge;
                            TempPowerToken3 = 0;
                            TempPowerToken1 = 0;
                            TempKnowledge = 0;
                        };
                        ActionQueue.Enqueue(action);
                        return true;
                    case "pwo":
                        if (rFNum * 2 != rTNum * 3)
                        {
                            log = "兑换比例为3：2";
                            return false;
                        }
                        if (PowerToken3 < rFNum)
                        {
                            log = "魔力值不够";
                            return false;
                        }
                        TempPowerToken3 -= rFNum;
                        TempPowerToken1 += rFNum;
                        TempOre += rTNum;
                        action = () =>
                        {
                            PowerToken3 = PowerToken3;
                            PowerToken1 = PowerToken1;
                            Ore = Ore;
                            TempPowerToken3 = 0;
                            TempPowerToken1 = 0;
                            TempOre = 0;
                        };
                        ActionQueue.Enqueue(action);
                        return true;
                }
            }
            return base.ConvertOneResourceToAnother(rFNum, rFKind, rTNum, rTKind, out log);

        }
    }
}
