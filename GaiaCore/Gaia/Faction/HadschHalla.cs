using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class HadschHalla : Faction
    {
        public HadschHalla(GaiaGame gg) : base(FactionName.HadschHalla, gg)
        {
            this.ChineseName = "圣禽族";
            this.ColorCode = colorList[1];
            IncreaseTech("eco");
        }
        public override Terrain OGTerrain { get => Terrain.Red; }
        public override void CalIncome()
        {
            Credit += 3;
            base.CalIncome();
        }

        internal override bool ConvertOneResourceToAnother(int rFNum, string rFKind, int rTNum, string rTKind, out string log)
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
                        if (PowerToken3 < rFNum)
                        {
                            log = "魔力值不够";
                        }
                        TempCredit -= rFNum;
                        TempQICs += rTNum;
                        Action action = () =>
                        {
                            Credit = Credit;
                            QICs = QICs;
                            TempCredit = 0;
                            TempQICs =0;
                        };
                        ActionQueue.Enqueue(action);
                        break;
                    case "co":
                        if (rFNum != rTNum * 3)
                        {
                            log = "兑换比例为3：1";
                            return false;
                        }
                        if (PowerToken3 < rFNum)
                        {
                            log = "魔力值不够";
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
                        if (PowerToken3 < rFNum)
                        {
                            log = "魔力值不够";
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
                        if (PowerToken3 < rFNum)
                        {
                            log = "魔力值不够";
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
                        return base.ConvertOneResourceToAnother(rFNum, rFKind, rTNum, rTKind, out log);
                }
                return true;
            }
            else
            {
                return base.ConvertOneResourceToAnother(rFNum, rFKind, rTNum, rTKind, out log);
            }
        }
    }
}
