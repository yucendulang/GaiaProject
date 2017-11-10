using GaiaCore.Gaia.Tiles;
using System;
using System.Collections;
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
            int transNumNeed = 0;
            if (map.HexArray[row, col].TFTerrain == Terrain.Green)
            {
                isGreenPlanet = true;
                if (map.HexArray[row, col].TFTerrain == Terrain.Green
                    && map.HexArray[row, col].Building is GaiaBuilding
                    && map.HexArray[row, col].FactionBelongTo == FactionName)
                {
                    isGaiaPlanet = true;
                }
            }
            else
            {
                transNumNeed = Math.Min(7 - Math.Abs(map.HexArray[row, col].OGTerrain - OGTerrain), Math.Abs(map.HexArray[row, col].OGTerrain - OGTerrain));
                if (Math.Max((transNumNeed - TerraFormNumber), 0) * GetTransformCost > Ore)
                {
                    log = string.Format("原始地形为{0},需要地形{1},需要矿石为{2}或者使用action行动获取铲子", map.HexArray[row, col].OGTerrain.ToString(), OGTerrain.ToString(), transNumNeed * GetTransformCost);
                    return false;
                }
            }
            if (!(Mines.Count > 1 && Credit >= m_MineCreditCost && Ore >= m_MineOreCost+ Math.Max((transNumNeed - TerraFormNumber), 0) * GetTransformCost))
            {
                log = "资源不够";
                return false;
            }
            if (!isGaiaPlanet&&isGreenPlanet && QICs < 1)
            {
                log = "至少需要一块QIC";
                return false;
            }
            if (!isGaiaPlanet&&!(map.HexArray[row, col].Building == null && map.HexArray[row, col].FactionBelongTo == null))
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
            Action queue = () =>
            {
                Ore -= m_MineOreCost + transNumNeed * GetTransformCost;
                Credit -= m_MineCreditCost;
                if (isGaiaPlanet)
                {
                    Gaias.Add(map.HexArray[row, col].Building as GaiaBuilding);
                }
                map.HexArray[row, col].Building = Mines.First();
                map.HexArray[row, col].FactionBelongTo = FactionName;
                Mines.RemoveAt(0);
                GaiaGame.SetLeechPowerQueue(FactionName, row, col);
                if (!isGaiaPlanet && isGreenPlanet)
                {
                    QICs -= 1;
                }
                if (isGreenPlanet)
                {
                    TriggerRST(typeof(RST4));
                    GaiaPlanetNumber++;
                }
                else
                {
                    TriggerRST(typeof(RST1));
                }
                for (int i = 0; i < transNumNeed; i++)
                {
                    TriggerRST(typeof(RST7));
                }
            };
            ActionQueue.Enqueue(queue);
            TerraFormNumber = 0;
            TempShip = 0;
            return true;
        }

        internal object GetRemainBuildCount()
        {
            var i1 = Academy1 == null ? 0 : 1;
            var i2 = Academy2 == null ? 0 : 1;
            var i3 = StrongHold == null ? 0 : 1;
            return Mines.Count + TradeCenters.Count + ResearchLabs.Count + i1 + i2 + i3;
        }

        internal void PowerUse(int v)
        {
            PowerToken3 -= v;
            PowerToken1 += v;
        }

        private void TriggerRST(Type type)
        {
            if (GaiaGame.RSTList[(GaiaGame.GameStatus.RoundCount - 1)].GetType()==type)
            {
                Score += GaiaGame.RSTList[(GaiaGame.GameStatus.RoundCount - 1)].GetTriggerScore;
            }
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
            if (PowerToken1 + PowerToken2 + PowerToken3 < GetGaiaCost())
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
            if (!map.CalIsBuildValidate(row, col, FactionName, GetShipDistance))
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
                PowerTokenGaia += GetGaiaCost();
                Gaias.RemoveAt(0);
            };
            ActionQueue.Enqueue(queue);
            TempShip = 0;
            return true;
        }

        internal int GetSpaceSectorCount()
        {
            var hexList = GaiaGame.Map.GetHexList();
            var q =
                    from h in hexList
                    where h.FactionBelongTo == FactionName && !(h.Building is GaiaBuilding)
                    group h by h.SpaceSectorName into g
                    select g;
            return q.Count();
        }

        internal int GetPlanetTypeCount()
        {
            var hexList = GaiaGame.Map.GetHexList();
            var q =
                from h in hexList
                where h.FactionBelongTo == FactionName && h.TFTerrain != Terrain.Purple
                group h by h.TFTerrain into g
                select g;
            return q.Count();
        }

        private void RemovePowerToken(int n)
        {
            if (PowerToken1 + PowerToken2 + PowerToken3 <n)
            {
                throw new Exception(string.Format("没有{0}个魔力豆来移除",n));
            }
            PowerToken1 -= n;
            if (PowerToken1 < 0)
            {
                PowerToken2 += PowerToken1;
                PowerToken1 = 0;
            }
            if (PowerToken2 < 0)
            {
                PowerToken3 += PowerToken2;
                PowerToken2 = 0;
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
            Type trigger;
            switch (syn)
            {
                case BuildingSyntax.TC:
                    build = TradeCenters.First();
                    oreCost = m_TradeCenterOreCost;
                    if (GaiaGame.FactionList.Where(x => x != this).ToList().Exists(y => GaiaGame.Map.GetSurroundhex(row, col, y.FactionName,2).Count != 0))
                    {
                        creditCost = m_TradeCenterCreditCostCluster;
                    }
                    else
                    {
                        creditCost = m_TradeCenterCreditCostAlone;
                    }       
                    trigger = typeof(RST2);
                    break;
                case BuildingSyntax.RL:
                    build = ResearchLabs.First();
                    oreCost = m_ReaserchLabOreCost;
                    creditCost = m_ReaserchLabCreditCost;
                    trigger = null;
                    break;
                case BuildingSyntax.SH:
                    build = StrongHold;
                    oreCost = m_StrongHoldOreCost;
                    creditCost = m_StrongHoldCreditCost;
                    trigger = typeof(RST3);
                    break;
                case BuildingSyntax.AC1:
                    build = Academy1;
                    oreCost = m_AcademyOreCost;
                    creditCost = m_AcademyCreditCost;
                    trigger = typeof(RST3);
                    break;
                case BuildingSyntax.AC2:
                    build = Academy2;
                    oreCost = m_AcademyOreCost;
                    creditCost = m_AcademyCreditCost;
                    trigger = typeof(RST3);
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
                log = string.Format("需要基础建筑类型{0}", build.BaseBuilding.ToString());
                return false;
            }
            if (Ore < oreCost || Credit < creditCost)
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
                Ore -= oreCost;
                Credit -= creditCost;
                ReturnBuilding(map.HexArray[row, col].Building);
                map.HexArray[row, col].Building = build;
                RemoveBuilding(syn);
                GaiaGame.SetLeechPowerQueue(FactionName, row, col);
                if (trigger != null)
                {
                    TriggerRST(trigger);
                }
                if (syn == BuildingSyntax.AC2)
                {
                    var tile = new AC2();
                    GameTileList.Add(tile);
                    ActionList.Add(tile.GetType().Name.ToLower(), tile.InvokeGameTileAction);
                }
            };
            ActionQueue.Enqueue(queue);
            return true;
        }

        internal int GetTechLevelbyIndex(int index)
        {
            switch (index)
            {
                case 0:return TransformLevel;
                case 1:return ShipLevel;
                case 2:return AILevel;
                case 3:return GaiaLevel;
                case 4:return EconomicLevel;
                case 5:return ScienceLevel;
                default:
                    throw new Exception("index越界"+index);
            }
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
                    ResearchLabs.RemoveAt(0);
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
            if (TerraFormNumber != 0 && !(IsUseAction2&&TerraFormNumber==1))
            {
                log = "还存在没使用的Transform";
                return true;
            }
            if (TempShip != 0)
            {
                log = "还存在没使用的QICSHIP";
                return true;
            }
            if (m_TechTilesGet != 0)
            {
                log = "还存在没拿取的科技版";
                return true;
            }
            if (m_AllianceTileGet != 0)
            {
                log = "还存在没拿取的星盟版";
                return true;
            }
            return false;
        }


        internal void ResetUnfinishAction()
        {
            ActionQueue.Clear();
            TerraFormNumber = 0;
            TempShip = 0;
            m_TechTilesGet = 0;
            m_TechTrachAdv = 0;
            m_AllianceTileGet = 0;
            LimitTechAdvance = string.Empty;
            IsUseAction2 = false;
            TempPowerToken1 = 0;
            TempPowerToken2 = 0;
            TempPowerToken3 = 0;
            TempCredit = 0;
            TempKnowledge = 0;
            TempOre = 0;
            TempQICs = 0;
        }



        static List<string> TechStrList = new List<string>(){
            "tf",
            "ship",
            "ai",
            "gaia",
            "eco",
            "sci",
        };

        public bool IsUseAction2 { get; internal set; }

        internal static string ConvertTechIndexToStr(int v)
        {
            return TechStrList[v];
        }

        internal bool SetQICShip(int num, out string log)
        {
            log = string.Empty;
            if (num > QICs)
            {
                log = string.Format("使用{0}QICSHIP需要足够的QIC", num);
                return false;
            }
            
            Action queue = () =>
            {
                QICs -= QICs;
                TempQICs = 0;
            };
            ActionQueue.Enqueue(queue);

            TempQICs -= num;
            TempShip += num*2;
            return true;
        }

        internal void IncreaseTech(string tech)
        {
            switch (tech)
            {
                case "tf":
                    m_TransformLevel++;
                    if (m_TransformLevel == 1)
                    {
                        Ore += 2;
                    }
                    else if (m_TransformLevel == 3)
                    {
                        PowerIncrease(GameConstNumber.TechLv2toLv3BonusPower);
                    }
                    else if (m_TransformLevel == 4)
                    {
                        Ore += 2;
                    }
                    else if (m_TransformLevel == 5)
                    {
                        GameTileList.Find(x => x is AllianceTile && x.IsUsed == false).IsUsed = true;
                        GameTileList.Add(GaiaGame.AllianceTileForTransForm);
                        GaiaGame.AllianceTileForTransForm.OneTimeAction(this);
                    }
                    break;
                case "ai":
                    m_AILevel++;
                    if (m_AILevel == 1 || m_AILevel == 2)
                    {
                        QICs += 1;
                    }else if (m_AILevel == 3)
                    {
                        QICs += 2;
                        PowerIncrease(GameConstNumber.TechLv2toLv3BonusPower);
                    }else if (m_AILevel == 4)
                    {
                        QICs += 2;
                    }
                    else if(m_AILevel==5)
                    {
                        GameTileList.Find(x => x is AllianceTile && x.IsUsed == false).IsUsed = true;
                        QICs += 4;
                    }
                    break;
                case "eco":
                    m_EconomicLevel++;
                    if (m_EconomicLevel==3)
                    {
                        PowerIncrease(GameConstNumber.TechLv2toLv3BonusPower);
                    }else if (m_EconomicLevel == 5)
                    {
                        GameTileList.Find(x => x is AllianceTile && x.IsUsed == false).IsUsed = true;
                    }
                    break;
                case "gaia":
                    m_GaiaLevel++;
                    if (m_GaiaLevel == 1)
                    {
                        Gaias.Add(new GaiaBuilding());
                    }else if(m_GaiaLevel == 2)
                    {
                        PowerToken1 += 3;
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
                        GameTileList.Find(x => x is AllianceTile && x.IsUsed == false).IsUsed = true;
                        Score +=4;
                        Score += GaiaPlanetNumber * 1;
                    }
                    break;
                case "sci":
                    m_ScienceLevel++;
                    if (m_ScienceLevel == 3)
                    {
                        PowerIncrease(GameConstNumber.TechLv2toLv3BonusPower);
                    }
                    else if (m_ScienceLevel == 5)
                    {
                        GameTileList.Find(x => x is AllianceTile && x.IsUsed == false).IsUsed = true;
                    }
                    break;
                case "ship":
                    m_ShipLevel++;
                    if (m_ShipLevel == 1)
                    {
                        QICs += 1;
                    }
                    else if (m_ShipLevel == 3)
                    {
                        QICs += 1;
                        PowerIncrease(GameConstNumber.TechLv2toLv3BonusPower);
                    }
                    else if (m_AILevel == 5)
                    {
                        GameTileList.Find(x => x is AllianceTile && x.IsUsed == false).IsUsed = true;
                        throw new NotImplementedException("黑星科技没有完成");
                    }
                    break;
                default:
                    throw new Exception("不存在此科技条" + tech);
            }
            TriggerRST(typeof(RST6));
            return;
        }

        internal void ForgingAllianceGetTileWithOutSatellite(List<Tuple<int, int>> list)
        {
            Action action = () =>
            {
                list.ForEach(x => GaiaGame.Map.HexArray[x.Item1, x.Item2].IsAlliance=true);
            };

            ActionQueue.Enqueue(action);
            m_AllianceTileGet++;
        }

        internal bool ForgingAllianceCheckAllWithOutSatellite(List<Tuple<int, int>> list, out string log)
        {
            log = string.Empty;
            if(!list.TrueForAll(x=>
            {
                TerrenHex terrenHex = GaiaGame.Map.HexArray[x.Item1, x.Item2];
                return terrenHex.Building != null && terrenHex.FactionBelongTo == FactionName && !(terrenHex.Building is GaiaBuilding);
            }))
            {
                log = "形成星盟的只能是本家的非盖亚建筑物";
                return false;
            }

            if (list.Exists(x =>
            {
                TerrenHex terrenHex = GaiaGame.Map.HexArray[x.Item1, x.Item2];
                return terrenHex.IsAlliance == true;
            }))
            {
                log = "有地块已经形成过星盟";
                return false;
            }

            if (list.Exists(x =>
            {
                TerrenHex terrenHex = GaiaGame.Map.HexArray[x.Item1, x.Item2];
                var surroundHex = GaiaGame.Map.GetSurroundhex(x.Item1, x.Item2, FactionName);
                return surroundHex.Exists(y => GaiaGame.Map.HexArray[y.Item1, y.Item2].IsAlliance);
            }))
            {
                log = "周围接壤的地块也不能形成过星盟";
                return false;
            }

            if (list.Sum(x => GaiaGame.Map.HexArray[x.Item1, x.Item2].Building.MagicLevel) < 7)
            {
                log = "魔力等级不够7级";
                return false;
            }

            return true;
        }

        internal bool ConvertOneResourceToAnother(int rFNum, string rFKind, int rTNum, string rTKind, out string log)
        {
            log = string.Empty;
            var str = rFKind + rTKind;
            switch (str)
            {
                case "pwqic":
                    if (rFNum != rTNum * 4)
                    {
                        log = "兑换比例为4：1";
                        return false;
                    }
                    if (PowerToken3 < rFNum)
                    {
                        log = "魔力值不够";
                    }
                    TempPowerToken3 -= rFNum;
                    TempPowerToken1 += rFNum;
                    TempQICs += rTNum;
                    Action action=() =>{
                        PowerToken3 = PowerToken3;
                        PowerToken1 = PowerToken1;
                        QICs = QICs;
                        TempPowerToken3 = 0;
                        TempPowerToken1 = 0;
                        TempQICs = 0;
                    };
                    ActionQueue.Enqueue(action);
                    break;
                case "pwo":
                    if (rFNum != rTNum * 3)
                    {
                        log = "兑换比例为3：1";
                        return false;
                    }
                    if (PowerToken3 < rFNum)
                    {
                        log = "魔力值不够";
                    }
                    TempPowerToken3 -= rFNum;
                    TempPowerToken1 += rFNum;
                    TempOre += rTNum;
                    action = () => {
                        PowerToken3 = PowerToken3;
                        PowerToken1 = PowerToken1;
                        Ore = Ore;
                        TempPowerToken3 = 0;
                        TempPowerToken1 = 0;
                        TempOre = 0;
                    };
                    ActionQueue.Enqueue(action);
                    break;
                case "pwk":
                    if (rFNum != rTNum * 4)
                    {
                        log = "兑换比例为4：1";
                        return false;
                    }
                    if (PowerToken3 < rFNum)
                    {
                        log = "魔力值不够";
                    }
                    TempPowerToken3 -= rFNum;
                    TempPowerToken1 += rFNum;
                    TempKnowledge += rTNum;
                    action = () => {
                        PowerToken3 = PowerToken3;
                        PowerToken1 = PowerToken1;
                        Knowledge = Knowledge;
                        TempPowerToken3 = 0;
                        TempPowerToken1 = 0;
                        TempKnowledge = 0;
                    };
                    ActionQueue.Enqueue(action);
                    break;
                case "pwc":
                    if (rFNum != rTNum * 1)
                    {
                        log = "兑换比例为1：1";
                        return false;
                    }
                    if (PowerToken3 < rFNum)
                    {
                        log = "魔力值不够";
                    }
                    TempPowerToken3 -= rFNum;
                    TempPowerToken1 += rFNum;
                    TempCredit += rTNum;
                    action = () => {
                        PowerToken3 = PowerToken3;
                        PowerToken1 = PowerToken1;
                        Credit = Credit;
                        TempPowerToken3 = 0;
                        TempPowerToken1 = 0;
                        TempCredit = 0;
                    };
                    ActionQueue.Enqueue(action);
                    break;
                case "qico":
                    if (rFNum != rTNum * 1)
                    {
                        log = "兑换比例为1：1";
                        return false;
                    }
                    if (QICs < rFNum)
                    {
                        log = "QIC不够";
                    }
                    TempQICs -= rFNum;
                    TempOre += rTNum;
                    action = () => {
                        QICs = QICs;
                        Ore = Ore;
                        TempQICs = 0;
                        TempOre = 0;
                    };
                    ActionQueue.Enqueue(action);
                    break;
                case "kc":
                    if (rFNum != rTNum * 1)
                    {
                        log = "兑换比例为1：1";
                        return false;
                    }
                    if (Knowledge < rFNum)
                    {
                        log = "知识不够";
                    }
                    TempKnowledge -= rFNum;
                    TempCredit += rTNum;
                    action = () => {
                        Knowledge = Knowledge;
                        Credit = Credit;
                        TempKnowledge = 0;
                        TempCredit = 0;
                    };
                    ActionQueue.Enqueue(action);
                    break;
                case "oc":
                    if (rFNum != rTNum * 1)
                    {
                        log = "兑换比例为1：1";
                        return false;
                    }
                    if (Ore < rFNum)
                    {
                        log = "矿不够";
                    }
                    TempOre -= rFNum;
                    TempCredit += rTNum;
                    action = () => {
                        Ore = Ore;
                        Credit = Credit;
                        TempOre = 0;
                        TempCredit = 0;
                    };
                    ActionQueue.Enqueue(action);
                    break;
                case "opwt":
                    if (rFNum != rTNum * 1)
                    {
                        log = "兑换比例为1：1";
                        return false;
                    }
                    if (Ore < rFNum)
                    {
                        log = "矿不够";
                    }
                    TempOre -= rFNum;
                    TempPowerToken1 += rTNum;
                    action = () => {
                        Ore = Ore;
                        PowerToken1 = PowerToken1;
                        TempOre = 0;
                        TempPowerToken1 = 0;
                    };
                    ActionQueue.Enqueue(action);
                    break;
                default:
                    throw new Exception("不支持这种转换");
            }
            return true;
        }

        internal bool GetAllianceTile(AllianceTile alt, out string log)
        {
            log = string.Empty;
            if (m_AllianceTileGet == 0)
            {
                log = "没有获得城版的权限";
                return false;
            }
            Action action = () =>
            {
                GameTileList.Add(alt);
                alt.OneTimeAction(this);
                GaiaGame.ALTList.Remove(alt);
            };
            ActionQueue.Enqueue(action);
            m_AllianceTileGet--;
            return true;
        }

        internal void ForgingAllianceGetTile(List<Tuple<int, int>> list)
        {
            Action action = () =>
            {
                ForgingAlliance(list, out string log, true);
            };
            ActionQueue.Enqueue(action);
            m_AllianceTileGet++;
        }

        internal bool ForgingAllianceCheckAll(List<Tuple<int, int>> list, out string log)
        {
            log = string.Empty;
            if (PowerToken1 + PowerToken2 + PowerToken3 < list.Count)
            {
                log = "魔力豆要比卫星数量多";
                return false;
            }
            if (!list.TrueForAll(x => !GaiaGame.Map.HexArray[x.Item1, x.Item2].Satellite.Contains(FactionName)
             && GaiaGame.Map.HexArray[x.Item1, x.Item2].TFTerrain == Terrain.Empty))
            {
                log = "卫星不能建立在非空地以及形成过星盟的地盘上";
                return false;
            }
            if (!ForgingAlliance(list, out log))
            {
                return false;
            }
            foreach (var item in list)
            {
                var dump = new List<Tuple<int, int>>(list);
                dump.Remove(item);
                if (ForgingAlliance(dump, out log))
                {
                    log = string.Format("缺少{0}{1}也能形成星盟", item.Item1, item.Item2);
                    return false;
                }
            }
            return true;
        }
        internal bool ForgingAlliance(List<Tuple<int, int>> list, out string log, bool isSetFlag = false)
        {
            log = string.Empty;

            Queue<Tuple<int, int>> allianceQueue = new Queue<Tuple<int, int>>();
            List<Tuple<int, int>> allianceList = new List<Tuple<int, int>>();
            list.ForEach(x => allianceQueue.Enqueue(x));
            list.ForEach(x => allianceList.Add(x));
            while (allianceQueue.Any())
            {
                var hex = allianceQueue.Dequeue();
                var surroundHex = GaiaGame.Map.GetSurroundhex(hex.Item1, hex.Item2, FactionName);
                foreach (var shex in surroundHex)
                {
                    if (GaiaGame.Map.HexArray[hex.Item1, hex.Item2].TFTerrain != Terrain.Empty && GaiaGame.Map.HexArray[hex.Item1, hex.Item2].IsAlliance)
                    {
                        log = string.Format("跟{0}{1}格接壤,不符合星盟规则");
                        return false;
                    }
                }
                var newhex = surroundHex.Where(x => !allianceList.Exists(y => x.Item1 == y.Item1 && x.Item2 == y.Item2));
                newhex.ToList().ForEach(x => allianceQueue.Enqueue(x));
                newhex.ToList().ForEach(x => allianceList.Add(x));
            }
            if (allianceList.Sum(x => GaiaGame.Map.HexArray[x.Item1, x.Item2].Building == null ? 0 : GaiaGame.Map.HexArray[x.Item1, x.Item2].Building.MagicLevel) < 7)
            {
                log = "魔力等级不够7级";
                return false;
            }

            var isConnectList = new List<Tuple<int, int>>();
            allianceQueue = new Queue<Tuple<int, int>>();
            allianceQueue.Enqueue(allianceList.First());
            while (allianceQueue.Any())
            {
                var hex = allianceQueue.Dequeue();
                isConnectList.Add(hex);
                var surroundHex = GaiaGame.Map.GetSatellitehex(hex.Item1, hex.Item2, FactionName, list);
                var newlist = surroundHex.Where(x => allianceList.Exists(y => y.Item1 == x.Item1 && y.Item2 == x.Item2))
                    .Where(x => !isConnectList.Exists(y => y.Item1 == x.Item1 && y.Item2 == x.Item2)).ToList();
                newlist.ForEach(x => allianceQueue.Enqueue(x));
            }

            if (isConnectList.Count != allianceList.Count)
            {
                log = "没有组成连通的地块";
                return false;
            }

            if (isSetFlag)
            {
                allianceList.ForEach(x =>
                {
                    if (GaiaGame.Map.HexArray[x.Item1, x.Item2].OGTerrain != Terrain.Empty)
                    {
                        GaiaGame.Map.HexArray[x.Item1, x.Item2].IsAlliance = true;
                    }
                    else
                    {
                        GaiaGame.Map.HexArray[x.Item1, x.Item2].AddSatellite(FactionName);
                    }
                });
                RemovePowerToken(list.Count);
            }

            return true;
        }

        internal bool PredicateAction(string actionStr, out string log)
        {
            log = string.Empty;
            if (PredicateActionList.ContainsKey(actionStr)&& !PredicateActionList[actionStr].Invoke(this))
            {
                log = "此行动不可用";
                return false;
            }
            if (!ActionList.ContainsKey(actionStr))
            {
                log = "没有做此行动的板子";
                return false;
            }
            return true;
        }

        internal void DoAction(string actionStr,bool isFreeSyntax=false)
        {
            var func = ActionList[actionStr];
            if (isFreeSyntax)
            {
                func.Invoke(this);
            }
            else
            {
                Action action = () =>
                {
                    func.Invoke(this);
                };
                ActionQueue.Enqueue(action);
            }
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
                    ResearchLabs.Add(building as ResearchLab);
                    break;
                default:
                    throw new Exception(building.GetType().ToString() + "不会被归还");
            }
        }
    }
}
