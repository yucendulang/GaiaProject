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

        internal virtual bool BuildMine(Map map, int row, int col, out string log)
        {

            log = string.Empty;
            bool isGreenPlanet = false;
            //判断是否是Gaia改变过的绿色星球
            bool isGaiaPlanet = false;
            int transNumNeed = 0;
            if(map.HexArray[row, col] == null)
            {
                log = "出界了兄弟";
                return false;
            }
            if (map.HexArray[row, col].TFTerrain == Terrain.Purple)
            {
                log = "不能在紫色星球上建造";
                return false;
            }
            if (map.HexArray[row, col].TFTerrain == Terrain.Empty)
            {
                log = "你必须在星球上进行建造";
                return false;
            }

            //矿石铲子
            int oreTF = 0;
            //航海的Q
            int QSHIP = 0;
            if (map.HexArray[row, col].TFTerrain == Terrain.Green)
            {
                isGreenPlanet = true;
                if (map.HexArray[row, col].TFTerrain == Terrain.Green
                    && map.HexArray[row, col].Building is GaiaBuilding
                    && map.HexArray[row, col].FactionBelongTo == FactionName)
                {
                    isGaiaPlanet = true;
                }
                else
                {
                    TempQICs -= 1;
                    if (QICs < 0)
                    {
                        log = "至少需要一块Q";
                        return false;
                    }
                }
            }
            else
            {
                transNumNeed = Math.Min(7 - Math.Abs(map.HexArray[row, col].OGTerrain - OGTerrain), Math.Abs(map.HexArray[row, col].OGTerrain - OGTerrain));
                oreTF = Math.Max((transNumNeed - TerraFormNumber), 0);
                if (oreTF * GetTransformCost > Ore)
                {
                    log = string.Format("原始地形为{0},需要地形{1},需要矿石为{2}或者使用action行动获取铲子", map.HexArray[row, col].OGTerrain.ToString(), OGTerrain.ToString(), transNumNeed * GetTransformCost);
                    return false;
                }
            }
            if (!(Mines.Count >= 1 && Credit >= m_MineCreditCost && Ore >= m_MineOreCost + Math.Max((transNumNeed - TerraFormNumber), 0) * GetTransformCost))
            {
                log = "资源不够";
                return false;
            }

            if (!isGaiaPlanet && !(map.HexArray[row, col].Building == null && map.HexArray[row, col].FactionBelongTo == null))
            {
                log = "该地点已经有人占领了";
                return false;
            }
            var distanceNeed = map.CalShipDistanceNeed(row, col, FactionName);

            if (!isGaiaPlanet && QICs * 2 < distanceNeed - GetShipDistance)
            {
                log = string.Format("建筑距离太偏远了,需要{0}个Q来加速", (distanceNeed - GetShipDistance + 1) / 2);
                return false;
            }
            QSHIP=Math.Max((distanceNeed - GetShipDistance + 1) / 2, 0);
            //扣资源建建筑
            Action queue = () =>
            {
                Ore -= m_MineOreCost + oreTF * GetTransformCost;
                Credit -= m_MineCreditCost;
                if (isGaiaPlanet)
                {
                    Gaias.Add(map.HexArray[row, col].Building as GaiaBuilding);
                }
                map.HexArray[row, col].Building = Mines.First();
                map.HexArray[row, col].FactionBelongTo = FactionName;
                Mines.RemoveAt(0);
                if (!isGaiaPlanet && isGreenPlanet)
                {
                    QICs = QICs;
                    TempQICs = 0;

                }
                if (isGreenPlanet)
                {
                    TriggerRST(typeof(RST4));
                    TriggerRST(typeof(STT2));
                    GaiaPlanetNumber++;
                }
                GaiaGame.SetLeechPowerQueue(FactionName, row, col);
                TriggerRST(typeof(RST1));
                TriggerRST(typeof(ATT4));
                for (int i = 0; i < transNumNeed; i++)
                {
                    TriggerRST(typeof(RST7));
                }
                var surroundhex = map.GetSurroundhex(row, col, FactionName);
                if (surroundhex.Exists(x => map.HexArray[x.Item1, x.Item2].IsAlliance))
                {
                    map.HexArray[row, col].IsAlliance = true;
                }
                if (!isGaiaPlanet)
                {
                    QICs -= QSHIP;
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

        protected void TriggerRST(Type type)
        {
            if (GaiaGame.GameStatus.stage <= Stage.SELECTROUNDBOOSTER)
                return;
            if (GaiaGame.RSTList[(GaiaGame.GameStatus.RoundCount - 1)].GetType()==type)
            {
                Score += GaiaGame.RSTList[(GaiaGame.GameStatus.RoundCount - 1)].GetTriggerScore;
            }
            if(GameTileList.Exists(x=>x.GetType()==type))
            {
                Score += GameTileList.Find(x => x.GetType() == type).GetTriggerScore;
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
            if (map.HexArray[row, col].TFTerrain != Terrain.Purple)
            {
                log = "必须为紫星";
                return false;
            }
            if (!(map.HexArray[row, col].Building == null && map.HexArray[row, col].FactionBelongTo == null))
            {
                log = "该地点已经有人占领了";
                return false;
            }
            var distanceNeed = map.CalShipDistanceNeed(row, col, FactionName);

            if (QICs * 2 < distanceNeed - GetShipDistance)
            {
                log = string.Format("建筑距离太偏远了,需要{0}个Q来加速", (distanceNeed - GetShipDistance + 1) / 2);
                return false;
            }

            var QSHIP = Math.Max((distanceNeed - GetShipDistance + 1) / 2, 0);
            //扣资源建建筑
            Action queue = () =>
            {
                RemovePowerToken(GetGaiaCost());
                map.HexArray[row, col].Building = Gaias.First();
                map.HexArray[row, col].FactionBelongTo = FactionName;
                PowerTokenGaia += GetGaiaCost();
                Gaias.RemoveAt(0);
                QICs -= QSHIP;
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

        internal bool IsPlanetTypeExist(Terrain terrain)
        {
            var hexList = GaiaGame.Map.GetHexList();

            return hexList.ToList().Exists(x => x.FactionBelongTo == FactionName && !(x.Building is GaiaBuilding) && x.TFTerrain == terrain);
        }
        public virtual void PowerBurnSpecialPreview(int v)
        {
            return;
        }
        public virtual void PowerBurnSpecialActual(int v)
        {
            return;
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

        internal virtual void GaiaPhaseIncome()
        {
            PowerToken1 += PowerTokenGaia;
            PowerTokenGaia = 0;
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

        internal bool UpgradeBuilding(Map map, int row, int col, string buildStr, out string log)
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
            if (hex.TFTerrain==Terrain.Black)
            {
                log = "死星建筑不能升级";
                return false;
            }
            if (Ore < oreCost || Credit < creditCost)
            {
                log = string.Format("资源不够");
                return false;
            }
            if (syn == BuildingSyntax.RL|| syn == BuildingSyntax.AC1 || syn == BuildingSyntax.AC2)
            {
                TechTilesGet++;
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
                    AddGameTiles(new AC2());
                }
                else if (syn == BuildingSyntax.TC)
                {
                    TriggerRST(typeof(ATT5));
                }
                else if (syn == BuildingSyntax.SH)
                {
                    CallSpecialSHBuild();
                }
            };
            ActionQueue.Enqueue(queue);
            return true;
        }

        protected virtual void CallSpecialSHBuild()
        {
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
                var ret=PowerIncrease(power);
                Score -= Math.Max(ret - 1, 0);
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
            if (TechTilesGet > 0)
            {
                log = "还存在没拿取的科技版";
                return true;
            }else if (TechTilesGet < 0)
            {
                log = "板子拿多了喂！想干嘛兄弟";
                return true;
            }
            if (m_AllianceTileGet != 0)
            {
                log = "还存在没拿取的星盟版";
                return true;
            }
            if (TechReturn != 0)
            {
                log = "获取高级科技必须退回一个低级科技";
                return true;
            }
            if (PlanetGet != 0)
            {
                log = "死星没有被放置在地图上";
                return true;
            }
            if (TechTracAdv > 0)
            {
                log = "可以提升科技,请选择一项科技提升,如果不需要提升科技请使用-advance语句";
                return true;
            }
            if (TechTracAdv <-1)
            {
                log = "科技升级多咯";
                return true;
            }
            if (TechTracAdv == -1 && !IsNoAdvTechTrack)
            {
                log = "多升级了一次科技";
                return true;
            }
            if (AllianceTileReGet != 0)
            {
                log = "选择要重新积分的城版";
                return true;
            }
            if (FactionSpecialAbility < 0)
            {
                log = "貌似多用了SH能力?";
                return true;
            }
            if (FactionSpecialAbility > 0)
            {
                log = "SH能力别忘了使用啊";
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
            TempPowerTokenGaia = 0;
            TempCredit = 0;
            TempKnowledge = 0;
            TempOre = 0;
            TempQICs = 0;
            TechReturn = 0;
            PlanetGet = 0;
            IsSingleAdvTechTrack = false;
            IsNoAdvTechTrack = false;
            AllianceTileReGet = 0;
            PlanetAlready = false;
            FactionSpecialAbility = 0;
        }



        static List<string> TechStrList = new List<string>(){
            "tf",
            "ship",
            "ai",
            "gaia",
            "eco",
            "sci",
        };

#region 临时变量 退出前需要清除
        public bool IsUseAction2 { get; internal set; }
        /// <summary>
        /// 代表需要退回的板子数量
        /// </summary>
        public int TechReturn { get; internal set; }
        public int PlanetGet { get; internal set; }
        public bool IsSingleAdvTechTrack { get; internal set; }
        public bool IsNoAdvTechTrack { get; internal set; }
        public bool PlanetAlready { get; internal set; }
        public int FactionSpecialAbility { get; set; }
#endregion

        internal static string ConvertTechIndexToStr(int v)
        {
            return TechStrList[v];
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
                        TriggerRST(typeof(RST5));
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
                        Ore = +3;
                        Credit = +6;
                        PowerIncrease(6);
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
                        Knowledge += 9;
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
                    else if (m_ShipLevel == 5)
                    {
                        GameTileList.Find(x => x is AllianceTile && x.IsUsed == false).IsUsed = true;
                        //throw new NotImplementedException("黑星科技没有完成");
                    }
                    break;
                default:
                    throw new Exception("不存在此科技条" + tech);
            }
            TriggerRST(typeof(RST6));
            TriggerRST(typeof(ATT6));
            return;
        }

        internal GameTiles GameTileGet(string str)
        {
            var ret= GameTileList.Find(x => x.GetType().Name.Equals(str, StringComparison.OrdinalIgnoreCase));
            return ret;
        }

        internal void ForgingAllianceGetTileWithOutSatellite(List<Tuple<int, int>> list)
        {
            Action action = () =>
            {
                list.ForEach(x => GaiaGame.Map.HexArray[x.Item1, x.Item2].IsAlliance=true);
                TriggerRST(typeof(RST5));
            };

            ActionQueue.Enqueue(action);
            m_AllianceTileGet++;
        }

        internal virtual bool BuildBlackPlanet(int row, int col, out string log)
        {
            log = string.Empty;
            var hex = GaiaGame.Map.HexArray[row, col];
            if (hex == null)
            {
                log = "出界了兄弟";
                return false;
            }
            if (hex.TFTerrain != Terrain.Empty)
            {
                log = "死星必须建立在太空中";
                return false;
            }
            if (hex.Satellite.Any())
            {
                log = "不能建在别人的卫星上";
                return false;
            }
            var distanceNeed = GaiaGame.Map.CalShipDistanceNeed(row, col, FactionName);
            TempShip++;
            var qicship = Math.Max((distanceNeed - GetShipDistance + 1) / 2, 0);
            if (QICs * 2 < distanceNeed - GetShipDistance)
            {
                log = string.Format("死星距离太偏远了,需要{0}个Q来加速", Math.Max((distanceNeed - GetShipDistance + 1) / 2, 0));
                return false;
            }


            Action action = () =>
            {
                hex.TFTerrain = Terrain.Black;
                hex.Building = new Mine();
                hex.FactionBelongTo = FactionName;
                var surroundhex = GaiaGame.Map.GetSurroundhex(row, col, FactionName);
                if (surroundhex.Exists(x => GaiaGame.Map.HexArray[x.Item1, x.Item2].IsAlliance))
                {
                    GaiaGame.Map.HexArray[row, col].IsAlliance = true;
                }
                QICs -= qicship;
                GaiaGame.SetLeechPowerQueue(FactionName, row, col);
                TriggerRST(typeof(RST1));
                TriggerRST(typeof(ATT4));
            };
            ActionQueue.Enqueue(action);
            TempShip = 0;
            return true;
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

            if (list.Sum(x => GaiaGame.Map.HexArray[x.Item1, x.Item2].Building.MagicLevel) < m_allianceMagicLevel)
            {
                log = string.Format("魔力等级不够{0}级", m_allianceMagicLevel);
                return false;
            }

            var isConnectList = new List<Tuple<int, int>>();
            var allianceQueue = new Queue<Tuple<int, int>>();
            allianceQueue.Enqueue(list.First());
            while (allianceQueue.Any())
            {
                var hex = allianceQueue.Dequeue();
                var surroundHex = GaiaGame.Map.GetSatellitehex(hex.Item1, hex.Item2, FactionName,new List<Tuple<int, int>>());
                var newlist = surroundHex.Where(x => list.Exists(y => y.Item1 == x.Item1 && y.Item2 == x.Item2))
                    .Where(x => !isConnectList.Exists(y => y.Item1 == x.Item1 && y.Item2 == x.Item2)).ToList();
                newlist.ForEach(x => allianceQueue.Enqueue(x));
                isConnectList.AddRange(newlist);
            }

            if (isConnectList.Count != list.Count)
            {
                log = "没有组成连通的地块";
                return false;
            }

            return true;
        }

        internal virtual bool ConvertOneResourceToAnother(int rFNum, string rFKind, int rTNum, string rTKind, out string log)
        {
            log = string.Empty;
            var str = rFKind + rTKind;
            switch (str)
            {
                case "pwq":
                    if (rFNum != rTNum * 4)
                    {
                        log = "兑换比例为4：1";
                        return false;
                    }
                    if (PowerToken3 < rFNum)
                    {
                        log = "魔力值不够";
                        return false;
                    }
                    TempPowerToken3 -= rFNum;
                    TempPowerToken1 += rFNum;
                    TempQICs += rTNum;
                    Action action = () =>
                    {
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
                        return false;
                    }
                    TempPowerToken3 -= rFNum;
                    TempPowerToken1 += rFNum;
                    TempOre += rTNum;
                    action = () =>
                    {
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
                        return false;
                    }
                    TempPowerToken3 -= rFNum;
                    TempPowerToken1 += rFNum;
                    TempKnowledge += rTNum;
                    action = () =>
                    {
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
                        return false;
                    }
                    TempPowerToken3 -= rFNum;
                    TempPowerToken1 += rFNum;
                    TempCredit += rTNum;
                    action = () =>
                    {
                        PowerToken3 = PowerToken3;
                        PowerToken1 = PowerToken1;
                        Credit = Credit;
                        TempPowerToken3 = 0;
                        TempPowerToken1 = 0;
                        TempCredit = 0;
                    };
                    ActionQueue.Enqueue(action);
                    break;
                case "qo":
                    if (rFNum != rTNum * 1)
                    {
                        log = "兑换比例为1：1";
                        return false;
                    }
                    if (QICs < rFNum)
                    {
                        log = "Q不够";
                        return false;
                    }
                    TempQICs -= rFNum;
                    TempOre += rTNum;
                    action = () =>
                    {
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
                        return false;
                    }
                    TempKnowledge -= rFNum;
                    TempCredit += rTNum;
                    action = () =>
                    {
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
                        return false;
                    }
                    TempOre -= rFNum;
                    TempCredit += rTNum;
                    action = () =>
                    {
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
                        return false;
                    }
                    TempOre -= rFNum;
                    TempPowerToken1 += rTNum;
                    action = () =>
                    {
                        Ore = Ore;
                        PowerToken1 = PowerToken1;
                        TempOre = 0;
                        TempPowerToken1 = 0;
                    };
                    ActionQueue.Enqueue(action);
                    break;
                case "qc":
                    if (rFNum != rTNum * 1)
                    {
                        log = "兑换比例为1：1";
                        return false;
                    }
                    if (Ore < rFNum)
                    {
                        log = "Q不够";
                        return false;
                    }
                    TempQICs -= rFNum;
                    TempCredit += rTNum;
                    action = () =>
                    {
                        QICs = QICs;
                        Credit = Credit;
                        TempQICs = 0;
                        TempCredit = 0;
                    };
                    ActionQueue.Enqueue(action);
                    break;
                case "qpwt":
                    if (rFNum != rTNum * 1)
                    {
                        log = "兑换比例为1：1";
                        return false;
                    }
                    if (QICs < rFNum)
                    {
                        log = "q不够";
                        return false;
                    }
                    TempQICs -= rFNum;
                    TempPowerToken1 += rTNum;
                    action = () =>
                    {
                        QICs = QICs;
                        PowerToken1 = PowerToken1;
                        TempQICs = 0;
                        TempPowerToken1 = 0;
                    };
                    ActionQueue.Enqueue(action);
                    break;
                default:
                    log = "不支持这种转换";
                    return false;
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
                TriggerRST(typeof(RST5));
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
            if (list.Exists(x => (GaiaGame.Map.HexArray[x.Item1, x.Item2].Satellite != null
             && GaiaGame.Map.HexArray[x.Item1, x.Item2].Satellite.Contains(FactionName))
             || GaiaGame.Map.HexArray[x.Item1, x.Item2].TFTerrain != Terrain.Empty))
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
            if (allianceList.Sum(x => GaiaGame.Map.HexArray[x.Item1, x.Item2].Building == null ? 0 : GaiaGame.Map.HexArray[x.Item1, x.Item2].Building.MagicLevel) < m_allianceMagicLevel)
            {
                log = string.Format("魔力等级不够{0}级", m_allianceMagicLevel);
                return false;
            }

            var isConnectList = new List<Tuple<int, int>>();
            allianceQueue = new Queue<Tuple<int, int>>();
            allianceQueue.Enqueue(allianceList.First());
            while (allianceQueue.Any())
            {
                var hex = allianceQueue.Dequeue();
                var surroundHex = GaiaGame.Map.GetSatellitehex(hex.Item1, hex.Item2, FactionName, list);
                var newlist = surroundHex.Where(x => allianceList.Exists(y => y.Item1 == x.Item1 && y.Item2 == x.Item2))
                    .Where(x => !isConnectList.Exists(y => y.Item1 == x.Item1 && y.Item2 == x.Item2)).ToList();
                newlist.ForEach(x => allianceQueue.Enqueue(x));
                isConnectList.AddRange(newlist);
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

        internal bool IsIncreateTechValide(string tech,out string log)
        {
            log = string.Empty;
            var index = TechStrList.FindIndex(x=>x.Equals(tech));
            return IsIncreaseTechLevelByIndexValidate(index,out log);
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
