using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GaiaProject2.Gaia
{
    public class Faction
    {
        private int m_credit=0;
        private int m_ore = 0;
        private int m_knowledge = 0;
        private int m_QICs = 0;
        private int m_powerToken1 = 0;
        private int m_powerToken2 = 0;
        private int m_powerToken3 = 0;

        public List<Mine> Mines { set; get; }
        public List<TradeCenter> TradeCenters { set; get; }
        public List<ReaserchLab> ReaserchLabs { set; get; }
        public List<Academy> Academies{ set; get; }
        public Government Government { set; get; }
    }
    public abstract class Building
    {
        public abstract int MagicLevel { get; }
    }

    public class Mine:Building
    {
        public override int MagicLevel { get
            {
                return 1;
            }
        }
    }
    public class TradeCenter : Building
    {
        public override int MagicLevel
        {
            get
            {
                return 2;
            }
        }
    }

    public class ReaserchLab : Building
    {
        public override int MagicLevel
        {
            get
            {
                return 2;
            }
        }
    }

    public class Academy : Building
    {
        public override int MagicLevel
        {
            get
            {
                return 3;
            }
        }
    }

    public class Government : Building
    {
        public override int MagicLevel
        {
            get
            {
                return 3;
            }
        }
    }

    enum FactionName
    {
        Lantida,
        Terraner,
        Ambas,
        Taklons,
        Firaks,
        MadAndroid,
        BalTak,
        Geoden,
        Hive,
        HadschHalla,
        Itar,
        Nevla,
        Gleen,
        Xenos
    }
}
