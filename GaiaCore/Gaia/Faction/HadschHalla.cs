﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class HadschHalla : Faction
    {
        public HadschHalla(GaiaGame gg) : base(FactionName.HadschHalla, gg)
        {
            this.ChineseName = "圣禽族";
            base.SetColor(1);

            if (gg != null)
            {
                IncreaseTech("eco");
            }
        }
        public override Terrain OGTerrain { get => Terrain.Red; }
        public override void CalIncome()
        {
            base.CalIncome();
        }

        protected override int CalCreditIncome()
        {
            return base.CalCreditIncome() + 3;
        }

        public override bool ConvertOneResourceToAnother(int rFNum, string rFKind, int rTNum, string rTKind, out string log, int? rTNum2 = null, string rTKind2 = null, int? rFNum2 = null, string rFKind2 = null)
        {

            log = string.Empty;
            var str = rFKind + rTKind;
            if (StrongHold == null)
            {
                switch (str)
                {
                    case "cq":
                        if (rFNum != rTNum * 4)
                        {
                            log = "兑换比例为4：1";
                            return false;
                        }
                        if (Credit < rFNum)
                        {
                            log = "钱不够";
                        }
                        TempCredit -= rFNum;
                        TempQICs += rTNum;
                        Action action = () =>
                        {
                            Credit = Credit;
                            QICs = QICs;
                            TempCredit = 0;
                            TempQICs = 0;
                        };
                        ActionQueue.Enqueue(action);
                        break;
                    case "co":
                        if (rFNum != rTNum * 3)
                        {
                            log = "兑换比例为3：1";
                            return false;
                        }
                        if (Credit < rFNum)
                        {
                            log = "钱不够";
                        }
                        TempCredit -= rFNum;
                        TempOre += rTNum;
                        action = () =>
                        {
                            Credit = Credit;
                            Ore = Ore;
                            TempCredit = 0;
                            TempOre = 0;
                        };
                        ActionQueue.Enqueue(action);
                        break;
                    case "ck":
                        if (rFNum != rTNum * 4)
                        {
                            log = "兑换比例为4：1";
                            return false;
                        }
                        if (Credit < rFNum)
                        {
                            log = "钱不够";
                        }
                        TempCredit -= rFNum;
                        TempKnowledge += rTNum;
                        action = () =>
                        {
                            Credit = Credit;
                            Knowledge = Knowledge;
                            TempCredit = 0;
                            TempKnowledge = 0;
                        };
                        ActionQueue.Enqueue(action);
                        break;
                    case "cpwt":
                        if (rFNum != rTNum * 3)
                        {
                            log = "兑换比例为3：1";
                            return false;
                        }
                        if (Credit < rFNum)
                        {
                            log = "钱不够";
                        }
                        TempCredit -= rFNum;
                        TempPowerToken1 += rTNum;
                        action = () =>
                        {
                            Credit = Credit;
                            PowerToken1 = PowerToken1;
                            TempCredit = 0;
                            TempPowerToken1 = 0;
                        };
                        ActionQueue.Enqueue(action);
                        break;
                    default:
                        return base.ConvertOneResourceToAnother(rFNum, rFKind, rTNum, rTKind, out log, rTNum2, rTKind2);
                }
                return true;
            }
            else
            {
                return base.ConvertOneResourceToAnother(rFNum, rFKind, rTNum, rTKind, out log, rTNum2, rTKind2);
            }
        }
    }
}
