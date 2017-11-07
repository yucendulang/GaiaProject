using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GaiaCore.Gaia
{
    public abstract partial class Faction
    {

        internal bool BuildMine(Map map, int row, int col, out string log)
        {
            log = string.Empty;
            bool isGreenPlanet = false;
            bool isGaiaPlanet = false;
            if (!(Mines.Count > 1 && m_credit > m_MineCreditCost && m_ore > m_MineOreCost))
            {
                log = "资源不够";
                return false;
            }
            if (map.HexArray[row, col].TFTerrain == Terrain.Green)
            {
                isGreenPlanet = true;
                if(map.HexArray[row,col].TFTerrain==Terrain.Green
                    &&map.HexArray[row,col].Building is GaiaBuilding
                    &&map.HexArray[row,col].FactionBelongTo== FactionName)
                {
                    isGaiaPlanet = true;
                }
            }
            else
            {
                var transNumNeed = Math.Min(7 - Math.Abs(map.HexArray[row, col].OGTerrain - OGTerrain), Math.Abs(map.HexArray[row, col].OGTerrain - OGTerrain));
                if (!(transNumNeed <= m_TerraFormNumber))
                {
                    log = string.Format("原始地形为{0},需要地形{1},需要改造等级为{2}", map.HexArray[row, col].OGTerrain.ToString(), OGTerrain.ToString(), transNumNeed);
                    return false;
                }
            }
            if (!isGaiaPlanet&&isGreenPlanet && m_QICs < 1)
            {
                log = "至少需要一块QIC";
                return false;
            }
            if (!isGaiaPlanet&&!(map.HexArray[row, col].Building == null && map.HexArray[row, col].FactionBelongTo == null))
            {
                log = "该地点已经有人占领了";
                return false;
            }
            if (!map.CalIsBuildValidate(row, col, FactionName, GetShipDistance + m_QICShip))
            {
                log = "航海距离不够";
                return false;
            }
            //扣资源建建筑
            Action queue = () =>
              {
                  m_ore -= m_MineOreCost;
                  m_credit -= m_MineCreditCost;
                  if (isGaiaPlanet)
                  {
                      Gaias.Add(map.HexArray[row, col].Building as GaiaBuilding);
                  }
                  map.HexArray[row, col].Building = Mines.First();
                  map.HexArray[row, col].FactionBelongTo = FactionName;
                  Mines.RemoveAt(0);
                  GaiaGame.SetLeechPowerQueue(FactionName, row, col);
                  if (!isGaiaPlanet&&isGreenPlanet)
                  {
                      m_QICs -= 1;
                  }
              };
            ActionQueue.Enqueue(queue);
            m_TerraFormNumber = 0;
            m_QICShip = 0;
            return true;
        }
        internal bool BuildGaia(Map map, int row, int col, out string log)
        {
            log = string.Empty;
            if (m_GaiaLevel == 0)
            {
                log = "执行盖亚行动至少需要一级GaiaLevel";
                return false;
            }
            if (Gaias.Count == 0)
            {
                log = "没有可用的盖亚建筑";
                return false;
            }
            if (m_powerToken1+m_powerToken2+m_powerToken3<GetGaiaCost())
            {
                log = "魔力豆资源不够";
                return false;
            }
            if (map.HexArray[row, col].TFTerrain !=Terrain.Purple)
            {
                log = "必须为紫星";
                return false;
            }
            if (!(map.HexArray[row, col].Building == null && map.HexArray[row, col].FactionBelongTo == null))
            {
                log = "该地点已经有人占领了";
                return false;
            }
            if (!map.CalIsBuildValidate(row, col, FactionName, GetShipDistance + m_QICShip))
            {
                log = "航海距离不够";
                return false;
            }
            //扣资源建建筑
            Action queue = () =>
            {
                RemovePowerToken(GetGaiaCost());
                map.HexArray[row, col].Building = Gaias.First();
                map.HexArray[row, col].FactionBelongTo = FactionName;
            };
            ActionQueue.Enqueue(queue);
            m_QICShip = 0;
            return true;
        }

        private void RemovePowerToken(int n)
        {
            if (m_powerToken1 + m_powerToken2 + m_powerToken3 < GetGaiaCost())
            {
                throw new Exception(string.Format("没有{0}个魔力豆来移除",n));
            }
            m_powerToken1 -= n;
            if (m_powerToken1 < 0)
            {
                m_powerToken2 += m_powerToken1;
                m_powerToken1 = 0;
            }
            if (m_powerToken2 < 0)
            {
                m_powerToken3 += m_powerToken2;
                m_powerToken2 = 0;
            }
        }

        private int GetGaiaCost()
        {
            if (m_GaiaLevel == 0)
            {
                throw new Exception("0级GaiaLevel不能造Gaia建筑");
            }else if (m_GaiaLevel == 1 || m_GaiaLevel == 2)
            {
                return 6;
            }else if (m_GaiaLevel == 3)
            {
                return 4;
            }else if (m_GaiaLevel == 4 || m_GaiaLevel == 5)
            {
                return 3;
            }
            throw new Exception(m_GaiaLevel+"级GaiaLevel出错");
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

        internal bool UpdateBuilding(Map map, int row, int col, string buildStr, out string log)
        {
            log = string.Empty;
            var hex = map.HexArray[row, col];
            if (hex.FactionBelongTo != FactionName)
            {
                log = string.Format("该地点还不属于{0}", FactionName.ToString());
                return false;
            }
            if (!Enum.TryParse(buildStr, true, out BuildingSyntax syn))
            {
                log = string.Format("没有该建筑物", buildStr);
                return false;
            }
            int oreCost;
            int creditCost;
            Building build;
            switch (syn)
            {
                case BuildingSyntax.TC:
                    build = TradeCenters.First();
                    oreCost = m_TradeCenterOreCost;
                    creditCost = m_TradeCenterCreditCostCluster;
                    break;
                case BuildingSyntax.RL:
                    build = ReaserchLabs.First();
                    oreCost = m_ReaserchLabOreCost;
                    creditCost = m_ReaserchLabCreditCost;
                    break;
                case BuildingSyntax.SH:
                    build = StrongHold;
                    oreCost = m_StrongHoldOreCost;
                    creditCost = m_StrongHoldCreditCost;
                    break;
                case BuildingSyntax.AC1:
                    build = Academy1;
                    oreCost = m_AcademyOreCost;
                    creditCost = m_AcademyCreditCost;
                    break;
                case BuildingSyntax.AC2:
                    build = Academy2;
                    oreCost = m_AcademyOreCost;
                    creditCost = m_AcademyCreditCost;
                    break;
                default:
                    log = "未知升级";
                    return false;
            }
            if (build == null)
            {
                log = string.Format("需要该建筑{0}还有剩余", buildStr);
                return false;
            }
            if (hex.Building.GetType() != build.BaseBuilding)
            {
                log = string.Format("需要基础建筑类型{0}", hex.Building.GetType().ToString());
                return false;
            }
            if (m_ore < oreCost || m_credit < creditCost)
            {
                log = string.Format("资源不够");
                return false;
            }
            if (syn == BuildingSyntax.RL|| syn == BuildingSyntax.AC1 || syn == BuildingSyntax.AC2)
            {
                m_TechTilesGet++;
                m_TechTrachAdv++;
            }
            //扣资源,执行操作
            Action queue = () =>
            {
                m_ore -= oreCost;
                m_credit -= creditCost;
                ReturnBuilding(map.HexArray[row, col].Building);
                map.HexArray[row, col].Building = build;
                RemoveBuilding(syn);
                GaiaGame.SetLeechPowerQueue(FactionName, row, col);
            };
            ActionQueue.Enqueue(queue);


            return true;
        }

        internal void LeechPower(int power, FactionName factionFrom, bool isLeech)
        {
            LeechPowerQueue.RemoveAt(LeechPowerQueue.FindIndex(x => x.Item1 == power && x.Item2 == factionFrom));
            if (isLeech)
            {
                PowerIncrease(power);
                Score -= Math.Max(power - 1, 0);
            }
        }

        private void RemoveBuilding(BuildingSyntax syn)
        {
            switch (syn)
            {
                case BuildingSyntax.M:
                    Mines.RemoveAt(0);
                    break;
                case BuildingSyntax.TC:
                    TradeCenters.RemoveAt(0);
                    break;
                case BuildingSyntax.RL:
                    ReaserchLabs.RemoveAt(0);
                    break;
                case BuildingSyntax.AC1:
                    Academy1 = null;
                    break;
                case BuildingSyntax.AC2:
                    Academy2 = null;
                    break;
                case BuildingSyntax.SH:
                    StrongHold = null;
                    break;
                default:
                    throw new Exception(syn.ToString() + "不会被移除");
            }
        }

        internal bool IsExitUnfinishFreeAction(out string log)
        {
            log = string.Empty;
            if (m_TerraFormNumber != 0)
            {
                log = "还存在没使用的Transform";
                return true;
            }
            if (m_QICShip != 0)
            {
                log = "还存在没使用的QICSHIP";
                return true;
            }
            if (m_TechTilesGet != 0)
            {
                log = "还存在没拿取的科技版";
                return true;
            }
            return false;
        }


        internal void ResetUnfinishAction()
        {
            ActionQueue.Clear();
            m_TerraFormNumber = 0;
            m_QICShip = 0;
            m_TechTilesGet = 0;
            m_TechTrachAdv = 0;
            LimitTechAdvance = string.Empty;
        }

        internal bool SetTransformNumber(int num, out string log)
        {
            log = string.Empty;
            if (num * GetTransformCost > m_ore)
            {
                log = string.Format("{0}改造费用大于拥有矿石数量{1}", num * GetTransformCost, m_ore);
                return false;
            }
            Action queue = () =>
            {
                m_ore -= num * GetTransformCost;
            };
            ActionQueue.Enqueue(queue);
            m_TerraFormNumber = num;
            return true;
        }

        internal void AdvanceTechByIndex(int v){
            switch (v)
            {
                case 0:m_TransformLevel ++;break;
                case 1:m_ShipLevel++; break;
                case 2:m_AILevel++;break;
                case 3:m_GaiaLevel++;break;
                case 4:m_EconomicLevel++;break;
                case 5:m_ScienceLevel++;break;
                default:
                    throw new Exception(string.Format("科技条只支持0-5,不支持{0}",v));
            }
        }
        static List<string> TechStrList = new List<string>(){
            "tf",
            "ship",
            "ai",
            "gaia",
            "eco",
            "sci",
        };

        internal static string ConvertTechIndexToStr(int v)
        {
            return TechStrList[v];
        }

        internal bool SetQICShip(int num, out string log)
        {
            log = string.Empty;
            if (num > m_QICs)
            {
                log = string.Format("使用{0}QICSHIP需要足够的QIC", num);
                return false;
            }

            Action queue = () =>
            {
                m_QICs -= num;
            };
            ActionQueue.Enqueue(queue);


            m_QICShip = num*2;
            return true;
        }

        internal void IncreaseTech(string tech)
        {
            switch (tech)
            {
                case "tf":
                    m_TransformLevel++;
                    break;
                case "ai":
                    m_AILevel++;
                    break;
                case "eco":
                    m_EconomicLevel++;
                    break;
                case "gaia":
                    m_GaiaLevel++;
                    if (m_GaiaLevel == 1)
                    {
                        Gaias.Add(new GaiaBuilding());
                    }else if(m_GaiaLevel == 2)
                    {
                        m_powerToken1 += 3;
                    }
                    else if (m_GaiaLevel == 3)
                    {
                        PowerIncrease(GameConstNumber.TechLv2toLv3BonusPower);
                        Gaias.Add(new GaiaBuilding());
                    }
                    else if (m_GaiaLevel == 4)
                    {
                        Gaias.Add(new GaiaBuilding());
                    }
                    else if(m_GaiaLevel ==5)
                    {
                        throw new NotImplementedException();
                    }
                    break;
                case "sci":
                    m_ScienceLevel++;
                    break;
                case "ship":
                    m_ShipLevel++;
                    break;
                default:
                    throw new Exception("不存在此科技条" + tech);
            }
            return;
        }

        internal bool IsIncreateTechValide(string tech)
        {
            var index = TechStrList.FindIndex(x=>x.Equals(tech));
            return IsIncreaseTechLevelByIndexValidate(index);
        }

        private void ReturnBuilding(Building building)
        {
            switch (building.GetType().Name)
            {
                case "Mine":
                    Mines.Add(building as Mine);
                    break;
                case "TradeCenter":
                    TradeCenters.Add(building as TradeCenter);
                    break;
                case "ResearchLab":
                    ReaserchLabs.Add(building as ResearchLab);
                    break;
                default:
                    throw new Exception(building.GetType().ToString() + "不会被归还");
            }
        }
    }
}
