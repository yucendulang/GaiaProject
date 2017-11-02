using GaiaCore.Gaia.Tiles;
using GaiaCore.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace GaiaCore.Gaia
{
    /// <summary>
    /// 每一局游戏实例化一个Game
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class GaiaGame
    {
        private string m_TailLog=string.Empty;

        public GaiaGame(string[] username)
        {
            GameStatus = new GameStatus();
            FactionList = new List<Faction>();
            Username = username;
        }
        public bool ProcessSyntax(string syntax, out string log)
        {
            log = string.Empty;
            if (syntax.StartsWith("#"))
                return false;
            syntax = syntax.ToLower();
            bool ret;
            switch (GameStatus.stage)
            {
                case Stage.RANDOMSETUP:
                    return ProcessSyntaxRandomSetup(syntax, ref log);
                case Stage.FACTIONSELECTION:
                    return ProcessSyntaxFactionSelect(syntax, ref log);
                case Stage.INITIALMINES:
                    ret = ProcessSyntaxIntialMines(syntax, ref log);
                    if (ret)
                    {
                        GameStatus.NextPlayerForIntial();
                    }
                    if (ret && FactionList.All(x => x.FinishIntialMines()))
                    {
                        ChangeGameStatus(Stage.SELECTROUNDBOOSTER);
                        GameStatus.SetPlayerIndexLast();
                    }
                    return ret;
                case Stage.SELECTROUNDBOOSTER:
                    ret = ProcessSyntaxRoundBoosterSelect(syntax, ref log);
                    if (ret)
                    {
                        GameStatus.NextPlayerReverse();
                    }
                    ///所有人都选完RBT了
                    if (ret && GameStatus.PlayerIndex + 1 == GameStatus.m_PlayerNumber)
                    {
                        ChangeGameStatus(Stage.ROUNDINCOME);
                        FactionList.ForEach(x => x.CalIncome());
                        ChangeGameStatus(Stage.ROUNDSTART);
                        GameStatus.SetPlayerIndexFirst();
                    }
                    return ret;
                case Stage.ROUNDSTART:
                    ret = ProcessSyntaxCommand(syntax, ref log);
                    return ret;

                default:
                    return false;
            }
        }

        private bool ProcessSyntaxCommand(string syntax, ref string log)
        {
            if (!(ValidateSyntaxCommand(syntax, ref log, out string command, out Faction faction)))
            {
                return false;
            }
            var commandList = command.Split('.');
            //非免费行动只能执行一个
            var NoneFreeActionCount=commandList.Sum(y => GameFreeSyntax.GetRegexList().Exists(x => x.IsMatch(y)) ? 0 : 1);
            if (NoneFreeActionCount != 1)
            {
                log = "能且只能执行一个普通行动";
                return false;
            }

            foreach(var item in commandList)
            {
                if (GameSyntax.updateRegex.IsMatch(item))
                {
                    var match=GameSyntax.updateRegex.Match(item);
                    var pos=match.Groups[1].Value;
                    var buildStr = match.Groups[2].Value;
                    ConvertPosToRowCol(pos, out int row, out int col);
                    if(!faction.UpdateBuilding(Map, row, col, buildStr, out log))
                    {
                        return false;
                    }
                }
            }

            return true;

        }

        private bool ProcessSyntaxRoundBoosterSelect(string syntax, ref string log)
        {
            if (!(ValidateSyntaxCommand(syntax, ref log, out string command, out Faction faction)))
            {
                return false;
            }
            ///处理SelectRBT命令
            if (!GameSyntax.RBTRegex.IsMatch(command))
            {
                log = "命令错误";
                return false;
            }
            var rbtStr = command.Substring(1);
            var rbt = RBTList.Find(x => x.GetType().Name.Equals(rbtStr, StringComparison.OrdinalIgnoreCase));
            if (rbt == null)
            {
                log = string.Format("{0}板子不存在", rbtStr);
                return false;
            }

            faction.GameTileList.Add(rbt);
            RBTList.Remove(rbt);

            return true;
        }

        private bool ProcessSyntaxIntialMines(string syntax, ref string log)
        {
            if (!(ValidateSyntaxCommand(syntax, ref log, out string command, out Faction faction)))
            {
                return false;
            }
            ///处理Build命令
            if (!GameSyntax.buildRegex.IsMatch(command))
            {
                log = "命令错误";
                return false;
            }
            ///Build A2
            var position = command.Substring(GameSyntax.factionSelection.Length + 1);

            ConvertPosToRowCol(position, out int row, out int col);

            if (faction.BuildIntialMine(Map, row, col, out log))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private static void ConvertPosToRowCol(string position, out int row, out int col)
        {
            row = position.Substring(0, 1).ToCharArray().First() - 'a';
            col = position.Substring(1).ParseToInt(0);
        }

        public bool ValidateSyntaxCommand(string syntax, ref string log, out string command, out Faction faction)
        {
            command = string.Empty;
            faction = null;
            if (!GameSyntax.commandRegex.IsMatch(syntax))
            {
                log = "格式为 种族:命令";
                return false;
            }

            var factionName = syntax.Split(':').First();
            if (!Enum.TryParse(factionName, true, out FactionName result))
            {
                log = "FactionName is wrong";
                return false;
            }
            if (!(FactionList[GameStatus.PlayerIndex].FactionName == result))
            {
                log = string.Format("不是种族{0}行动轮,是{1}行动轮", factionName, FactionList[GameStatus.PlayerIndex].FactionName.ToString());
                return false;
            }
            faction = FactionList.Find(x => x.FactionName == result);
            if (faction == null)
            {
                log = "FactionName doesn't exit";
                return false;
            }
            command = syntax.Split(':').Last();
            return true;
        }

        private bool ProcessSyntaxFactionSelect(string syntax, ref string log)
        {
            log = string.Empty;
            if (GameSyntax.factionSelectionRegex.IsMatch(syntax))
            {
                var faction = syntax.Substring(GameSyntax.factionSelection.Length + 1);
                if (Enum.TryParse(faction, true, out FactionName result))
                {
                    if (!FactionList.Exists(x => x.FactionName == result))
                    {
                        SetupFaction(result);
                    }
                    else
                    {
                        log = "FactionName has been choosen!";
                        return false;
                    }
                    if (FactionList.Count == 4)
                    {
                        ChangeGameStatus(Stage.INITIALMINES);
                    }
                    return true;
                }
                else
                {
                    log = "FactionName is wrong";
                }
            }
            return false;
        }

        private bool ProcessSyntaxRandomSetup(string syntax, ref string log)
        {
            log = string.Empty;
            if (GameSyntax.setupGameRegex.IsMatch(syntax))
            {
                var seed = syntax.Substring(GameSyntax.setupGame.Length).ParseToInt(0);
                GameStart(syntax, seed);
                ChangeGameStatus(Stage.FACTIONSELECTION);
                return true;
            }
            return false;
        }

        public void Syntax(string syntax, out string log)
        {
            if (ProcessSyntax(syntax, out log))
            {
                UserActionLog += syntax.AddEnter();
                UserActionLog += m_TailLog;
                m_TailLog = string.Empty;
            }
        }

        private void SetupFaction(FactionName faction)
        {
            switch (faction)
            {
                case FactionName.Terraner:
                    FactionList.Add(new Terraner());
                    break;
                case FactionName.Taklons:
                    FactionList.Add(new Taklons());
                    break;
                case FactionName.MadAndroid:
                    FactionList.Add(new MadAndroid());
                    break;
                case FactionName.Geoden:
                    FactionList.Add(new Geoden());
                    break;
                default:
                    break;
            };
        }

        private void GameStart(string syntax, int i = 0)
        {
            Seed = i == 0 ? RandomInstance.Next(int.MaxValue) : i;
            var random = new Random(Seed);
            Map = MapMgr.GetRandomMap(random);
            ATTList = (from items in ATTMgr.GetRandomList(6, random) orderby items.GetType().Name.Remove(0, 3).ParseToInt(-1) select items).ToList();
            STT6List = STTMgr.GetRandomList(6, random);
            STT3List = (from items in STTMgr.GetOtherList(STT6List) orderby items.GetType().Name.Remove(0, 3).ParseToInt(-1) select items).ToList();
            RSTList = RSTMgr.GetRandomList(6, random);
            FSTList = new List<FinalScoring>();
            FSTList.Add(new FST1());
            RBTList = (from items in RBTMgr.GetRandomList(4 + 3, random) orderby items.GetType().Name.Remove(0, 3).ParseToInt(-1) select items).ToList();
            ALTList = ALTMgr.GetList();
            AllianceTileForKnowledge = ALTList.RandomRemove(random);
        }
        private void ChangeGameStatus(Stage stage)
        {
            m_TailLog += "#" + stage.ToString().AddEnter();
            GameStatus.stage = stage;
        }
        /// <summary>
        /// 实例化四个玩家
        /// </summary>
        public List<Faction> FactionList { set; get; }
        #region 存档需要save的内容
        [JsonProperty]
        /// <summary>
        /// 在游戏开始的时候实例化一个Map
        /// </summary>
        public Map Map { set; get; }
        /// <summary>
        /// 游戏的一些公共状态包括现在第几轮轮到哪个玩家行动等等
        /// </summary>
        public GameStatus GameStatus { set; get; }
        /// <summary>
        /// StandardTechList
        /// </summary>
        public List<StandardTechnology> STT6List { set; get; }
        /// <summary>
        /// StandardTechListWithNoKnowledge
        /// </summary>
        public List<StandardTechnology> STT3List { set; get; }
        /// <summary>
        /// AdvanceTechList
        /// </summary>
        public List<AdavanceTechnology> ATTList { set; get; }
        /// <summary>
        /// RoundScoringList
        /// </summary>
        public List<RoundScoring> RSTList { set; get; }
        /// <summary>
        /// FinalScoringList
        /// </summary>
        public List<FinalScoring> FSTList { set; get; }
        /// <summary>
        /// RoundBoostList
        /// </summary>
        public List<RoundBooster> RBTList { set; get; }
        /// <summary>
        /// AlianceList
        /// </summary>
        public List<AllianceTile> ALTList { set; get; }
        public AllianceTile AllianceTileForKnowledge { set; get; }
        [JsonProperty]
        public string UserActionLog { set; get; }
        [JsonProperty]
        public int Seed { set; get; }
        [JsonProperty]
        public string[] Username { set; get; }
        #endregion
    }
}
