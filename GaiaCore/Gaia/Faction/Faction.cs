﻿using GaiaCore.Gaia.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GaiaCore.Gaia
{
    public  abstract partial class Faction
    {
        public Faction(FactionName name, GaiaGame gg)
        {
            FactionName = name;
            m_credit = 30;
            m_knowledge = 3;
            m_ore = 11;
            m_QICs = 1;
            m_powerToken1 = 2;
            m_powerToken2 = 4;
            m_powerToken3 = 0;
            m_powerTokenGaia = 0;
            m_TransformLevel = 0;
            m_AILevel = 0;
            m_EconomicLevel = 0;
            m_GaiaLevel = 0;
            m_ScienceLevel = 0;
            m_ShipLevel = 5;
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
            ReaserchLabs = new List<ResearchLab>();
            for (int i = 0; i < GameConstNumber.ResearchLabCount; i++)
            {
                ReaserchLabs.Add(new ResearchLab());
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
            PredicateActionList = new Dictionary<string, Func<Faction,bool>>();
            GaiaGame.MapActionMrg.AddMapActionList(ActionList, PredicateActionList);
            GaiaPlanetNumber = 0;

        }

        internal bool FinishIntialMines()
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
        private int m_credit;
        private int m_ore;
        private int m_knowledge;
        private int m_QICs;
        private int m_powerToken1;
        private int m_powerToken2;
        private int m_powerToken3;
        private int m_powerTokenGaia;
        private int m_TransformLevel;
        private int m_ShipLevel;
        private int m_AILevel;
        private int m_GaiaLevel;
        private int m_EconomicLevel;
        private int m_ScienceLevel;
        private const int m_MineOreCost=1;
        private const int m_MineCreditCost = 2;

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
#region 临时变量 判断动作完成与否 需要清零
        private int m_TerraFormNumber=0;
        private int m_TempShip=0;
        private int m_TechTilesGet = 0;
        private int m_TechTrachAdv = 0;
        private int m_AllianceTileGet = 0;
        private int m_AllianceTileReGet = 0;
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
            if (Academy1 == null)
            {
                m_knowledge += 2;
            }

            m_knowledge += GameTileList.Sum(x => x.GetKnowledgeIncome());
        }

        protected virtual void CalQICIncome()
        {
            if (Academy2 == null)
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

        public virtual void PowerIncrease(int i)
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
        public List<GaiaBuilding> Gaias { set; get; }
        public List<Mine> Mines { set; get; }
        public List<TradeCenter> TradeCenters { set; get; }
        public List<ResearchLab> ReaserchLabs { set; get; }
        public Academy Academy1 { set; get; }
        public Academy Academy2 { set; get; }
        public StrongHold StrongHold { set; get; }
        public int Credit { get => m_credit; set => m_credit = value; }
        public int Ore { get => m_ore; set => m_ore = value; }
        public int Knowledge { get => m_knowledge; set => m_knowledge = value; }
        public int QICs { get => m_QICs; set => m_QICs = value; }
        public int PowerToken1 { get => m_powerToken1; set => m_powerToken1=value; }
        public int PowerToken2 { get => m_powerToken2;  }
        public int PowerToken3 { get => m_powerToken3; set => m_powerToken3 = value; }
        public int TransformLevel { get => m_TransformLevel;  }
        public int ShipLevel { get => m_ShipLevel;  }
        public int AILevel { get => m_AILevel; }
        public int GaiaLevel { get => m_GaiaLevel;}
        public int EconomicLevel { get => m_EconomicLevel; }
        public int ScienceLevel { get => m_ScienceLevel; }
        public abstract Terrain OGTerrain { get; }
        public List<Tuple<int,FactionName>> LeechPowerQueue { get; }
        public int Score { get; set; }
        public GaiaGame GaiaGame { get; }
        public Queue<Action> ActionQueue { get; set; }

        public Dictionary<string,Func<Faction,bool>> ActionList { get; set; }
        public Dictionary<string,Func<Faction,bool>> PredicateActionList { get; set; }
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
                else if (m_TransformLevel == 2 | m_TransformLevel == 3)
                {
                    return 2;
                }else if (m_TransformLevel == 4 | m_TransformLevel == 5)
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
        public int PowerTokenGaia { get => m_powerTokenGaia; set => m_powerTokenGaia = value; }
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
            var level= (int)list[index].GetValue(this);
            if (level >= 0 && level < 4)
            {
                //level++;
            }
            else if (level == 4)
            {
                if(GameTileList.Exists(x => x is AllianceTile && x.IsUsed == false))
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
