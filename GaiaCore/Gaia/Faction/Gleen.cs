using GaiaCore.Gaia.Tiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class Gleen : Faction
    {
        public Gleen(GaiaGame gg) : base(FactionName.Gleen, gg)
        {
            this.ChineseName = "格伦星人";
            this.ColorCode = colorList[3];
            this.ColorMap = colorMapList[3];
            m_QICs -= 1;
            IsOreReplaceQICSIncome = true;
            if (gg != null)
            {
                IncreaseTech("ship");
            }
            //Set Green Planet 会多加两分
            Score -= 2;
        }
        public override Terrain OGTerrain { get => Terrain.Yellow; }

        public bool IsOreReplaceQICSIncome { set; get; }

        protected override bool PreBuildGaiaPlanetMine(out string log)
        {
            log = string.Empty;
            TempOre -= 1;
            if (Ore < 0)
            {
                log = "至少需要一块O代替Q在盖亚星球上建造矿场";
                return false;
            }
            return false;
        }

        protected override void BuildGaiaPlanetMine()
        {
            Ore = Ore;
            TempOre = 0;
        }

        public override int GaiaPlanetNumber
        {
            get => base.GaiaPlanetNumber;
            set
            {
                base.GaiaPlanetNumber = value;
                Score += 2;
            }
        }

        public override int QICs
        {
            get
            {
                if (IsOreReplaceQICSIncome)
                {
                    return m_QICs;
                }
                else
                {
                    return base.QICs;
                }

            }
            set
            {
                if (IsOreReplaceQICSIncome)
                {
                    m_ore += TempOre;
                    TempOre = 0;
                    m_ore += value - QICs;
                }
                else
                {
                    base.QICs = value;
                }
            }
        }

        public override int TempQICs
        {
            get
            {
                if (IsOreReplaceQICSIncome)
                {
                    return TempOre;
                }
                else
                {
                    return base.TempQICs;
                }
            }
            set
            {
                if (IsOreReplaceQICSIncome)
                {
                    TempOre = value;
                }
                else
                {
                    base.TempQICs = value;
                }

            }
        }

        protected override void CallSpecialSHBuild()
        {
            AddGameTiles(new ALT7());
            base.CallSpecialSHBuild();
        }

        protected override int CallSHPowerTokenIncome()
        {
            return 0;
        }

        protected override int CalOreIncome()
        {
            var ret = 0;
            if (StrongHold == null)
            {
                ret++;
            }
            ret += base.CalOreIncome();
            return ret;
        }

        internal override void ResetUnfinishAction()
        {
            if (Academy2 == null)
            {
                IsOreReplaceQICSIncome = false;
            }
            else
            {
                IsOreReplaceQICSIncome = true;
            }
            base.ResetUnfinishAction();
        }
    }

    public class ALT7 : AllianceTile
    {
        public override string desc
        {
            get
            {
                return "1K,1O,2C";
            }
        }
        public override bool OneTimeAction(Faction faction)
        {
            faction.Knowledge += 1;
            faction.Ore += 1;
            faction.Credit += 2;
            return true;
        }
    }
}
