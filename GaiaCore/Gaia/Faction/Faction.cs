using GaiaCore.Gaia.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GaiaCore.Gaia
{
    public abstract class Faction
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
            return;
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
        }

        internal bool BuildMine(Map map, int row, int col, out string log)
        {
            log = string.Empty;
            if (!(Mines.Count > 1 && m_credit > m_MineCreditCost && m_ore > m_MineOreCost))
            {
                log = "资源不够";
                return false;
            }
            if (!(map.HexArray[row, col].OGTerrain == OGTerrain))
            {
                log = "地形不符";
                return false;
            }
            if (!(map.HexArray[row, col].Building == null && map.HexArray[row, col].FactionBelongTo == null))
            {
                log = "该地点已经有人占领了";
                return false;
            }
            if (!map.CalIsBuildValidate(row, col, FactionName, m_ShipLevel))
            {
                log = "航海距离不够";
                return false;
            }
            //扣资源建建筑
            m_ore -= m_MineOreCost;
            m_credit -= m_MineCreditCost;
            map.HexArray[row, col].Building = Mines.First();
            map.HexArray[row, col].FactionBelongTo = FactionName;
            Mines.RemoveAt(0);
            return true;
        }

        internal bool BuildIntialMine(Map map, int row, int col, out string log)
        {
            log = string.Empty;
            if (!(map.HexArray[row, col].OGTerrain == OGTerrain))
            {
                log = "地形不符";
                return false;
            }
            if (!(map.HexArray[row, col].Building == null && map.HexArray[row, col].FactionBelongTo == null))
            {
                log = "该地点已经有人占领了";
                return false;
            }

            map.HexArray[row, col].Building = Mines.First();
            map.HexArray[row, col].FactionBelongTo = FactionName;
            Mines.RemoveAt(0);
            return true;
        }

        protected virtual void CalKnowledgeIncome()
        {
            m_knowledge += 4 - ReaserchLabs.Count;
            if (Academies.Count <= 1)
            {
                m_knowledge += 2;
            }
        }

        protected virtual void CalQICIncome()
        {
            if (Academies.Count == 0)
            {
                m_QICs += 1;
            }
        }

        protected virtual void CalPowerIncome()
        {
            if (StrongHold == null)
            {
                PowerIncrease(4);
            }
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

    public class StrongHold : Building
    {
        public override int MagicLevel
        {
            get
            {
                return 3;
            }
        }
    }


}
