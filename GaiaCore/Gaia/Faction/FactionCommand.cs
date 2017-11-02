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

        internal bool UpdateBuilding(Map map, int row, int col, string buildStr, out string log)
        {
            log = string.Empty;
            var hex = map.HexArray[row, col];
            if (hex.FactionBelongTo != FactionName)
            {
                log = string.Format("该地点还不属于{0}", FactionName.ToString());
                return false;
            }
            if (!Enum.TryParse(buildStr, out BuildingSyntax syn))
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

            return true;
        }

        private void RemoveBuilding(Building building)
        {
            switch (building.GetType().ToString())
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

        private void ReturnBuilding(Building building)
        {
            switch (building.GetType().ToString())
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
