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
        public GaiaGame(string[] username)
        {
            GameStatus = new GameStatus();
            FactionList = new List<Faction>();
            Username = username;
        }
        public bool ProcessSyntax(string syntax, out string log)
        {
            log = "Syntax is wrong";
            syntax = syntax.ToLower();

            switch (GameStatus.stage)
            {
                case Stage.RANDOMSETUP:
                    return ProcessSyntaxRandomSetup(syntax, ref log);
                case Stage.FACTIONSELECTION:
                    return ProcessSyntaxFactionSelect(syntax, ref log);
                case Stage.INITIALMINES:
                    var ret=ProcessSyntaxIntialMines(syntax, ref log);
                    if (ret)
                    {
                        GameStatus.NextPlayerForIntial();
                    }
                    if(ret&& FactionList.All(x=>x.FinishIntialMines())){
                        GameStatus.stage = Stage.SELECTROUNDBOOSTER;
                    }
                    return ret;
                case Stage.SELECTROUNDBOOSTER:
                    log = "未完成";
                    return false;
                default:
                    return false;
            }
        }     

        private bool ProcessSyntaxIntialMines(string syntax, ref string log)
        {
            if (GameSyntax.commandRegex.IsMatch(syntax))
            {
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
                var faction = FactionList.Find(x => x.FactionName == result);
                if (faction == null)
                {
                    log = "FactionName doesn't exit";
                    return false;
                }
                var command = syntax.Split(':').Last();
                ///处理Build命令
                if (GameSyntax.buildRegex.IsMatch(command))
                {
                    ///Build A2
                    var position = command.Substring(GameSyntax.factionSelection.Length + 1);
                    var row = position.Substring(0, 1).ToCharArray().First() - 'a';
                    var col = position.Substring(1).ParseToInt(0);
                    if (faction.BuildMine(Map, row, col, out log))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                
            }
            return false;
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
                        GameStatus.stage = Stage.INITIALMINES;
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

        private bool ProcessSyntaxRandomSetup(string syntax,ref string log)
        {
            log = string.Empty;
            if (GameSyntax.setupGameRegex.IsMatch(syntax))
            {
                var seed = syntax.Substring(GameSyntax.setupGame.Length).ParseToInt(0);
                GameStart(syntax, seed);
                GameStatus.stage = Stage.FACTIONSELECTION;
                return true;
            }
            return false;
        }

        public void Syntax(string syntax,out string log)
        {
            if(ProcessSyntax(syntax,out log))
            {
                UserActionLog += syntax.AddEnter();
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
        private void SetupPlayer()
        {
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
