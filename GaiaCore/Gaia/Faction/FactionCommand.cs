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
            bool isGreenPlanet = false; ;
            if (!(Mines.Count > 1 && m_credit > m_MineCreditCost && m_ore > m_MineOreCost))
            {
                log = "资源不够";
                return false;
            }
            if(map.HexArray[row, col].TFTerrain == Terrain.Green)
            {
                isGreenPlanet = true;
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
            if (isGreenPlanet && m_QICs < 1)
            {
                log = "至少需要一块QIC";
                return false;
            }
            if (!(map.HexArray[row, col].Building == null && map.HexArray[row, col].FactionBelongTo == null))
            {
                log = "该地点已经有人占领了";
                return false;
            }
            if (!map.CalIsBuildValidate(row, col, FactionName, GetShipDistance))
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
            GaiaGame.SetLeechPowerQueue(FactionName, row, col);
            m_TerraFormNumber = 0;
            if (isGreenPlanet)
            {
                m_QICs -= 1;
            }
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
                case BuildingSyntax.AC:
                    build = Academies.First();
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

            m_ore -= oreCost;
            m_credit -= creditCost;
            ReturnBuilding(map.HexArray[row, col].Building);
            map.HexArray[row, col].Building = build;
            RemoveBuilding(build);
            GaiaGame.SetLeechPowerQueue(FactionName,row, col);

            return true;
        }

        internal void Rollback()
        {
            if (m_Backup == null)
            {
                throw new Exception("没有进行backup");
            }
            m_ore = m_Backup.Ore;
            m_TerraFormNumber = 0;
        }

        internal void Backup()
        {
            m_Backup = new FactionBackup();
            m_Backup.Ore = m_ore;
        }

        internal void LeechPower(int power, FactionName factionFrom,bool isLeech)
        {
            LeechPowerQueue.RemoveAt(LeechPowerQueue.FindIndex(x => x.Item1 == power && x.Item2 == factionFrom));
            if (isLeech)
            {
                PowerIncrease(power);
                Score -= Math.Max(power - 1, 0);
            }
        }

        private void RemoveBuilding(Building building)
        {
            switch (building.GetType().Name)
            {
                case "Mine":
                    Mines.RemoveAt(0);
                    break;
                case "TradeCenter":
                    TradeCenters.RemoveAt(0);
                    break;
                case "ReaserchLab":
                    ReaserchLabs.RemoveAt(0);
                    break;
                case "Academy":
                    Academies.RemoveAt(0);
                    break;
                case "StrongHold":
                    StrongHold = null;
                    break;
                default:
                    throw new Exception(building.GetType().ToString() + "不会被移除");
            }
        }

        internal bool SetTransformNumber(int num, out string log)
        {
            log = string.Empty;
            if (num * GetTransformCost > m_ore)
            {
                log = string.Format("{0}改造费用大于拥有矿石数量{1}", num * GetTransformCost, m_ore);
                return false;
            }

            m_ore -= num * GetTransformCost;
            m_TerraFormNumber = num;
            return true;
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
                case "ReaserchLab":
                    ReaserchLabs.Add(building as ReaserchLab);
                    break;
                default:
                    throw new Exception(building.GetType().ToString() + "不会被归还");
            }
        }
    }
}
