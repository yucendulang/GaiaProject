using GaiaCore.Gaia.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GaiaCore.Gaia
{
    public  abstract partial class Faction
    {
        public Faction(FactionName name)
        {
            FactionName = name;
            m_credit = 15;
            m_knowledge = 3;
            m_ore = 4;
            m_QICs = 1;
            m_powerToken1 = 2;
            m_powerToken2 = 4;
            m_powerToken3 = 0;
            m_TransformLevel = 0;
            m_AILevel = 0;
            m_EconomicLevel = 0;
            m_GaiaLevel = 0;
            m_ScienceLevel = 0;
            m_ShipLevel = 1;
            Mines = new List<Mine>();
            for (int i = 0; i < m_MineCount; i++)
            {
                Mines.Add(new Mine());
            }
            TradeCenters = new List<TradeCenter>();
            for (int i = 0; i < 4; i++)
            {
                TradeCenters.Add(new TradeCenter());
            }
            ReaserchLabs = new List<ReaserchLab>();
            for (int i = 0; i < 3; i++)
            {
                ReaserchLabs.Add(new ReaserchLab());
            }
            Academies = new List<Academy>();
            for (int i = 0; i < 2; i++)
            {
                Academies.Add(new Academy());
            }
            StrongHold=new StrongHold();
            GameTileList = new List<GameTiles>();
        }

        internal bool FinishIntialMines()
        {
            if (m_MineCount - Mines.Count == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public FactionName FactionName { get; }
        public List<GameTiles> GameTileList { set; get; }
        private int m_credit;
        private int m_ore;
        private int m_knowledge;
        private int m_QICs;
        private int m_powerToken1;
        private int m_powerToken2;
        private int m_powerToken3;
        private int m_TransformLevel;
        private int m_ShipLevel;
        private int m_AILevel;
        private int m_GaiaLevel;
        private int m_EconomicLevel;
        private int m_ScienceLevel;
        private const int m_MineOreCost=1;
        private const int m_MineCreditCost = 2;
        private const int m_MineCount=8;
        private const int m_TradeCenterOreCost = 2;
        private const int m_TradeCenterCreditCostCluster = 3;
        private const int m_TradeCenterCreditCostAlone = 6;
        private const int m_TradeCenterCount = 4;
        private const int m_ReaserchLabOreCost = 3;
        private const int m_ReaserchLabCreditCost = 5;
        private const int m_ReaserchLabCount = 3;
        private const int m_AcademyOreCost = 6;
        private const int m_AcademyCreditCost = 6;
        private const int m_AcademyCount = 2;
        private const int m_StrongHoldOreCost = 4;
        private const int m_StrongHoldCreditCost = 6;
        private const int m_StrongHoldCount = 1;



        public virtual void CalIncome()
        {
            CalOreIncome();
            CalCreditIncome();
            CalKnowledgeIncome();
            CalPowerIncome();
            CalQICIncome();          
        }
        protected virtual void CalOreIncome()
        {
            if (Mines.Count >= 6 && Mines.Count <= 8)
            {
                m_ore += 9 - Mines.Count;
            }else if (Mines.Count == 5)
            {
                m_ore += 3;
            }else if (Mines.Count < 5)
            {
                m_ore += 8 - Mines.Count;
            }

            m_ore+=GameTileList.Sum(x => x.GetOreIncome());
        }
        protected virtual void CalCreditIncome()
        {
            if (TradeCenters.Count == 3)
            {
                m_credit += 3;
            }else if(TradeCenters.Count == 2)
            {
                m_credit += 7;
            }
            else if (TradeCenters.Count == 1)
            {
                m_credit += 11;
            }else if (TradeCenters.Count ==0)
            {
                m_credit += 16;
            }
            m_credit += GameTileList.Sum(x => x.GetCreditIncome());
        }

      
        protected virtual void CalKnowledgeIncome()
        {
            m_knowledge += 4 - ReaserchLabs.Count;
            if (Academies.Count <= 1)
            {
                m_knowledge += 2;
            }

            m_knowledge += GameTileList.Sum(x => x.GetKnowledgeIncome());
        }

        protected virtual void CalQICIncome()
        {
            if (Academies.Count == 0)
            {
                m_QICs += 1;
            }

            m_QICs += GameTileList.Sum(x => x.GetQICIncome());
        }

        protected virtual void CalPowerIncome()
        {
            if (StrongHold == null)
            {
                PowerIncrease(4);
            }

            PowerIncrease(GameTileList.Sum(x => x.GetPowerIncome()));
        }

        protected virtual void PowerIncrease(int i)
        {
            if (m_powerToken1 > i)
            {
                m_powerToken1 -= i;
                m_powerToken2 += i;
            } else if (m_powerToken1 * 2 + m_powerToken2 > i)
            {
                m_powerToken2 = m_powerToken2 - i + m_powerToken1*2;
                m_powerToken3 += i - m_powerToken1;
                m_powerToken1 = 0;
            }else
            {
                m_powerToken3 += m_powerToken1 + m_powerToken2;
                m_powerToken2 = 0;
                m_powerToken1 = 0;
            }
        }

        public List<Mine> Mines { set; get; }
        public List<TradeCenter> TradeCenters { set; get; }
        public List<ReaserchLab> ReaserchLabs { set; get; }
        public List<Academy> Academies { set; get; }
        public StrongHold StrongHold { set; get; }
        public int Credit { get => m_credit; }
        public int Ore { get => m_ore;}
        public int Knowledge { get => m_knowledge;}
        public int QICs { get => m_QICs; }
        public int PowerToken1 { get => m_powerToken1; }
        public int PowerToken2 { get => m_powerToken2;  }
        public int PowerToken3 { get => m_powerToken3;  }
        public int TransformLevel { get => m_TransformLevel;  }
        public int ShipLevel { get => m_ShipLevel;  }
        public int AILevel { get => m_AILevel; }
        public int GaiaLevel { get => m_GaiaLevel;}
        public int EconomicLevel { get => m_EconomicLevel; }
        public int ScienceLevel { get => m_ScienceLevel; }
        public abstract Terrain OGTerrain { get; }
    }


}
