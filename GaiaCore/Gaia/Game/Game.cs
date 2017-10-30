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
            log = string.Empty;
            syntax = syntax.ToLower();

            if (GameSyntax.setupGameRegex.IsMatch(syntax))
            {
                var seed = syntax.Substring(GameSyntax.setupGame.Length).ParseToInt(0);
                GameStart(syntax, seed);
                return true;
            }
            else if (GameSyntax.factionSelectionRegex.IsMatch(syntax))
            {
                var faction = syntax.Substring(GameSyntax.factionSelection.Length + 1);
                if (Enum.TryParse(faction, true, out FactionName result))
                {
                    SetupFaction(result);
                    return true;
                }
                else
                {
                    log = "FactionName is wrong";
                    return false;
                }
            }
            else
            {
                log = "Syntax is wrong";
                return false;
            }
        }

        public void ProcessSyntax(string syntax)
        {
            if(ProcessSyntax(syntax,out string log))
            {
                UserActionLog += syntax.AddEnter();
            }
        }

        private void SetupFaction(FactionName faction)
        {
            switch (faction)
            {
                case FactionName.Ambas:
                    FactionList.Add(new Faction());
                    break;
                case FactionName.BalTak:
                    FactionList.Add(new Faction());
                    break;
                case FactionName.Firaks:
                    FactionList.Add(new Faction());
                    break;
                case FactionName.Geoden:
                    FactionList.Add(new Faction());
                    break;
                case FactionName.Gleen:
                    FactionList.Add(new Faction());
                    break;
                default:
                    FactionList.Add(new Faction());
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
            FactionList.Add(new Faction());
            FactionList.Add(new Faction());
            FactionList.Add(new Faction());
            FactionList.Add(new Faction());
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
