using GaiaCore.Gaia.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GaiaCore.Gaia
{
    public abstract partial class Faction
    {
        public Faction(FactionName name, GaiaGame gg)
        {
            FactionName = name;
            if (gg.IsTestGame)
            {
                m_credit = 50;
                m_knowledge = 20;
                m_ore = 30;
                m_QICs = 4;
                m_powerToken3 = 30;
                m_ShipLevel = 4;
            }
            else
            {
                m_credit = 15;
                m_knowledge = 3;
                m_ore = 4;
                m_QICs = 1;
                m_powerToken3 = 0;
                m_ShipLevel = 0;
            }
            
            m_powerToken1 = 2;
            m_powerToken2 = 4;
            m_powerTokenGaia = 0;
            m_TransformLevel = 0;
            m_AILevel = 0;
            m_EconomicLevel = 0;
            m_GaiaLevel = 0;
            m_ScienceLevel = 0;

            Mines = new List<Mine>();
            for (int i = 0; i < GameConstNumber.MineCount; i++)
            {
                Mines.Add(new Mine());
            }
            TradeCenters = new List<TradeCenter>();
            for (int i = 0; i < GameConstNumber.TradeCenterCount; i++)
            {
                TradeCenters.Add(new TradeCenter());
            }
            ResearchLabs = new List<ResearchLab>();
            for (int i = 0; i < GameConstNumber.ResearchLabCount; i++)
            {
                ResearchLabs.Add(new ResearchLab());
            }
            Academy1 = new Academy();
            Academy2 = new Academy();
            StrongHold = new StrongHold();
            Gaias = new List<GaiaBuilding>();
            GameTileList = new List<GameTiles>();
            LeechPowerQueue = new List<Tuple<int, FactionName>>();
            Score = 10;
            GaiaGame = gg;
            ActionQueue = new Queue<Action>();
            ActionList = new Dictionary<string, Func<Faction, bool>>();
            PredicateActionList = new Dictionary<string, Func<Faction, bool>>();
            GaiaGame.MapActionMrg.AddMapActionList(ActionList, PredicateActionList);
            GaiaPlanetNumber = 0;
            m_allianceMagicLevel = 7;
        }

        public virtual bool FinishIntialMines()
        {
            if (GameConstNumber.MineCount - Mines.Count == 2)
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
        protected int m_credit;
        protected int m_ore;
        protected int m_knowledge;
        protected int m_QICs;
        protected int m_powerToken1;
        protected int m_powerToken2;
        protected int m_powerToken3;
        protected int m_powerTokenGaia;
        protected int m_TransformLevel;
        protected int m_ShipLevel;
        protected int m_AILevel;
        protected int m_GaiaLevel;
        protected int m_EconomicLevel;
        protected int m_ScienceLevel;
        protected int m_allianceMagicLevel;

        protected const int m_MineOreCost = 1;
        protected const int m_MineCreditCost = 2;

        protected const int m_TradeCenterOreCost = 2;
        protected const int m_TradeCenterCreditCostCluster = 3;
        protected const int m_TradeCenterCreditCostAlone = 6;
        protected const int m_TradeCenterCount = 4;
        protected const int m_ReaserchLabOreCost = 3;
        protected const int m_ReaserchLabCreditCost = 5;
        protected const int m_ReaserchLabCount = 3;
        protected const int m_AcademyOreCost = 6;
        protected const int m_AcademyCreditCost = 6;
        protected const int m_AcademyCount = 2;
        protected const int m_StrongHoldOreCost = 4;
        protected const int m_StrongHoldCreditCost = 6;
        protected const int m_StrongHoldCount = 1;


        #region 临时变量 判断动作完成与否 需要清零
        private int m_TerraFormNumber = 0;
        private int m_TempShip = 0;
        private int m_TechTilesGet = 0;
        private int m_TechTrachAdv = 0;
        private int m_AllianceTileGet = 0;
        private int m_AllianceTileReGet = 0;
        public int TempPowerToken1 = 0;
        public int TempPowerToken2 = 0;
        public int TempPowerToken3 = 0;
        public int TempPowerTokenGaia = 0;

        public int TempCredit = 0;
        public int TempOre = 0;
        public int TempKnowledge = 0;
        public int TempQICs = 0;
        public string LimitTechAdvance { get; set; }
        #endregion



        public virtual void CalIncome()
        {
            CalOreIncome();
            CalCreditIncome();
            CalKnowledgeIncome();
            CalPowerIncome();
            CalQICIncome();
            CallTechIncome();
            CallSpecialFreeIncome();
        }

        protected virtual void CallSpecialFreeIncome()
        {
            return;
        }

        private void CallTechIncome()
        {
            switch (EconomicLevel)
            {
                case 1:
                    Credit += 2;
                    PowerIncrease(1);
                    break;
                case 2:
                    Credit += 2;
                    PowerIncrease(2);
                    Ore += 1;
                    break;
                case 3:
                    Credit += 3;
                    PowerIncrease(3);
                    Ore += 1;
                    break;
                case 4:
                    Credit += 4;
                    PowerIncrease(4);
                    Ore += 2;
                    break;
                case 5:
                    Credit += 6;
                    PowerIncrease(6);
                    Ore += 3;
                    break;
                default:
                    break;
            }
            switch (ScienceLevel)
            {
                case 1:
                    Knowledge += 1;
                    break;
                case 2:
                    Knowledge += 2;
                    break;
                case 3:
                    Knowledge += 3;
                    break;
                case 4:
                    Knowledge += 4;
                    break;
            }
        }

        protected virtual void CalOreIncome()
        {
            if (Mines.Count >= 6 && Mines.Count <= 8)
            {
                Ore += 9 - Mines.Count;
            }
            else if (Mines.Count == 5)
            {
                Ore += 3;
            }
            else if (Mines.Count < 5)
            {
                Ore += 8 - Mines.Count;
            }

            Ore += GameTileList.Sum(x => x.GetOreIncome());
        }
        protected virtual void CalCreditIncome()
        {
            if (TradeCenters.Count == 3)
            {
                Credit += 3;
            }
            else if (TradeCenters.Count == 2)
            {
                Credit += 7;
            }
            else if (TradeCenters.Count == 1)
            {
                Credit += 11;
            }
            else if (TradeCenters.Count == 0)
            {
                Credit += 16;
            }
            Credit += GameTileList.Sum(x => x.GetCreditIncome());
        }


        protected virtual void CalKnowledgeIncome()
        {
            Knowledge += 4 - ResearchLabs.Count;
            if (Academy1 == null)
            {
                CallAC1Income();
            }

            Knowledge += GameTileList.Sum(x => x.GetKnowledgeIncome());
        }

        protected virtual void CallAC1Income()
        {
            Knowledge += 2;
        }

        protected virtual void CalQICIncome()
        {
            if (Academy2 == null)
            {
                QICs += 1;
            }

            QICs += GameTileList.Sum(x => x.GetQICIncome());
        }

        protected virtual void CalPowerIncome()
        {
            if (StrongHold == null)
            {
                CallSHIncome();
            }

            PowerIncrease(GameTileList.Sum(x => x.GetPowerIncome()));
            PowerToken1 += GameTileList.Sum(x => x.GetPowerTokenIncome());
        }

        protected virtual void CallSHIncome()
        {
            PowerIncrease(4);
            m_powerToken1++;
        }

        public virtual int PowerIncrease(int i)
        {
            var ret = Math.Min(m_powerToken1 * 2 + m_powerToken2, i);
            if (m_powerToken1 > i)
            {
                m_powerToken1 -= i;
                m_powerToken2 += i;
            }
            else if (m_powerToken1 * 2 + m_powerToken2 > i)
            {
                m_powerToken2 = m_powerToken2 - i + m_powerToken1 * 2;
                m_powerToken3 += i - m_powerToken1;
                m_powerToken1 = 0;
            }
            else
            {
                m_powerToken3 += m_powerToken1 + m_powerToken2;
                m_powerToken2 = 0;
                m_powerToken1 = 0;
            }
            return ret;
        }
        public List<GaiaBuilding> Gaias { set; get; }
        public List<Mine> Mines { set; get; }
        public List<TradeCenter> TradeCenters { set; get; }
        public List<ResearchLab> ResearchLabs { set; get; }
        public Academy Academy1 { set; get; }
        public Academy Academy2 { set; get; }
        public StrongHold StrongHold { set; get; }
        public int Credit { get => m_credit+TempCredit; set => m_credit = value; }
        public int Ore { get => m_ore+TempOre; set => m_ore = value; }
        public int Knowledge { get => m_knowledge+TempKnowledge; set => m_knowledge = value; }
        public int QICs { get => m_QICs+TempQICs; set => m_QICs = value; }
        public int PowerToken1 { get => m_powerToken1 + TempPowerToken1; set => m_powerToken1 = value; }
        public int PowerToken2 { get => m_powerToken2 + TempPowerToken2; set => m_powerToken2 = value; }
        public int PowerToken3 { get => m_powerToken3 + TempPowerToken3; set => m_powerToken3 = value; }
        public int TransformLevel { get => m_TransformLevel; }
        public int ShipLevel { get => m_ShipLevel; }
        public int AILevel { get => m_AILevel; }
        public int GaiaLevel { get => m_GaiaLevel; }
        public int EconomicLevel { get => m_EconomicLevel; }
        public int ScienceLevel { get => m_ScienceLevel; }
        public abstract Terrain OGTerrain { get; }
        public List<Tuple<int, FactionName>> LeechPowerQueue { get; }
        public int Score { get; set; }
        public GaiaGame GaiaGame { get; }
        public Queue<Action> ActionQueue { get; set; }

        public Dictionary<string, Func<Faction, bool>> ActionList { get; set; }
        public Dictionary<string, Func<Faction, bool>> PredicateActionList { get; set; }
        public int GaiaPlanetNumber { get; set; }
        public int GetShipDistance
        {
            get
            {
                if (m_ShipLevel == 0 | m_ShipLevel == 1)
                {
                    return 1 + TempShip;
                }
                else if (m_ShipLevel == 2 | m_ShipLevel == 3)
                {
                    return 2 + TempShip;
                }
                else if (m_ShipLevel == 4)
                {
                    return 3 + TempShip;
                }
                else if (m_ShipLevel == 5)
                {
                    return 4 + TempShip;
                }
                throw new Exception("m_ShipLevel数值出错" + m_ShipLevel);
            }
        }

        public int GetTransformCost
        {
            get
            {
                if (m_TransformLevel == 0 | m_TransformLevel == 1)
                {
                    return 3;
                }
                else if (m_TransformLevel == 2)
                {
                    return 2;
                }
                else if (m_TransformLevel == 3 | m_TransformLevel == 4 | m_TransformLevel == 5)
                {
                    return 1;
                }
                throw new Exception("m_Transform数值出错" + m_TransformLevel);
            }
        }

        public int TechTilesGet { get => m_TechTilesGet; set => m_TechTilesGet = value; }
        public int TechTracAdv { get => m_TechTrachAdv; set => m_TechTrachAdv = value; }
        public int TerraFormNumber { get => m_TerraFormNumber; set => m_TerraFormNumber = value; }
        public int TempShip { get => m_TempShip; set => m_TempShip = value; }
        public int PowerTokenGaia { get => m_powerTokenGaia + TempPowerTokenGaia; set => m_powerTokenGaia = value; }
        public int AllianceTileReGet { get => m_AllianceTileReGet; set => m_AllianceTileReGet = value; }

        private static List<FieldInfo> list = new List<FieldInfo>()
        {
            typeof(Faction).GetField("m_TransformLevel",BindingFlags.NonPublic|BindingFlags.Instance),
            typeof(Faction).GetField("m_ShipLevel",BindingFlags.NonPublic|BindingFlags.Instance),
            typeof(Faction).GetField("m_AILevel",BindingFlags.NonPublic|BindingFlags.Instance),
            typeof(Faction).GetField("m_GaiaLevel",BindingFlags.NonPublic|BindingFlags.Instance),
            typeof(Faction).GetField("m_EconomicLevel",BindingFlags.NonPublic|BindingFlags.Instance),
            typeof(Faction).GetField("m_ScienceLevel",BindingFlags.NonPublic|BindingFlags.Instance),
        };


        public bool IsIncreaseTechLevelByIndexValidate(int index)
        {
            if (index < 0 | index > 5)
            {
                throw new Exception("超出科技条边界");
            }
            var level = (int)list[index].GetValue(this);
            if (level >= 0 && level < 4)
            {
                //level++;
            }
            else if (level == 4)
            {
                if (GameTileList.Exists(x => x is AllianceTile && x.IsUsed == false))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }


        //private int m_TransformLevel;

        //       private int m_ShipLevel;
        //       private int m_AILevel;
        //       private int m_GaiaLevel;
        //       private int m_EconomicLevel;
        //       private int m_ScienceLevel;
    }


}
